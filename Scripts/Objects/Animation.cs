using Raylib_cs;
using Suave.Scripts.Managers;
using Suave.Scripts.Tools;
namespace Suave.Scripts.Objects;

internal class Animation {
	public string Name { get; set; }
	public bool Loop { get; set; }
	public float Duration { get; set; }
	public float Elapsed { get; private set; } = 0f;
	public bool IsFinished => !Loop && Elapsed >= Duration;
	public List<Texture2D> Frames { get; set; } = [];
	public Texture2D CurrentTexture { get; private set; }

	public Animation(string name, float duration, bool loop = false) {
		string[] keys = [.. ResourceManager.Textures.Keys];
		int index = 1;

		foreach (string key in keys) {
			string keyFullName = name + $" {index:D2}";
			if (key.StartsWith(keyFullName)) {
				index++;
				Texture2D texture = ResourceManager.GetTexture(key);
				Frames.Add(texture);
			}
		}

		Name = name;
		Loop = loop;
		Duration = duration;

		Log.Err($"No frames found for animation '{name}'.", Frames.Count == 0);
	}

	/// <summary>
	/// Manually set the frames for this animation (useful for non-standard naming patterns).
	/// </summary>
	public void SetFrames(List<Texture2D> frames) {
		Frames = frames;
		Elapsed = 0f;
	}

	public void Update(float deltaTime) {
		Elapsed += deltaTime;

		if (Elapsed >= Duration) {
			if (Loop) Elapsed = 0f;
			else Elapsed = Duration;
		}

		float frameDuration = Duration / Frames.Count;
		int currentFrameIndex = (int) (Elapsed / frameDuration);
		currentFrameIndex = Math.Clamp(currentFrameIndex, 0, Frames.Count - 1);
		CurrentTexture = Frames[currentFrameIndex];
	}
}