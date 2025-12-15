using Raylib_cs;
using Suave.Scripts.Managers;

namespace Suave;

internal class Program {
    static void Main() {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        GameRenderer.Init();
        SoundPlayer.Init();
        GameManager.Init();
        LevelManager.Init();
        
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();

            if (!StateManager.IsPaused) {
                float delta = Raylib.GetFrameTime();
                EntityManager.Update(delta);

                EntityManager.ProcessEntityRegistration(delta);
            }

			Raylib.EndDrawing();
		}

        Raylib.CloseWindow();
	}
}
