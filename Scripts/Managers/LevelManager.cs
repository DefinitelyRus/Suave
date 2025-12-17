using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Implementations;
using Suave.Scripts.Levels;
using Suave.Scripts.Objects;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Managers;

internal static class LevelManager {

	#region General

	public static void Init() {
		Levels = [];
		CurrentLevel = null;
		CurrentLevelIndex = 0;
		CurrentWave = 0;
		DoneSpawningWave = false;
		LevelTimer = 0.0f;
		Levels = [
			// Level 1
			new Level(
				timeLimit: 120f,
				waveCount: 2,
				enemiesToSpawn: [
					new PackedEnemy(typeof(EnemyGrunt), 4)
				],
				"Level 1"
			),

			// Level 2
			new Level(
				timeLimit: 180f,
				waveCount: 2,
				enemiesToSpawn: [
					new PackedEnemy(typeof(EnemyGrunt), 2),
					new PackedEnemy(typeof(EnemyBetterGrunt), 2)
				],
				"Level 2"
			),

			// Level 3
			new Level(
				timeLimit: 180f,
				waveCount: 3,
				enemiesToSpawn: [
					new PackedEnemy(typeof(EnemyBerserker), 2),
					new PackedEnemy(typeof(EnemySniper), 2)
				],
				"Level 3"
			),

			// Level 4
			new Level(
				timeLimit: 180f,
				waveCount: 3,
				enemiesToSpawn: [
					new PackedEnemy(typeof(EnemyGrunt), 2),
					new PackedEnemy(typeof(EnemyBetterGrunt), 3),
					new PackedEnemy(typeof(EnemyBerserker), 2),
					new PackedEnemy(typeof(EnemySniper), 1),
				],
				"Level 4"
			),

			// Level 5
			new Level(
				timeLimit: 180f,
				waveCount: 1,
				enemiesToSpawn: [
					new PackedEnemy(typeof(EnemyBoss), 2),
					new PackedEnemy(typeof(EnemyBetterGrunt), 8)
				],
				"Level 5"
			)
		];
	}

	public static void Update(float delta) {
		UpdateTimer(delta);
		ScanWave(delta);
	}

	#endregion

	#region Level Management

	public static uint CurrentLevelIndex = 0;

	public static Level[] Levels = [];
	public static Level? CurrentLevel = null;

	public static async void StartLevel(int levelIndex = -1) {
		// Use current level index if none specified.
		if (levelIndex < 0) levelIndex = (int) ++CurrentLevelIndex;

		if (levelIndex >= Levels.Length) {
			Log.Me(() => "All levels complete! Restarting from Level 1.");
			StateManager.CurrentState = StateManager.States.Win;
			return;
		}

		CurrentLevelIndex = (uint) levelIndex;
		CurrentLevel = Levels[CurrentLevelIndex];
		LevelTimer = CurrentLevel.TimeLimit;

		// Reset player state.
		Player player = EntityManager.Player!;
		player.ResetContemporaryValues();
		player.Position = PlayerSpawnPosition;

		await StateManager.StartTransition();

		CurrentWave = 0;
		StartWave();
	}

	#endregion

	#region Wave Management

	public static uint CurrentWave { get; private set; } = 0;

	private static bool DoneSpawningWave { get; set; } = false;

	public static async void StartWave() {
		DoneSpawningWave = false;
		CurrentWave++;
		Log.Me(() => $"Wave {CurrentWave} starting!");
		await Task.Run(() => SpawnEnemies(CurrentLevel!));
		DoneSpawningWave = true;
	}

	public static void ScanWave(float _) {
		if (!DoneSpawningWave) return;

		// Check for remaining enemies.
		Enemy[] remainingEnemies = [.. EntityManager
			.Characters
			.Where(c => c is Enemy)
			.Cast<Enemy>()
		];

		// If enemies remain, do nothing.
		if (remainingEnemies.Length != 0) return;

		// If no enemies remain, move to next wave or level.
		if (CurrentWave <= CurrentLevel!.Waves) {
			StartWave();
			return;
		}

		Log.Me(() => "Level complete!");
		StartLevel();
	}

	#endregion

	#region Timer

	public static float LevelTimer = 0.0f;

	public static void UpdateTimer(float delta) {
		LevelTimer -= delta;
		if (LevelTimer < 0) LevelTimer = 0;
	}

	#endregion

	#region Entity Spawning

	private static readonly Vector2 SpawnAreaStart = new(128, 72);
	private static readonly Vector2 SpawnAreaEnd = new(1152, 648);
	private static readonly Vector2 PlayerSpawnPosition = new(GameRenderer.CenterX, GameRenderer.CenterY);

	public static Vector2 GetRandomSpawnPosition() {
		float x = Raylib.GetRandomValue((int) SpawnAreaStart.X, (int) SpawnAreaEnd.X);
		float y = Raylib.GetRandomValue((int) SpawnAreaStart.Y, (int) SpawnAreaEnd.Y);

		return new(x, y);
	}

	public static void Spawn(Type characterType, Vector2? position = null) {
		Character? spawnedCharacter = null;

		while (spawnedCharacter == null) {
			Vector2 spawnPosition = (position == null) ? GetRandomSpawnPosition() : position.Value;

			// Return if trying to spawn non-player before player.
			if (EntityManager.Player == null) {
				Log.Warn(() => "The player character must be registered before other characters.");
				return;
			}

			// Skip if too close to player.
			Vector2 playerPosition = EntityManager.Player!.Position;
			float distanceToPlayer = Vector2.Distance(spawnPosition, playerPosition);
			if (distanceToPlayer < 240) continue;

			// Create character instance.
			spawnedCharacter = CreateCharacterInstance(characterType, spawnPosition);

			if (spawnedCharacter == null) {
				Log.Err(() => $"Failed to create character instance of type '{characterType.Name}'.");
				return;
			}
		}

		//TODO: AVFX here.
	}

	private static void SpawnEnemies(Level level) {
		Thread.Sleep(1000);
		foreach (PackedEnemy packedEnemy in level.Enemies) {
			for (int i = 0; i < packedEnemy.Count; i++) {
				Spawn(packedEnemy.EnemyType);
				Thread.Sleep(200);
			}
		}
	}

	public static Character? CreateCharacterInstance(Type characterType, Vector2 position) {
		Character? character = null;

		switch (characterType) {
			case Type t when t == typeof(EnemyGrunt):
				character = new EnemyGrunt(position);
				break;

			case Type t when t == typeof(EnemyBetterGrunt):
				character = new EnemyBetterGrunt(position);
				break;

			case Type t when t == typeof(EnemyBerserker):
				character = new EnemyBerserker(position);
				break;

			case Type t when t == typeof(EnemySniper):
				character = new EnemySniper(position);
				break;

			case Type t when t == typeof(EnemyBoss):
				character = new EnemyBoss(position);
				break;

			default:
				Log.Err(() => $"Cannot create character instance of unsupported type '{characterType.Name}'.");
				return null;
		}

		return character;
	}

	#endregion

}
