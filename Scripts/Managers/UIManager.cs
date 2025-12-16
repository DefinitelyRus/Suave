using Raylib_cs;
namespace Suave.Scripts.Managers;

internal class UIManager {
    public static void DrawHUD()
    {
        
    }
    public static void DrawMenu()
    {
        Raylib.DrawText("SUAVE", 420, 180, 120, Color.White);
        Raylib.DrawText("Press ENTER to start", 360, 600, 42, Color.White);  
    }
    public static void DrawPaused()
    {
        Raylib.DrawText("Game Paused", 350, 100, 86, Color.White);
        Raylib.DrawText("Press SPACE to resume", 360, 600, 42, Color.White);
    }

    public static void DrawWin()
    {
        Raylib.DrawText("You Win!", 300, 550, 36, Color.Green);
        Raylib.DrawText("Press F5 to restart", 360, 600, 42, Color.White);
    }

    public static void DrawLose()
    {
        Raylib.DrawText("You Lose!", 0, 550, 36, Color.Red);
        Raylib.DrawText("Press F5 to restart", 360, 600, 42, Color.White);
    }
}