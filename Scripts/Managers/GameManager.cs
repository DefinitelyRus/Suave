using System.Numerics;
using Suave.Scripts.Implementations;

namespace Suave.Scripts.Managers;

internal static class GameManager {

	public static void Init() {
		Player player = new(
			name: "Teto Buttowski",
			entityId: "Player",
			position: Vector2.Zero
		);

		EntityManager.Player = player;
		StateManager.CurrentState = StateManager.States.Menu;
	}

	public static void Reset() {
		EntityManager.ClearAllEntities();
		EntityManager.Player = null;
		Init();
		LevelManager.Init();
		LevelManager.StartLevel(0);
	}
}
