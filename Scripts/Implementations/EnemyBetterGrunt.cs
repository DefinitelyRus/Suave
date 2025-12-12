using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Implementations;

internal class EnemyBetterGrunt : Enemy {
	public EnemyBetterGrunt(Vector2 position) : base(
		name: "Senior Grunt",
		entityId: "EnemyGrunt",
		position: position,
		hitRadius: 32f,
		maxHealth: 24,
		damage: 2,
		attackRange: 128f,
		attackCooldown: 1f,
		moveSpeed: 64f,
		aggroRange: 192f,
		avoidRange: 48f
		) {
		Name = "Senior Grunt";
		EntityId = "EnemyBetterGrunt";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Better Grunt");
	}
}
