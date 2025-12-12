using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemySniper : Enemy {
	public EnemySniper(Vector2 position) : base(
		name: "Sniper",
		entityId: "EnemySniper",
		position: position,
		hitRadius: 32f,
		maxHealth: 12,
		damage: 3,
		attackRange: 300f,
		attackCooldown: 3f,
		moveSpeed: 24f,
		aggroRange: 450f,
		avoidRange: 64f
		) {
		Name = "Sniper";
		EntityId = "EnemySniper";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Sniper");
	}
}
