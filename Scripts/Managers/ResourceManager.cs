using Raylib_cs;

namespace Suave.Scripts.Managers;

internal class ResourceManager {

	#region General

	public const string RootPath = "Assets";

	public static void PreloadAssets() {
		string[] texturePaths = Directory.GetFiles(TexturePath);
		foreach (string path in texturePaths) {
			string fileName = Path.GetFileNameWithoutExtension(path);
			Texture2D texture = Raylib.LoadTexture(path);

			Textures.Add(fileName, texture);
		}

		string[] soundPaths = Directory.GetFiles(SoundPath);
		foreach (string path in soundPaths) {
			string fileName = Path.GetFileNameWithoutExtension(path);
			SoundPaths.Add(fileName, path);
		}
	}

	#endregion

	#region Textures

	public const string TexturePath = RootPath + "/Textures";

	public static Dictionary<string, Texture2D> Textures = [];

	public static Texture2D GetTexture(string name) {
		if (Textures.TryGetValue(name, out Texture2D texture)) return texture;
		return default;
	}

	#endregion

	#region Sounds

	public const string SoundPath = RootPath + "/Audio";

	public static Dictionary<string, string> SoundPaths = [];

	public static Sound GetSound(string name) {
		return Raylib.LoadSound(SoundPaths[name]);
	}

	#endregion

}
