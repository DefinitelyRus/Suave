using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyBerserker : Enemy {
	public EnemyBerserker(Vector2 position) : base(
		name: "Berserker",
		entityId: "EnemyBerserker",
		position: position,
		hitRadius: 32f,
		maxHealth: 20,
		damage: 1,
		attackRange: 72f,
		attackCooldown: 1f,
		moveSpeed: 24f,
		aggroRange: 108f,
		avoidRange: 48f
		) {
		Name = "Berserker";
		EntityId = "EnemyBerserker";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Berserker");
	}
}
