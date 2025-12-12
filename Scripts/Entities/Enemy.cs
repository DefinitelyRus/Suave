using System.Numerics;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Entities;

internal abstract class Enemy(
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
	float aggroRange = 200f,
	float avoidRange = 50f
	) : Character(
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

	private void UpdateState(Character targetCharacter, float _) {
		float distanceToTarget = Vector2.Distance(Position, targetCharacter.Position);

		if (distanceToTarget <= AvoidRange) CurrentState = EnemyState.Avoiding;
		else if (distanceToTarget <= AttackRange * 0.9f) CurrentState = EnemyState.Attacking;
		else if (distanceToTarget <= AggroRange) CurrentState = EnemyState.Aggro;
		else CurrentState = EnemyState.Idle;
	}

	private void EnactStateBehavior(Character targetCharacter, float delta) {
		switch (CurrentState) {
			case EnemyState.Idle:
				break;

			case EnemyState.Aggro:
				MoveTowards(targetCharacter.Position, delta);
				break;

			case EnemyState.Attacking:
				Attack(targetCharacter);
				break;

			case EnemyState.Avoiding:
				AvoidCharacter(targetCharacter, delta);
				break;
		}
	}

	#endregion

	#region Avoidance

	public float AggroRange { get; protected set; } = aggroRange;
	public float AvoidRange { get; protected set; } = avoidRange;

	public const float AvoidSpeedMultiplier = 2f;
	public const float AvoidDistanceBuffer = 1.2f;

	public void AvoidCharacter(Character targetCharacter, float delta) {
		Vector2 directionAwayFromTarget = Vector2.Normalize(Position - targetCharacter.Position);
		Vector2 targetPosition = Position + directionAwayFromTarget * AvoidRange * AvoidDistanceBuffer;
		MoveTowards(targetPosition, delta * AvoidSpeedMultiplier);
	}

	#endregion
}
