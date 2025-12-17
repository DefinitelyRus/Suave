using Raylib_cs;

namespace Suave.Scripts.Managers;

internal class SoundPlayer {

	public static void Init() {
		Raylib.InitAudioDevice();
		Raylib.SetMasterVolume(0.3f);
	}
	public static void Play(Sound sound) {
		Raylib.PlaySound(sound);
	}

	public static void Play(string name) {
		Sound sound = ResourceManager.GetSound(name);
		Play(sound);
	}
}
