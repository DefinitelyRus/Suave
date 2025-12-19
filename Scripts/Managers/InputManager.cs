using System.Numerics;
using Raylib_cs;
namespace Suave.Scripts.Managers;

internal class InputManager {
	public const KeyboardKey Start = KeyboardKey.Enter;
	public const KeyboardKey Pause = KeyboardKey.Space;
	public const KeyboardKey Reset = KeyboardKey.F5;
	public const KeyboardKey Sprint = KeyboardKey.LeftShift;

	public const MouseButton Parry = MouseButton.Left;
	public const MouseButton Dash = MouseButton.Right;

	public const KeyboardKey Up = KeyboardKey.W;
	public const KeyboardKey Down = KeyboardKey.S;
	public const KeyboardKey Left = KeyboardKey.A;
	public const KeyboardKey Right = KeyboardKey.D;

	public static void Update(float delta) {
		switch (StateManager.CurrentState) {

			// ----- In-game -----
			case StateManager.States.Playing:

				if (EntityManager.Player == null) return;

				Vector2 mousePosition = Raylib.GetMousePosition();

				// Sprint
				EntityManager.Player.IsSprinting = Raylib.IsKeyDown(Sprint);

				// Actions
				if (Raylib.IsMouseButtonPressed(Parry)) EntityManager.Player.Parry();
				if (Raylib.IsMouseButtonPressed(Dash)) EntityManager.Player.Dash();

				// Movement
				EntityManager.Player.FaceTowards(mousePosition, delta);
				bool isMoving = false;
				if (Raylib.IsKeyDown(Up)) { EntityManager.Player.MoveTowardsDirection(new Vector2(0, -1), delta); isMoving = true; }
				if (Raylib.IsKeyDown(Down)) { EntityManager.Player.MoveTowardsDirection(new Vector2(0, 1), delta); isMoving = true; }
				if (Raylib.IsKeyDown(Left)) { EntityManager.Player.MoveTowardsDirection(new Vector2(-1, 0), delta); isMoving = true; }
				if (Raylib.IsKeyDown(Right)) { EntityManager.Player.MoveTowardsDirection(new Vector2(1, 0), delta); isMoving = true; }
				if (!isMoving) EntityManager.Player.StopMoving();

				// Options
				if (Raylib.IsKeyPressed(Pause)) StateManager.TogglePause();
				if (Raylib.IsKeyPressed(Reset)) GameManager.Reset();
				break;

			// ----- Main menu -----
			case StateManager.States.Menu:

				// Start the game
				if (Raylib.IsKeyPressed(Start)) {
					LevelManager.StartLevel(0);
				}

				break;

			// ----- Paused -----
			case StateManager.States.Paused:
				if (Raylib.IsKeyPressed(Pause)) StateManager.TogglePause();
				if (Raylib.IsKeyPressed(Reset)) GameManager.Reset();
				break;

			// ----- Won/Lost -----
			case StateManager.States.Win:
			case StateManager.States.Lose:
				if (Raylib.IsKeyPressed(Start)) {
					StateManager.CurrentState = StateManager.States.Menu;
					GameManager.Reset();
				}

				if (Raylib.IsKeyPressed(Reset)) GameManager.Reset();

				break;
		}
	}
}
