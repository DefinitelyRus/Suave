using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Managers;

internal static class SpriteRenderer {

	/// <summary>
	/// Renders a texture at the specified position with optional scaling, rotation, and color tinting.
	/// It automatically centers the texture based on its dimensions.
	/// </summary>
	public static void Render(Texture2D texture, Vector2 position, Vector2 rotation, float scale = 1f, Color? color = null) {
		Vector2 size = new(texture.Width * scale, texture.Height * scale);

		// Source rectangle covers the entire texture
		Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);

		// Destination rectangle is centered at the given position
		Rectangle destRect = new(position.X, position.Y, size.X, size.Y);

		// Origin should be the center of the destination rectangle (scaled), not the raw texture size.
		Vector2 origin = new(size.X / 2, size.Y / 2);

		// Convert rotation vector to angle in degrees
		float rotationAngle = Utilities.GetAngle(rotation);

		// Default color to white if none provided
		color ??= Color.White;

		Raylib.DrawTexturePro(texture, sourceRect, destRect, origin, rotationAngle, color.Value);
	}

	public static void Render(string textureKey, Vector2 position, Vector2 rotation, float scale = 1f, Color? color = null) {
		bool hasValue = ResourceManager.Textures.TryGetValue(textureKey, out Texture2D value);

		if (hasValue) Render(value, position, rotation, scale, color);
		else Log.Err($"CurrentTexture with key '{textureKey}' not found in ResourceManager.Textures.");
	}
}
