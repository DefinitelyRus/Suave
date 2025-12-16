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

            if (!StateManager.IsPaused) {
                float delta = Raylib.GetFrameTime();
				GameRenderer.Update(delta);
				InputManager.Update(delta);
				LevelManager.Update(delta);
                EntityManager.Update(delta);

				EntityManager.ProcessEntityRegistration(delta);
            }

			Raylib.EndDrawing();
		}

        Raylib.CloseWindow();
	}
}
