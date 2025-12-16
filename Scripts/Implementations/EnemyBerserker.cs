using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemyBerserker : Enemy {
	public EnemyBerserker(Vector2 position) : base(
		name: "Berserker",
		entityId: "EnemyBerserker",
		position: position,
		hitRadius: 50f,
		maxHealth: 12,
		damage: 1,
		attackRange: 450f,
		attackCooldown: 0.25f,
		moveSpeed: 80f,
		aggroRange: 500f,
		avoidRange: 150f
		) {
		Name = "Berserker";
		EntityId = "EnemyBerserker";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("EnemyBerserker");
	}

	public override void Attack(Character target) {
		// TODO: AVFX here.
		if (AttackCooldownRemaining > 0) return;
		SoundPlayer.Play("Berserker - Attack");

		base.Attack(target);
	}
}
