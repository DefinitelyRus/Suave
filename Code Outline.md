# Core Architecture
## Interfaces
### `IUpdatable`
- `void Update(float dt)`

### `IDrawable`
- `void Draw()`

### `IEntity`
- `Vector2 Position { get; set; }`
- `float Radius { get; }`
- `bool IsAlive { get; }`

### `IEnemy : IEntity`
- `int HP { get; set; }`
- `int Damage { get; }`
- `float MoveSpeed { get; }`
- `void TakeDamage(int amount)`
- `void OnDeath()`

### `IProjectile : IEntity`
- `int Damage { get; }`
- `Vector2 Velocity { get; }`



&nbsp;

# Static Manager Classes
### `static EntityManager`

- Lists:
  - `List<IEnemy> Enemies`
  - `List<IProjectile> Projectiles`
  - `Player Player`
- Methods:
  - `void Add(IEnemy enemy)`
  - `void Add(IProjectile proj)`
  - `void Clear()`
  - `IEnumerable<IEntity> FindEntitiesAt(Vector2 position, float radius)`
  - `void UpdateAll(float dt)`
  - `void DrawAll()`

### `static LevelManager`

- `int CurrentLevel`
- `void StartLevel(int levelIndex)`
- `bool IsLevelComplete()`
- `bool IsLevelFailed()`
- Holds level definitions (data objects, not logic).

### `static GameState`

- `bool IsInMenu`
- `bool IsPlaying`
- `bool IsGameOver`

### `static Input`
- Simple wrapper for Raylib input polling.



&nbsp;

# Player

### `class Player : IEntity, IUpdatable, IDrawable`
- State
  - `Vector2 Position`
  - `float Radius`
  - `int HP`
  - `int MaxHP`
  - `int BaseDamage`
  - `int DamageBonus`
  - `float InvincibilityTime`
  - `float ParryCooldown`
- Actions
  - `void Update(float dt)`
  - `void Draw()`
  - `void Move(float dt)`
  - `void Dash()`
  - `void Attack()` (parry inside this)
  - `void TakeDamage(int amount)`
  - `void AddDamageBonus(int amount)`
  - `void ResetDamageBonus()`



&nbsp;

# Projectiles
### `class EnemyProjectile : IProjectile, IUpdatable, IDrawable`
- `Vector2 Position`
- `float Radius`
- `Vector2 Velocity`
- `int Damage`
- `void Update(float dt)`
- `void Draw()`
- Basic lifetime or fade-out depending on needs.



&nbsp;

# Enemy Base + Variants
### `abstract class EnemyBase : IEnemy, IUpdatable, IDrawable`

- Common State
  - `Vector2 Position`
  - `float Radius`
  - `int HP`
  - `int Damage`
  - `float MoveSpeed`
  - `float FireDelay`
  - `float FireCooldown`
  - `float AttackRange`
- Common Behavior
  - `void Update(float dt)`
  - `void Draw()`
  - `void TakeDamage(int amount)`
  - `void OnDeath()`
  - `abstract void Move(float dt)`
  - `abstract void Attack(float dt)`

### Concrete enemies:
Each overrides stats + movement + attack logic.

- `class Grunt : EnemyBase`
- `class BetterGrunt : EnemyBase`
- `class Berserker : EnemyBase`
- `class Sniper : EnemyBase`
- `class Boss : EnemyBase`



&nbsp;

# Level Data
### `class LevelDefinition`
- `int EnemyCount`
- `Type[] EnemyTypes`
- `float TimeLimit`
- `string MusicPath`

LevelManager loads these, then:

- Spawns enemies
- Resets timers
- Loads music
- Tracks win/lose



&nbsp;

# HUD / UI
### `static class UI`
- `void DrawMainMenu()`
- `void DrawHUD(Player player, int level, float timeLeft)`
- Small and centralised.



&nbsp;

# Utility
### `static class MathUtil`
- `bool CircleOverlap(Vector2 a, float ar, Vector2 b, float br)`
- Maybe lerp helpers.



&nbsp;

# Flow
- `Program` contains game loop.
- State machine lives in `GameState`.
- Level logic in `LevelManager`.
- Entities gathered in `EntityManager`.