using System.Numerics;
using Suave.Scripts.Implementations;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Entities;

internal abstract class Enemy(
	string name,
	string entityId,
	Vector2 position,
	float hitRadius = 16,
	int maxHealth = 10,
	int damage = 1,
	float attackRange = 64,
	float attackCooldown = 1f,
	float moveSpeed = 100f,
	float aggroRange = 200f,
	float avoidRange = 50f
	) : Character(
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

	public virtual void Attack(Character target) {
		if (AttackCooldownRemaining > 0) return;

		Bullet bullet = new(this);
		Vector2 directionToTarget = Vector2.Normalize(target.Position - Position);
		bullet.Launch(directionToTarget);

		AttackCooldownRemaining = AttackCooldown;
	}

	public override void Update(float delta) {
		base.Update(delta);

		if (EntityManager.Player == null) return;

		UpdateState(EntityManager.Player, delta);
		EnactStateBehavior(EntityManager.Player, delta);
	}

	#endregion

	#region States

	public enum EnemyState {
		Idle,
		Aggro,
		Attacking,
		Avoiding
	}

	public EnemyState CurrentState { get; protected set; } = EnemyState.Idle;

	private void UpdateState(Character targetCharacter, float delta) {
		float distanceToTarget = Vector2.Distance(Position, targetCharacter.Position);
		
		if (AvoidCharacter(delta)) return;
		else if (distanceToTarget <= AttackRange * 0.9f) CurrentState = EnemyState.Attacking;
		else if (distanceToTarget <= AggroRange) CurrentState = EnemyState.Aggro;
		else CurrentState = EnemyState.Idle;
	}

	private void EnactStateBehavior(Character targetCharacter, float delta) {
		switch (CurrentState) {
			case EnemyState.Idle:
				break;

			case EnemyState.Aggro:
				FaceDirection = Vector2.Normalize(targetCharacter.Position - Position);
				MoveTowardsPosition(targetCharacter.Position, delta);
				break;

			case EnemyState.Attacking:
				FaceDirection = Vector2.Normalize(targetCharacter.Position - Position);
				Attack(targetCharacter);
				break;
		}
	}

	#endregion

	#region Avoidance

	public float AggroRange { get; protected set; } = aggroRange;
	public float AvoidRange { get; protected set; } = avoidRange;

	public const float AvoidSpeedMultiplier = 2f;
	public const float AvoidDistanceBuffer = 1.2f;

	public bool AvoidCharacter(float delta) {
		// Find the nearest character to avoid
		Character? targetCharacter = EntityManager
			.GetAllEntitiesInRadius<Character>(Position, AvoidRange)
			.Where(c => c != this)
			.OrderBy(c => Vector2.Distance(Position, c.Position))
			.FirstOrDefault();

		// No character to avoid
		if (targetCharacter == null) return false;

		// Move away from the target character
		Vector2 directionAwayFromTarget = Vector2.Normalize(Position - targetCharacter.Position);
		Vector2 targetPosition = Position + directionAwayFromTarget * AvoidRange * AvoidDistanceBuffer;
		MoveTowardsPosition(targetPosition, delta * AvoidSpeedMultiplier);
		return true;
	}

	#endregion
}
