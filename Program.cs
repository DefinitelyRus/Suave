using Raylib_cs;
using Suave.Scripts.Managers;

namespace Suave;

internal class Program {
    static void Main() {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		SoundPlayer.Init();
		GameRenderer.Init();
        GameManager.Init();
        LevelManager.Init();
        
        while (!Raylib.WindowShouldClose()) {
			Raylib.BeginDrawing();

			float delta = Raylib.GetFrameTime();
			InputManager.Update(delta);
			GameRenderer.Update(delta);

			if (StateManager.IsPlaying) {
				LevelManager.Update(delta);
                EntityManager.Update(delta);
				EntityManager.ProcessEntityRegistration(delta);
            }

			else EntityManager.Render(delta);

			Raylib.EndDrawing();
		}

        Raylib.CloseWindow();
	}
}
