using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class EnemySniper : Enemy {
	public EnemySniper(Vector2 position) : base(
		name: "Sniper",
		entityId: "EnemySniper",
		position: position,
		hitRadius: 50f,
		maxHealth: 10,
		damage: 5,
		attackRange: 800f,
		attackCooldown: 3f,
		moveSpeed: 48f,
		aggroRange: 1200f,
		avoidRange: 120f
		) {
		Name = "Sniper";
		EntityId = "EnemySniper";
		Position = position;
		CurrentTexture = ResourceManager.GetTexture("EnemySniper");
	}

	public override void Attack(Character target) {
		// TODO: AVFX here.
		if (AttackCooldownRemaining > 0) return;
		SoundPlayer.Play("Sniper - Attack");

		base.Attack(target);
	}
}
