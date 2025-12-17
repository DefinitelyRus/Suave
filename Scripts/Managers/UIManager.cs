using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Implementations;
using System.Numerics;

namespace Suave.Scripts.Managers;

internal class UIManager {
    private static float damageBouncTimer = 0f;

    public static void DrawHUD()
    {
        Player? player = EntityManager.Player;
        if (player == null) return;
        const int fontSize = 24;
        const int padding = 20;
        Color hudColor = Color.White;

        // Top-left: Health Bar
        int yPosition = padding;

        // Health Bar
        const int barWidth = 200;
        const int barHeight = 25;
        float healthPercentage = (float)player.Health / player.MaxHealth;
        int filledWidth = (int)(barWidth * healthPercentage);
        
        Raylib.DrawRectangle(padding, yPosition, barWidth, barHeight, Color.DarkGray);
        Raylib.DrawRectangle(padding, yPosition, filledWidth, barHeight, Color.Green);
        Raylib.DrawRectangleLines(padding, yPosition, barWidth, barHeight, hudColor);
        Raylib.DrawText($"{player.Health}/{player.MaxHealth}", padding + barWidth + 10, yPosition + 2, fontSize, hudColor);

        // Top-center: Time Remaining
        if (LevelManager.CurrentLevel != null)
        {
            int minutes = (int)(LevelManager.LevelTimer / 60);
            int seconds = (int)(LevelManager.LevelTimer % 60);
            string timeText = $"Time: {minutes:D2}:{seconds:D2}";
            int timeTextWidth = Raylib.MeasureText(timeText, fontSize);
            int centerX = (GameRenderer.SizeX - timeTextWidth) / 2;
            Raylib.DrawText(timeText, centerX, padding, fontSize, hudColor);
        }

        // Top-right: Level and Wave
        if (LevelManager.CurrentLevel != null)
        {
            string levelText = $"Level: {LevelManager.CurrentLevelIndex + 1}";
            int levelTextWidth = Raylib.MeasureText(levelText, fontSize);
            int rightX = GameRenderer.SizeX - levelTextWidth - padding;
            Raylib.DrawText(levelText, rightX, padding, fontSize, hudColor);

            string waveText = $"Wave: {LevelManager.CurrentWave}/{LevelManager.CurrentLevel.Waves}";
            int waveTextWidth = Raylib.MeasureText(waveText, fontSize);
            rightX = GameRenderer.SizeX - waveTextWidth - padding;
            Raylib.DrawText(waveText, rightX, padding + 30, fontSize, hudColor);
        }

        // Bottom-left: Level info (keeping it compact)
        int bottomY = GameRenderer.SizeY - fontSize - padding;
        Raylib.DrawText($"Level {LevelManager.CurrentLevelIndex + 1}", padding, bottomY, fontSize, hudColor);

        // Floating Damage Bonus following player with bounce physics
        DrawFloatingDamageBonus(player);
    }

    public static void UpdateHUD(float delta)
    {
        damageBouncTimer += delta;
    }

    private static void DrawFloatingDamageBonus(Player player)
    {
        if (player.DamageBonus <= 0) return;

        const int fontSize = 20;
        const float circleRadius = 40f;
        const float circleSpeed = 2f;

        // Calculate circular motion using sine and cosine
        float angle = damageBouncTimer * circleSpeed;
        float offsetX = MathF.Cos(angle) * circleRadius;
        float offsetY = MathF.Sin(angle) * circleRadius;

        // Position around the player
        Vector2 displayPos = player.Position + new Vector2(offsetX, offsetY - 30);

        string bonusText = $"+{player.DamageBonus}";
        int textWidth = Raylib.MeasureText(bonusText, fontSize);

        // Draw text centered at position (convert to screen coordinates)
        int screenX = (int)displayPos.X - textWidth / 2;
        int screenY = (int)displayPos.Y;

        // Draw with yellow/gold color and a slight outline for visibility
        Raylib.DrawText(bonusText, screenX - 1, screenY - 1, fontSize, Color.Black);
        Raylib.DrawText(bonusText, screenX + 1, screenY - 1, fontSize, Color.Black);
        Raylib.DrawText(bonusText, screenX - 1, screenY + 1, fontSize, Color.Black);
        Raylib.DrawText(bonusText, screenX + 1, screenY + 1, fontSize, Color.Black);
        Raylib.DrawText(bonusText, screenX, screenY, fontSize, Color.Yellow);
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

	public static void DrawTransition() {
		Raylib.DrawText($"Level {LevelManager.CurrentLevelIndex}", 420, 180, 120, Color.Black);
		Raylib.DrawText("Loading...", 360, 600, 42, Color.Black);
	}
}