using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
using Suave.Scripts.Objects;
using Suave.Scripts.Tools;
namespace Suave.Scripts.Implementations;

internal class Player(
	string name,
	string entityId,
	Vector2 position,
	float hitRadius = 16f,
	int maxHealth = 25,
	int damage = 1,
	float attackRange = 72f,
	float attackCooldown = 1f,
	float moveSpeed = 200f
	) :
	Character(
		name,
		entityId,
		position,
		hitRadius,
		maxHealth,
		damage,
		attackRange,
		attackCooldown,
		moveSpeed
	) {

	#region General

	public override void Update(float delta) {
		if (DashCooldownRemaining > 0) {
			DashCooldownRemaining -= delta;
			if (DashCooldownRemaining < 0) DashCooldownRemaining = 0;
		}

		// Update parry animation if active
		if (CurrentParryAnimation != null) {
			CurrentParryAnimation.Update(delta);
			CurrentTexture = CurrentParryAnimation.CurrentTexture;
			
			// Reset to idle when animation finishes
			if (CurrentParryAnimation.IsFinished) {
				CurrentParryAnimation = null;
				CurrentTexture = default;
			}
		}

		CheckNearMiss(delta);

		base.Update(delta);
	}

	public override void ResetContemporaryValues() {
		DamageBonus = 0;
		DamageCooldownRemaining = 3f;
		DashCooldownRemaining = 0f;
		CurrentParryAnimation = null;
	}

	public override void Render(float _) {
		// If animation is active, render the animation frame; otherwise render normally
		if (CurrentParryAnimation != null && CurrentTexture.Id != 0) {
			SpriteRenderer.Render(CurrentTexture, Position, FaceDirection, 0.15f);
		} else {
			base.Render(_);
		}

		// Draw parry range marker only if parry is not on cooldown
		if (AttackCooldownRemaining <= 0) {
			// Get the angle of the facing direction in radians
			float facingAngle = Utilities.GetAngle(FaceDirection) * MathF.PI / 180f;
			
			// Draw an arc in front of the player (180 degrees, from -90 to +90 relative to facing direction)
			int segments = 32;
			for (int i = 0; i < segments; i++) {
				float angle1 = facingAngle - MathF.PI / 2 + (MathF.PI / segments) * i;
				float angle2 = facingAngle - MathF.PI / 2 + (MathF.PI / segments) * (i + 1);
				
				Vector2 p1 = Position + new Vector2(MathF.Cos(angle1), MathF.Sin(angle1)) * ParryRange;
				Vector2 p2 = Position + new Vector2(MathF.Cos(angle2), MathF.Sin(angle2)) * ParryRange;
				
				Raylib.DrawLineV(p1, p2, Color.Gold);
			}
		}
	}

	#endregion

	#region Dodging & Taking Damage

	public int DamageBonus { get; protected set; } = 0;

	public const float NearMissRange = 50;

	public const float DamageCooldown = 0.5f;

	private float DamageCooldownRemaining { get; set; } = 0.0f;

	private const float DodgeCooldown = 0.2f;

	private float DodgeCooldownRemaining { get; set; } = 0.0f;

	public void CheckNearMiss(float delta) {
		// Dodges only trigger if Player can also take damage.
		if (DamageCooldownRemaining <= 0) return;

		// Handle Dodge Cooldown
		if (DodgeCooldownRemaining > 0) {
			DodgeCooldownRemaining -= delta;
			if (DodgeCooldownRemaining < 0) DodgeCooldownRemaining = 0;

			return;
		}

		Projectile[] projectiles = [.. EntityManager
			.GetAllEntitiesInRadius<Projectile>(Position, HitRadius + NearMissRange)
			.Where(e => Vector2.Dot(Vector2.Normalize(e.Position - Position), FaceDirection) > 0.5f)
			.OrderBy(p => Vector2.Distance(Position, p.Position))
		];

		if (projectiles.Length == 0) return;

		Projectile projectile = projectiles[0];
		float distanceToProjectile = Vector2.Distance(Position, projectile.Position);

		// Failed to dodge.
		if (distanceToProjectile <= HitRadius) {
			Log.Me(() => $"Dodge failed! Projectile at distance {distanceToProjectile}.");

			TakeDamage(projectile.Owner.Damage);

			// Reset Damage Bonus
			int newDamageBonus = DamageBonus - projectile.Owner.Damage * 2;
			DamageBonus = Math.Max(0, newDamageBonus);

			return;
		}

		// Successful dodge.
		DamageBonus += 1;
		DamageCooldownRemaining = DamageCooldown;
		DodgeCooldownRemaining = DodgeCooldown;

		//TODO: AVFX here.
		_ = new ParticleNearMiss(Position, FaceDirection);
		SoundPlayer.Play("Player - Near Miss");
	}

	public override void Despawn() {
		base.Despawn();

		StateManager.CurrentState = StateManager.States.Lose;
	}

	#endregion

	#region Parrying

	public float ParryRange => AttackRange;

	private float ParryMissCooldown => AttackCooldown;

	private const float ParryHitCooldown = 0.4f;

	private Animation? CurrentParryAnimation { get; set; } = null;

	/// <summary>
	/// 
	/// </summary>
	public void Parry() {
		if (AttackCooldownRemaining > 0) return;

		// Check if there is an enemy within `ParryRange` in the `FaceDirection`.
		Projectile[] projectiles = [.. EntityManager
			.GetAllEntitiesInRadius<Projectile>(Position, ParryRange)
			.OrderBy(p => Vector2.Distance(Position, p.Position))
		];

		// No projectiles to parry.
		if (projectiles.Length == 0) {
			Log.Me(() => "Parry failed!");
			AttackCooldownRemaining = ParryMissCooldown;
			return;
		}

		// Parry the closest projectile.
		Projectile projectile = projectiles[0];
		projectile.Parry(FaceDirection, this);

		// Heal self for 3x the damage
		int triple = projectile.Owner.Damage * 3;
		int newHealth = Math.Min(Health + triple, MaxHealth);
		Health = Math.Min(Health + triple, MaxHealth);

		// Increase damage bonus by 3x the damage
		DamageBonus += triple;

		// Start parry animation with Attack-2 and Attack-3 frames
		List<Texture2D> parryFrames = [
			ResourceManager.GetTexture("Attack-2"),
			ResourceManager.GetTexture("Attack-3")
		];
		CurrentParryAnimation = new Animation("Parry", ParryHitCooldown * 1.4f, false);
		// Manually set the frames since we're using non-standard naming
		if (parryFrames[0].Id != 0 && parryFrames[1].Id != 0) {
			CurrentParryAnimation.SetFrames(parryFrames);
		}

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Parry");
		_ = new ParticleParry(Position, FaceDirection);

		AttackCooldownRemaining = ParryHitCooldown;
	}

	#endregion

	#region Dashing

	public const float DashDistance = 160f;

	public const float DashHitCooldown = 0.1f;

	public const float DashMissCooldown = 2.0f;

	private float DashCooldownRemaining { get; set; } = 0.0f;

	public const float DashHitTolerance = 0.9f;

	public const int DashHitDamageBonus = 2;

	public void Dash() {
		if (DashCooldownRemaining > 0) return;

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Dash");
		_ = new ParticleDash(Position, FaceDirection);

		//Check if there are any enemies in the dash direction within a certain range.
		Enemy[] enemies = [.. EntityManager
			.GetAllEntitiesInRadius<Character>(Position, DashDistance)
			.OfType<Enemy>()
			.Where(e => Vector2.Dot(Vector2.Normalize(e.Position - Position), FaceDirection) > DashHitTolerance)
			.OrderBy(e => Vector2.Distance(Position, e.Position))
		];

		// Dash normally, apply cooldown.
		if (enemies.Length == 0) {
			Position += FaceDirection * DashDistance;
			DashCooldownRemaining = DashMissCooldown;

			_ = new ParticleTeleport(Position, FaceDirection);

			return;
		}

		// Dash to the closest enemy
		else {
			Position = enemies[0].Position;
			DashCooldownRemaining = DashHitCooldown;

			// Damage Enemy
			int totalDamage = Damage + DamageBonus + DashHitDamageBonus;
			bool willKill = enemies[0].Health <= totalDamage;
			enemies[0].TakeDamage(totalDamage);
			DamageBonus = 0;

			// Start attack animation
			List<Texture2D> attackFrames = [
				ResourceManager.GetTexture("Attack-2"),
				ResourceManager.GetTexture("Attack-3")
			];
			CurrentParryAnimation = new Animation("Attack", DashHitCooldown * 1.4f, false);
			// Manually set the frames since we're using non-standard naming
			if (attackFrames[0].Id != 0 && attackFrames[1].Id != 0) {
				CurrentParryAnimation.SetFrames(attackFrames);
			}

			//TODO: AVFX here.
			if (willKill) SoundPlayer.Play("Player - Dash Kill");
			else SoundPlayer.Play("Player - Dash Hit");
			_ = new ParticleTeleport(Position, FaceDirection);

			return;
		}
	}

	#endregion
}
