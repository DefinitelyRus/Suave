namespace Suave.Scripts.Objects;

internal class PackedEnemy(Type enemyType, int count) {
	public Type EnemyType { get; private set; } = enemyType;
	public int Count { get; private set; } = count;
}
