using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Implementations;

internal class Player(
	string name,
	string entityId,
	Projectile projectile,
	float hitRadius = 16,
	int maxHealth = 10,
	int damage = 1,
	float attackRange = 64,
	float attackCooldown = 1f,
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
		projectile,
		hitRadius,
		maxHealth,
		damage,
		attackRange,
		attackCooldown
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

	public override void Kill() {
		//TODO: AVFX here.

		Despawn();
	}

	#endregion

	#region Parrying

	public float ParryRange => AttackRange;

	public float ParryTolerance { get; protected set; } = parryTolerance;


	#endregion

	#region Dashing

	public float DashDistance { get; protected set; } = dashDistance;

	public float DashHitCooldown { get; protected set; } = dashHitCooldown;

	public float DashMissCooldown { get; protected set; } = dashMissCooldown;

	private float DashCooldownRemaining { get; set; } = 0.0f;

	public float DashHitTolerance { get; protected set; } = dashHitTolerance;

	#endregion
}
