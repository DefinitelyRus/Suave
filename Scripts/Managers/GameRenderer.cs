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
		UpdateBackground();
	}

	public static void UpdateBackground() {
		// Load background based on current level (1-indexed)
		uint levelNumber = LevelManager.CurrentLevelIndex + 1;
		string backgroundName = levelNumber > 1 ? $"Background{levelNumber}" : "Background";
		Background = ResourceManager.GetTexture(backgroundName);
	}

	public static void Update(float _) {

		switch (StateManager.CurrentState) {
			case StateManager.States.Playing:
				Raylib.DrawTexture(Background, 0, 0, Color.White);
				UIManager.DrawHUD();
				break;

			case StateManager.States.Paused:
				Raylib.DrawTexture(Background, 0, 0, Color.DarkGray);
				UIManager.DrawPaused();
				break;

			case StateManager.States.Menu:
				Raylib.ClearBackground(Color.Red);
				UIManager.DrawMenu();
				break;

			case StateManager.States.Win:
				Raylib.ClearBackground(Color.SkyBlue);
				UIManager.DrawWin();
				break;

			case StateManager.States.Lose:
				Raylib.ClearBackground(Color.Black);
				UIManager.DrawLose();
				break;

			case StateManager.States.Transition:
				Raylib.ClearBackground(Color.Orange);
				UIManager.DrawTransition();
				break;
		}
	}
}
