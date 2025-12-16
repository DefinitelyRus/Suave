using System.Numerics;
using Suave.Scripts.Implementations;

namespace Suave.Scripts.Managers;

internal static class GameManager {

	public static void Init() {
		Player player = new(
			name: "Teto Buttowski",
			entityId: "Player",
			position: Vector2.Zero,
			hitRadius: 16f,
			maxHealth: 40,
			damage: 1,
			attackRange: 72f,
			attackCooldown: 1f,
			moveSpeed: 200f,
			nearMissRange: 50f,
			damageCooldown: 0.5f,
			parryTolerance: 0.8f,
			dashDistance: 160f,
			dashHitCooldown: 0.5f,
			dashMissCooldown: 2.0f,
			dashHitTolerance: 0.9f
		);

		EntityManager.Player = player;
		StateManager.CurrentState = StateManager.States.Menu;
	}
}
