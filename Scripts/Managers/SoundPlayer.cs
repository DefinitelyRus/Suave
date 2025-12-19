using Raylib_cs;

namespace Suave.Scripts.Managers;

internal class SoundPlayer {

	private static Music? _currentMusic = null;
	private static bool _isMusicPlaying = false;

	public static void Init() {
		Raylib.InitAudioDevice();
		Raylib.SetMasterVolume(0.15f);
	}	

	public static void Play(Sound sound) {
		Raylib.PlaySound(sound);
	}

	public static void Play(string name) {
		Sound sound = ResourceManager.GetSound(name);
		Play(sound);
	}

	/// <summary>
	/// Play looping background music.
	/// </summary>
	public static void PlayMusic(string musicName) {
		// Stop any existing music
		StopMusic();

		// Load and play the music
		string musicPath = System.IO.Path.Combine(ResourceManager.RootPath, "Audio", musicName);
		if (System.IO.File.Exists(musicPath)) {
			_currentMusic = Raylib.LoadMusicStream(musicPath);
			Raylib.PlayMusicStream(_currentMusic.Value);
			_isMusicPlaying = true;
		}
	}

	/// <summary>
	/// Stop the currently playing background music.
	/// </summary>
	public static void StopMusic() {
		if (_isMusicPlaying && _currentMusic != null) {
			Raylib.StopMusicStream(_currentMusic.Value);
			Raylib.UnloadMusicStream(_currentMusic.Value);
			_isMusicPlaying = false;
		}
		_currentMusic = null;
	}

	/// <summary>
	/// Update music stream (must be called every frame).
	/// </summary>
	public static void UpdateMusic() {
		if (_isMusicPlaying && _currentMusic != null) {
			Raylib.UpdateMusicStream(_currentMusic.Value);
		}
	}
}
