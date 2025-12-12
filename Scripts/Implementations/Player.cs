using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Implementations;

internal class Player(
	string name,
	string entityId,
	Vector2 position,
	Projectile projectile,
	float hitRadius = 16,
	int maxHealth = 10,
	int damage = 1,
	float attackRange = 64,
	float attackCooldown = 1f,
	float moveSpeed = 100f,
	float nearMissRange = 48.0f,
	float damageCooldown = 0.5f,
	float parryTolerance = 0.8f,
	float dashDistance = 150.0f,
	float dashHitCooldown = 0.5f,
	float dashMissCooldown = 2.0f,
	float dashHitTolerance = 0.9f
	) :
	Character(
		name,
		entityId,
		position,
		projectile,
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

		base.Update(delta);
	}

	#endregion

	#region Dodging & Taking Damage

	public int DamageBonus { get; protected set; } = 0;

	public float NearMissRange { get; protected set; } = nearMissRange;

	public float DamageCooldown { get; protected set; } = damageCooldown;

	private float DamageCooldownRemaining { get; set; } = 0.0f;

	public void CheckNearMiss() {
		// Dodges only trigger if Player can also take damage.
		if (DamageCooldownRemaining > 0) return;

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
			Character self = this;
			self.TakeDamage(projectile.Owner.Damage);

			Kill();
		}
	}

	public override void Kill() {
		//TODO: AVFX here.

		Despawn();
	}

	#endregion

	#region Parrying

	public float ParryRange => AttackRange;

	public float ParryTolerance { get; protected set; } = parryTolerance;

	/// <summary>
	/// For the Player character, attacks are always parries.
	/// </summary>
	public void Attack() {
		if (AttackCooldownRemaining > 0) return;

		// Check if there is an enemy within `ParryRange` in the `FaceDirection`.
		Projectile[] projectiles = [.. EntityManager
			.GetAllEntitiesInRadius<Projectile>(Position, ParryRange)
			.Where(e => Vector2.Dot(Vector2.Normalize(e.Position - Position), FaceDirection) > ParryTolerance)
			.OrderBy(p => Vector2.Distance(Position, p.Position))
		];

		// No projectiles to parry.
		if (projectiles.Length == 0) {
			AttackCooldownRemaining = AttackCooldown;
			return;
		}

		Projectile projectile = projectiles[0];

		// Parry the projectile: Heal self for half damage, deal half damage to owner.
		int effectOnTarget = (int) Math.Round(projectile.Owner.Damage / 2f);
		Health = Math.Min(Health + effectOnTarget, MaxHealth);
		projectile.Owner.TakeDamage(effectOnTarget);

		//TODO: AVFX here.

		AttackCooldownRemaining = AttackCooldown;
	}

	#endregion

	#region Dashing

	public float DashDistance { get; protected set; } = dashDistance;

	public float DashHitCooldown { get; protected set; } = dashHitCooldown;

	public float DashMissCooldown { get; protected set; } = dashMissCooldown;

	private float DashCooldownRemaining { get; set; } = 0.0f;

	public float DashHitTolerance { get; protected set; } = dashHitTolerance;

	public void Dash() {
		if (DashCooldownRemaining > 0) return;

		//Check if there are any enemies in the dash direction within a certain range.
		Enemy[] enemies = [.. EntityManager
			.GetAllEntitiesInRadius<Character>(Position, DashDistance)
			.OfType<Enemy>()
			.Where(e => Vector2.Dot(Vector2.Normalize(e.Position - Position), FaceDirection) > DashHitTolerance)
			.OrderBy(e => Vector2.Distance(Position, e.Position))
		];

		//TODO: AVFX here.

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

			//TODO: AVFX here.

			return;
		}
	}

	#endregion
}
