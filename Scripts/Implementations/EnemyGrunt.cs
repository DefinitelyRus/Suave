using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyGrunt : Enemy {
	public EnemyGrunt(Vector2 position) : base(
		name: "Grunt",
		entityId: "EnemyGrunt",
		position: position,
		hitRadius: 50f,
		maxHealth: 12,
		damage: 1,
		attackRange: 200f,
		attackCooldown: 1f,
		moveSpeed: 80f,
		aggroRange: 500f,
		avoidRange: 50f
		) {
		Name = "Grunt";
		EntityId = "EnemyGrunt";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Grunt");
	}

	public override void Attack(Character target) {
		// TODO: AVFX here.
		if (AttackCooldownRemaining > 0) return;
		SoundPlayer.Play("Grunt - Attack");

		base.Attack(target);
	}
}
