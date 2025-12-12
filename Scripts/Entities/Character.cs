using System.Numerics;

namespace Suave.Scripts.Entities;

internal abstract class Character(
	string name,
	string entityId,
	Vector2 position,
	Projectile projectile,
	float hitRadius = 16,
	int maxHealth = 10,
	int damage = 1,
	float attackRange = 64,
	float attackCooldown = 1f
	) :
	PhysicalEntity(
		name,
		entityId,
		position,
		hitRadius
	) {

	#region General

	public override void Update(float delta) {
		if (AttackCooldownRemaining > 0) {
			AttackCooldownRemaining -= delta;
			if (AttackCooldownRemaining < 0) AttackCooldownRemaining = 0;
		}
	}

	#endregion

	#region Combat

	public int Health { get; protected set; } = maxHealth;
	public int MaxHealth { get; protected set; } = maxHealth;
	public int Damage { get; protected set; } = damage;
	public float AttackRange { get; protected set; } = attackRange;
	public float AttackCooldown { get; protected set; } = attackCooldown;
	protected float AttackCooldownRemaining { get; set; } = 0f;
	public Projectile Projectile { get; protected set; } = projectile;

	public void Attack(Character target) {
		if (AttackCooldownRemaining > 0) return;

		Vector2 directionToTarget = Vector2.Normalize(target.Position - Position);

		Projectile.Instantiate(directionToTarget);
		AttackCooldownRemaining = AttackCooldown;
	}

	public void TakeDamage(int amount) {
		Health -= amount;

		if (Health < 0) {
			Health = 0;
			Kill();
		}
	}

	public abstract void Kill();

	#endregion

	#region Movement

	public Vector2 FaceDirection { get; protected set; }
	public Vector2 MoveDirection { get; protected set; }
	public float MoveSpeed { get; protected set; }

	/// <summary>
	/// Face towards the target position.
	/// </summary>
	/// <param name="targetPosition">The position to face towards.</param>
	/// <param name="_">An unused delta parameter for enforcing use only within <see cref="Update(float)"/>.</param>
	public void FaceTowards(Vector2 targetPosition, float _) => FaceDirection = Vector2.Normalize(targetPosition - Position);

	/// <summary>
	/// Face towards the target position.
	/// </summary>
	/// <param name="target">The position to face towards.</param>
	/// <param name="delta">An unused delta parameter for enforcing use only within <see cref="Update(float)"/>.</param>
	public void FaceTowards(Entity target, float delta) {
		FaceTowards(target.Position, delta);
	}

	/// <summary>
	/// Move towards the target position.
	/// </summary>
	/// <param name="targetPosition">The position to move towards.</param>
	/// <param name="delta">The time the current and the previous frame.</param>
	public void MoveTowards(Vector2 targetPosition, float delta) {
		MoveDirection = Vector2.Normalize(targetPosition - Position);

		Position += MoveDirection * MoveSpeed * delta;
	}

	#endregion
}
