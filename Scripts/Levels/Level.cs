using Suave.Scripts.Objects;
namespace Suave.Scripts.Levels;

internal class Level {

	#region Enemies

	public float TimeLimit { get; protected set; } = 120.0f;
	public int Waves { get; protected set; } = 3;
	public PackedEnemy[] Enemies { get; protected set; } = [];

	#endregion

	public const string MusicName = "Level 0";
}
