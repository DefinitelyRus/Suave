using Raylib_cs;
using Suave.Scripts.Managers;

namespace Suave;

internal class Program {
    static void Main() {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        GameRenderer.Init();
        //GameManager.Init();
        
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();

            float delta = Raylib.GetFrameTime();

			EntityManager.UpdateAllEntities(delta);

			Raylib.EndDrawing();
		}
	}
}
