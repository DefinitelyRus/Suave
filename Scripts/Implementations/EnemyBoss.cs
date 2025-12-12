using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyBoss : Enemy {
	public EnemyBoss(Vector2 position) : base(
		name: "Boss",
		entityId: "EnemyBoss",
		position: position,
		hitRadius: 32f,
		maxHealth: 150,
		damage: 2,
		attackRange: 256f,
		attackCooldown: 0.8f,
		moveSpeed: 96f,
		aggroRange: 392f,
		avoidRange: 48f
		) {
		Name = "Boss";
		EntityId = "EnemyBoss";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Boss");
	}
}
