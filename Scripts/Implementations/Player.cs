using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
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
		SoundPlayer.Play("Player - Near Miss");
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

		//TODO: AVFX here.
		SoundPlayer.Play("Player - Parry");

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
			int totalDamage = Damage + DamageBonus + DashHitDamageBonus;
			bool willKill = enemies[0].Health <= totalDamage;
			enemies[0].TakeDamage(totalDamage);
			DamageBonus = 0;

			//TODO: AVFX here.
			if (willKill) SoundPlayer.Play("Player - Dash Kill");
			else SoundPlayer.Play("Player - Dash Hit");

			return;
		}
	}

	#endregion
}
