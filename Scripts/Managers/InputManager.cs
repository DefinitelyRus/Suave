using System.Numerics;
using Raylib_cs;
namespace Suave.Scripts.Managers;

internal class InputManager {
	public const KeyboardKey Start = KeyboardKey.Enter;
	public const KeyboardKey Pause = KeyboardKey.Space;
	public const KeyboardKey Reset = KeyboardKey.F5;

	public const MouseButton Parry = MouseButton.Left;
	public const MouseButton Dash = MouseButton.Right;

	public const KeyboardKey Up = KeyboardKey.W;
	public const KeyboardKey Down = KeyboardKey.S;
	public const KeyboardKey Left = KeyboardKey.A;
	public const KeyboardKey Right = KeyboardKey.D;

	public static void Update(float delta) {
		if (Raylib.IsKeyPressed(Pause)) StateManager.TogglePause();

		if (Raylib.IsKeyPressed(Reset)) ;// RESET GAME

		if (StateManager.IsPaused) return;

		Vector2 mousePosition = Raylib.GetMousePosition();

		if (EntityManager.Player != null) {
			EntityManager.Player.FaceTowards(mousePosition, delta);

			if (Raylib.IsMouseButtonPressed(Parry)) EntityManager.Player!.Parry();

			if (Raylib.IsMouseButtonPressed(Dash)) EntityManager.Player!.Dash();

			if (Raylib.IsKeyDown(Up)) EntityManager.Player!.MoveTowardsDirection(new Vector2(0, -1), delta);

			if (Raylib.IsKeyDown(Down)) EntityManager.Player!.MoveTowardsDirection(new Vector2(0, 1), delta);

			if (Raylib.IsKeyDown(Left)) EntityManager.Player!.MoveTowardsDirection(new Vector2(-1, 0), delta);

			if (Raylib.IsKeyDown(Right)) EntityManager.Player!.MoveTowardsDirection(new Vector2(1, 0), delta);
		}
	}
}
