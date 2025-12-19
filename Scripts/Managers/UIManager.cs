using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Implementations;
using Suave.Scripts.Tools;
using System.Numerics;

namespace Suave.Scripts.Managers;

internal class UIManager {
    private const int Padding = 20;
    private const int FontSize = 24;

    public static void DrawHealthHUD() {
		Player? player = EntityManager.Player;
        if (player == null) return;

		int barWidth = 200;
		int barHeight = 25;
		float healthPercentage = (float) player.Health / player.MaxHealth;
		int filledWidth = (int) (barWidth * healthPercentage);

		Raylib.DrawRectangle(Padding, Padding, barWidth, barHeight, Color.DarkGray);
		Raylib.DrawRectangle(Padding, Padding, filledWidth, barHeight, Color.Green);
		Raylib.DrawRectangleLines(Padding, Padding, barWidth, barHeight, Color.White);
		Raylib.DrawText($"{player.Health}/{player.MaxHealth}", Padding + barWidth + 10, Padding + 2, FontSize, Color.White);
	}

	public static void DrawTimerHUD() {
        if (LevelManager.CurrentLevel == null) return;

		int minutes = (int) (LevelManager.LevelTimer / 60);
		int seconds = (int) (LevelManager.LevelTimer % 60);
		string timeText = $"Time: {minutes:D2}:{seconds:D2}";
		int timeTextWidth = Raylib.MeasureText(timeText, FontSize);
		int centerX = (GameRenderer.SizeX - timeTextWidth) / 2;
		Raylib.DrawText(timeText, centerX, Padding, FontSize, Color.White);
	}

	public static void DrawLevelInfo() {
        if (LevelManager.CurrentLevel == null) return;

		string levelText = $"Level: {LevelManager.CurrentLevelIndex + 1}";
		int levelTextWidth = Raylib.MeasureText(levelText, FontSize);
		int rightX = GameRenderer.SizeX - levelTextWidth - Padding;
		Raylib.DrawText(levelText, rightX, Padding, FontSize, Color.White);

		string waveText = $"Wave: {LevelManager.CurrentWave}/{LevelManager.CurrentLevel.Waves}";
		int waveTextWidth = Raylib.MeasureText(waveText, FontSize);
		rightX = GameRenderer.SizeX - waveTextWidth - Padding;
		Raylib.DrawText(waveText, rightX, Padding + 30, FontSize, Color.White);
	}

	public static void DrawDmgBonusCounter() {
        Player? player = EntityManager.Player;
        if (player == null) return;
		if (player.DamageBonus <= 0) return;

		string bonusText = $"+{player.DamageBonus}";
        int customSize = 20;
		int textWidth = Raylib.MeasureText(bonusText, customSize);

		// Top-right of the player
		float offsetX = 40;
        float offsetY = -30;
		Vector2 displayPos = player.Position + new Vector2(offsetX, offsetY);

		// Convert world position to screen position
		int screenX = (int) displayPos.X - textWidth / 2;
        int screenY = (int) displayPos.Y;

        // Draw outlined text
        Raylib.DrawText(bonusText, screenX - 1, screenY - 1, customSize, Color.Black);
        Raylib.DrawText(bonusText, screenX + 1, screenY - 1, customSize, Color.Black);
        Raylib.DrawText(bonusText, screenX - 1, screenY + 1, customSize, Color.Black);
        Raylib.DrawText(bonusText, screenX + 1, screenY + 1, customSize, Color.Black);
        Raylib.DrawText(bonusText, screenX, screenY, customSize, Color.Yellow);
    }

	public static void DrawParryRadiusPreview() {
		Player? player = EntityManager.Player;
		if (player == null) return;

		if (player.ParryCooldownRemaining > 0) return;

		// Get the angle of the facing direction in radians
		float facingAngle = Utilities.GetAngle(player.FaceDirection) * MathF.PI / 180f;

		// Draw an arc in front of the player (180 degrees, from -90 to +90 relative to facing direction)
		int segments = 32;
		for (int i = 0; i < segments; i++) {
			float angle1 = facingAngle - MathF.PI / 2 + (MathF.PI / segments) * i;
			float angle2 = facingAngle - MathF.PI / 2 + (MathF.PI / segments) * (i + 1);

			Vector2 p1 = player.Position + new Vector2(MathF.Cos(angle1), MathF.Sin(angle1)) * player.ParryRange;
			Vector2 p2 = player.Position + new Vector2(MathF.Cos(angle2), MathF.Sin(angle2)) * player.ParryRange;

			Raylib.DrawLineV(p1, p2, new(1f, 1f, 1f, 0.3f));
		}
	}

    public static void DrawMenu()
    {
        Raylib.DrawText("SUAVE", 420, 180, 120, Color.White);
        Raylib.DrawText("Press SPACE to start", 360, 600, 42, Color.White);  
    }

    public static void DrawPaused()
    {
        Raylib.DrawText("Game Paused", 350, 100, 86, Color.White);
        Raylib.DrawText("Press SPACE to resume", 360, 600, 42, Color.White);
    }

    public static void DrawWin()
    {
        Raylib.DrawText("You Win!", 360, 550, 36, Color.Black);
        Raylib.DrawText("Press F5 to restart", 360, 600, 42, Color.Black);
    }

    public static void DrawLose()
    {
        Raylib.DrawText("You Lose!", 360, 550, 36, Color.Red);
        Raylib.DrawText("Press F5 to restart", 360, 600, 42, Color.White);
	}

	public static void DrawTransition() {
		Raylib.DrawText($"Level {LevelManager.CurrentLevelIndex + 1}", 420, 180, 120, Color.Black);
		Raylib.DrawText("Loading...", 360, 600, 42, Color.Black);
	}
}