using Raylib_cs;

namespace Suave.Scripts.Managers;
internal class GameRenderer {
	public const int SizeX = 1280;
	public const int SizeY = 720;
	public const int FPS = 200;
	public static int CenterX => SizeX / 2;
	public static int CenterY => SizeY / 2;

	public const string Title = "Suave";

	public static Texture2D Background { get; private set; } = new();

	public static void Init() {
		Raylib.InitWindow(SizeX, SizeY, Title);
		Raylib.SetTargetFPS(FPS);
		ResourceManager.PreloadAssets();
		Background = ResourceManager.GetTexture("Background");
	}

	public static void Update(float _) {
		Raylib.ClearBackground(Color.Red);
		Raylib.DrawTexture(Background, 0, 0, Color.White);

		switch (StateManager.CurrentState) {
			case StateManager.States.Playing:
				UIManager.DrawHUD();
				break;

			case StateManager.States.Paused:
				UIManager.DrawPaused();
				break;

			case StateManager.States.Menu:
				UIManager.DrawMenu();
				break;

			case StateManager.States.Win:
				UIManager.DrawWin();
				break;

			case StateManager.States.Lose:
				UIManager.DrawLose();
				break;
		}
	}
}
