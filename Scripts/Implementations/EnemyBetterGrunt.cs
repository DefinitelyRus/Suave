using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;
namespace Suave.Scripts.Implementations;

internal class EnemyBetterGrunt : Enemy {
	public EnemyBetterGrunt(Vector2 position) : base(
		name: "Senior Grunt",
		entityId: "EnemyGrunt",
		position: position,
		hitRadius: 50f,
		maxHealth: 15,
		damage: 1,
		attackRange: 360f,
		attackCooldown: 0.8f,
		moveSpeed: 96f,
		aggroRange: 400f,
		avoidRange: 50f
		) {
		Name = "Senior Grunt";
		EntityId = "EnemyBetterGrunt";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("Enemy Better Grunt");
	}

	public override void Attack(Character target) {
		// TODO: AVFX here.
		if (AttackCooldownRemaining > 0) return;
		SoundPlayer.Play("Better Grunt - Attack");

		base.Attack(target);
	}
}
