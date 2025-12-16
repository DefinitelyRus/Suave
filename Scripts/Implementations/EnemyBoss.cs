using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyBoss : Enemy {
	public EnemyBoss(Vector2 position) : base(
		name: "Boss",
		entityId: "EnemyBoss",
		position: position,
		hitRadius: 50f,
		maxHealth: 50,
		damage: 2,
		attackRange: 650f,
		attackCooldown: 0.25f,
		moveSpeed: 64f,
		aggroRange: 1500f,
		avoidRange: 50f
		) {
		Name = "Boss";
		EntityId = "EnemyBoss";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("EnemyBoss");
	}

	public override void Attack(Character target) {
		// TODO: AVFX here.
		if (AttackCooldownRemaining > 0) return;
		SoundPlayer.Play("Boss - Attack");

		base.Attack(target);
	}
}
