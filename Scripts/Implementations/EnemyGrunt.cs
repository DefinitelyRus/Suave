using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyGrunt : Enemy {
	public EnemyGrunt(Vector2 position) : base(
		name: "Grunt",
		entityId: "EnemyGrunt",
		position: position,
		hitRadius: 32f,
		maxHealth: 20,
		damage: 1,
		attackRange: 128f,
		attackCooldown: 1f,
		moveSpeed: 64f,
		aggroRange: 156f,
		avoidRange: 48f
		)
	{
		Name = "Grunt";
		EntityId = "EnemyGrunt";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Grunt");
	}
}
