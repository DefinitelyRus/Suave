using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Managers;

internal static class SpriteRenderer {

	public static void Render(Texture2D texture, Vector2 position, Vector2 size, float rotation = 0f, Color? color = null) {
		Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);
		Rectangle destRect = new(position.X, position.Y, size.X, size.Y);
		Vector2 origin = new(texture.Width / 2, texture.Height / 2);
		color ??= Color.White;

		Raylib.DrawTexturePro(texture, sourceRect, destRect, origin, rotation, color.Value);
	}

	public static void Render(string textureKey, Vector2 position, Vector2 size, float rotation = 0f, Color? color = null) {
		bool hasValue = ResourceManager.Textures.TryGetValue(textureKey, out Texture2D value);

		if (hasValue) Render(value, position, size, rotation, color);
		else Log.Err($"CurrentTexture with key '{textureKey}' not found in ResourceManager.Textures.");
	}
}
