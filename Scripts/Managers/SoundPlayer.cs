using Raylib_cs;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Managers;

internal class SoundPlayer {

	public static void Init() {
		Raylib.InitAudioDevice();
	}
	public static void Play(Sound sound) {
		Raylib.PlaySound(sound);
	}

	public static void Play(string name) {
		bool hasValue = ResourceManager.Sounds.TryGetValue(name, out Sound sound);
		if (hasValue) Play(sound);
		else Log.Err($"Sound with key '{name}' not found in ResourceManager.Sounds.");
	}
}
