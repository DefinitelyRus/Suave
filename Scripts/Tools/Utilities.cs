using System.Numerics;

namespace Suave.Scripts.Tools;

internal static class Utilities {

	public static float GetAngle(Vector2 direction) {
		float angle = MathF.Atan2(direction.Y, direction.X);
		return angle;
	}

	public static Vector2 GetDirection(float angle) {
		float x = MathF.Cos(angle);
		float y = MathF.Sin(angle);
		return new Vector2(x, y);
	}
}
