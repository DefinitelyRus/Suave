using Raylib_cs;

namespace Suave.Scripts.Managers;
internal class GameRenderer {
	public const int SizeX = 1280;
	public const int SizeY = 720;
	public static int CenterX => SizeX / 2;
	public static int CenterY => SizeY / 2;

	public const string Title = "Suave";

	public static Texture2D Background { get; private set; } = new();

	public static void Init() {
		Raylib.InitWindow(SizeX, SizeY, Title);
		ResourceManager.PreloadAssets();
		Background = ResourceManager.GetTexture("Background");
	}

	public static void Update() {
		Raylib.DrawTexture(Background, 0, 0, Color.White);

		//switch (StateManager.State) {
		//	case StateManager.States.Playing:
		//		UIMan.DrawHUD();
		//		break;

		//	case StateManager.States.Paused:
		//		UIManager.DrawPaused();
		//		break;

		//	case StateMan.States.Menu:
		//		UIManager.DrawMenu();
		//		break;

		//	case StateMan.States.Win:
		//		UIManager.DrawWin();
		//		break;

		//	case StateMan.States.Lose:
		//		UIManager.DrawLose();
		//		break;
		//}
	}
}
