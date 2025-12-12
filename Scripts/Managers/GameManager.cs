using System.Numerics;
using Suave.Scripts.Implementations;

namespace Suave.Scripts.Managers;

internal static class GameManager {

	public static void Init() {
		Player player = new(
			name: "Teto Buttowski",
			entityId: "Player",
			Vector2.Zero,
			maxHealth: 15,
			attackRange: 32f,
			attackCooldown: 1f,
			moveSpeed: 150f,
			nearMissRange: 64f,
			damageCooldown: 0.5f,
			parryTolerance: 0.8f,
			dashDistance: 160f,
			dashHitCooldown: 0.5f,
			dashMissCooldown: 2.0f,
			dashHitTolerance: 0.9f
		);

		EntityManager.Player = player;
	}
}
