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

	private const float SprintSpeedMultiplier = 1.5f; // 50% speed increase
	public bool IsSprinting { get; set; } = false;

	public override void Update(float delta) {
		if (DashCooldownRemaining > 0) {
			DashCooldownRemaining -= delta;
			if (DashCooldownRemaining < 0) DashCooldownRemaining = 0;
		}

		if (LastEnemyResetTimeRemaining > 0) {
			LastEnemyResetTimeRemaining -= delta;
			if (LastEnemyResetTimeRemaining < 0) LastDashedEnemy = null;
		}

		CheckNearMiss(delta);

		base.Update(delta);
	}

	public override void ResetContemporaryValues() {
		DamageBonus = 0;
		DamageCooldownRemaining = 3f;
		DashCooldownRemaining = 0f;
		CurrentAnimation = null;
		IsSprinting = false;
	}

	public override void Render(float delta) {
		if (CurrentAnimation == null) CurrentTexture = ResourceManager.GetTexture("Player");
		else {
			CurrentAnimation.Update(delta);
			CurrentTexture = CurrentAnimation.CurrentTexture;

			if (CurrentAnimation.IsFinished) CurrentAnimation = null;
		}

		Vector2 renderPosition = Position + new Vector2(0, GetWobbleOffset());
		SpriteRenderer.Render(CurrentTexture, renderPosition, FaceDirection, 0.15f);

		// TODO: Move to UIManager

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

	#region Animations

	private Animation? CurrentAnimation { get; set; } = null;

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

	/// <summary>
	/// Override to apply sprint multiplier to movement speed.
	/// </summary>
	public override void MoveTowardsDirection(Vector2 direction, float delta) {
		float currentMoveSpeed = MoveSpeed;
		if (IsSprinting) {
			MoveSpeed *= SprintSpeedMultiplier;
		}
		base.MoveTowardsDirection(direction, delta);
		MoveSpeed = currentMoveSpeed;
	}

	/// <summary>
	/// Override to apply sprint multiplier to movement speed.
	/// </summary>
	public override void MoveTowardsPosition(Vector2 targetPosition, float delta) {
		float currentMoveSpeed = MoveSpeed;
		if (IsSprinting) {
			MoveSpeed *= SprintSpeedMultiplier;
		}
		base.MoveTowardsPosition(targetPosition, delta);
		MoveSpeed = currentMoveSpeed;
	}

	#endregion

	#region Parrying

	public float ParryRange => AttackRange;

	private float ParryMissCooldown => AttackCooldown;

	private const float ParryHitCooldown = 0.4f;

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
		
		LastDashedEnemy = null;
		CurrentAnimation = new Animation("Player - Attack", ParryHitCooldown * 1.0f, false);

		// No projectiles to parry.
		if (projectiles.Length == 0) {
			AttackCooldownRemaining = ParryMissCooldown;
			return;
		}

		// Parry all projectiles
		foreach (Projectile projectile in projectiles) {
			projectile.Parry(FaceDirection, this);

			// Heal self
			int newHealth = Health + projectile.Owner.Damage;
			Health = Math.Min(newHealth, MaxHealth);

			// Increase damage bonus by 3x the damage
			DamageBonus += projectile.Owner.Damage * 3;
		}

		// AVFX
		SoundPlayer.Play("Player - Parry");
		_ = new ParticleParry(Position, FaceDirection);

		AttackCooldownRemaining = ParryHitCooldown;
	}

	#endregion

	#region Dashing

	public const float DashDistance = 160f;

	public const float DashHitCooldown = 0.1f;

	public const float DashMissCooldown = 2f;

	private float DashCooldownRemaining { get; set; } = 0f;

	public const float DashHitTolerance = 0.9f;

	public const int DashHitDamageBonus = 2;

	private const float LastEnemyResetTime = 2f;

	private float LastEnemyResetTimeRemaining = 0f;

	private Enemy? LastDashedEnemy = null;

	public void Dash() {
		if (DashCooldownRemaining > 0) return;

		// AVFX
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
			LastDashedEnemy = null;

			_ = new ParticleTeleport(Position, FaceDirection);

			return;
		}

		// Dash to the closest enemy
		else {
			Enemy enemy = enemies[0];

			Position = enemy.Position;
			DashCooldownRemaining = DashHitCooldown;

			int totalDamage = 0;

			// Repeated dash on same enemy
			if (LastDashedEnemy == enemy) {
				Log.Me(() => "Repeated dash on same enemy!");
				totalDamage = Damage;
				TakeDamage(Damage * 4); // Take 4x self-damage
				DamageBonus = 0;
			}

			// Dash on new enemy
			else {
				Log.Me(() => "Dash on new enemy!");
				totalDamage = Damage + DamageBonus + DashHitDamageBonus;

				int newDamageBonus = totalDamage - enemy.Health;
				DamageBonus = Math.Max(0, newDamageBonus);
			}

			// Set this enemy as last dashed enemy
			LastDashedEnemy = enemy;
			LastEnemyResetTimeRemaining = LastEnemyResetTime;

			bool willKill = enemy.Health <= totalDamage;
			enemy.TakeDamage(totalDamage);

			// AVFX + Heal on kill
			if (!willKill) SoundPlayer.Play("Player - Dash Hit");
			else {
				SoundPlayer.Play("Player - Dash Kill");

				// Heal player for 25% of damage dealt
				int newHealth = totalDamage / 4;
				Health = Math.Min(MaxHealth, Health + newHealth);
			}

			_ = new ParticleTeleport(Position, FaceDirection);

			CurrentAnimation = new Animation("Player - Attack", DashHitCooldown * 1.4f, false);

			return;
		}
	}

	#endregion

}
