using Raylib_cs;

namespace Suave.Scripts.Managers;

internal class SoundPlayer {

	public static void Init() {
		Raylib.InitAudioDevice();
		Raylib.SetMasterVolume(0.15f);
	}

	public static void Play(Sound sound, float unloadAfter = 5f) {
		Raylib.PlaySound(sound);

		Task.Run(async () => {
			await Task.Delay((int) (unloadAfter * 1000));
			Raylib.UnloadSound(sound);
		});
	}

	public static void Play(string name, float unloadAfter = 5f) {
		Sound sound = ResourceManager.GetSound(name);
		Play(sound, unloadAfter);
	}
}
