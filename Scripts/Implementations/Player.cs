using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
using Suave.Scripts.Tools;
namespace Suave.Scripts.Implementations;

internal class Player(
	string name,
	string entityId,
	Vector2 position,
	float hitRadius,
	int maxHealth,
	int damage,
	float attackRange,
	float attackCooldown,
	float moveSpeed,
	float nearMissRange,
	float damageCooldown,
	float parryTolerance,
	float dashDistance,
	float dashHitCooldown,
	float dashMissCooldown,
	float dashHitTolerance
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

		CheckNearMiss(delta);

		base.Update(delta);
	}

	public override void ResetContemporaryValues() {
		DamageBonus = 0;
		DamageCooldownRemaining = 3f;
		DashCooldownRemaining = 0f;
	}

	#endregion

	#region Dodging & Taking Damage

	public int DamageBonus { get; protected set; } = 0;

	public float NearMissRange { get; protected set; } = nearMissRange;

	public float DamageCooldown { get; protected set; } = damageCooldown;

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

			Character self = this;
			self.TakeDamage(projectile.Owner.Damage);

			return;
		}

		Log.Me(() => $"Near miss detected! Projectile at distance {distanceToProjectile}.");

		// Successful dodge.
		DamageBonus += 1;
		DamageCooldownRemaining = DamageCooldown;
		DodgeCooldownRemaining = DodgeCooldown;

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Near Miss");
	}

	#endregion

	#region Parrying

	public float ParryRange => AttackRange;

	public float ParryTolerance { get; protected set; } = parryTolerance;

	/// <summary>
	/// For the Player character, attacks are always parries.
	/// </summary>
	public void Parry() {
		if (AttackCooldownRemaining > 0) return;

		// Check if there is an enemy within `ParryRange` in the `FaceDirection`.
		Projectile[] projectiles = [.. EntityManager
			.GetAllEntitiesInRadius<Projectile>(Position, ParryRange)
			.Where(e => Vector2.Dot(Vector2.Normalize(e.Position - Position), FaceDirection) > ParryTolerance)
			.OrderBy(p => Vector2.Distance(Position, p.Position))
		];

		// No projectiles to parry.
		if (projectiles.Length == 0) {
			Log.Me(() => "Parry failed!");
			AttackCooldownRemaining = AttackCooldown;
			return;
		}

		// Parry the closest projectile.
		Projectile projectile = projectiles[0];

		// Parry the projectile: Heal self for half damage, deal half damage to owner.
		int effectOnTarget = (int) Math.Round(projectile.Owner.Damage / 2f);
		Health = Math.Min(Health + effectOnTarget, MaxHealth);

		// Reflect the projectile
		projectile.Parry(FaceDirection, this);

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Parry");

		AttackCooldownRemaining = AttackCooldown;
	}

	#endregion

	#region Dashing

	public float DashDistance { get; protected set; } = dashDistance;

	public float DashHitCooldown { get; protected set; } = dashHitCooldown;

	public float DashMissCooldown { get; protected set; } = dashMissCooldown;

	private float DashCooldownRemaining { get; set; } = 0.0f;

	public float DashHitTolerance { get; protected set; } = dashHitTolerance;

	public const int DashHitDamageBonus = 2;

	public void Dash() {
		if (DashCooldownRemaining > 0) return;

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Dash");

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

			return;
		}

		// Dash to the closest enemy
		else {
			Position = enemies[0].Position;
			DashCooldownRemaining = DashHitCooldown;

			// Damage Enemy
			enemies[0].TakeDamage(Damage + DamageBonus + DashHitDamageBonus);

			//TODO: AVFX here.

			return;
		}
	}

	#endregion
}
