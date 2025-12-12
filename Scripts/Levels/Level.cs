using Suave.Scripts.Objects;
namespace Suave.Scripts.Levels;

internal class Level(float timeLimit, int waveCount, PackedEnemy[] enemiesToSpawn, string musicName = "Level 0") {
	public float TimeLimit { get; protected set; } = timeLimit;
	public int Waves { get; protected set; } = waveCount;
	public PackedEnemy[] Enemies { get; protected set; } = enemiesToSpawn;
	public string MusicName { get; protected set; } = musicName;
}
