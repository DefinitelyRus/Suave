# Suave
A 2D top-down game where you dodge projectiles, gain adrenaline, and defeat your enemy in one fell swoop.

## Game Mechanics
### The player can
- Move in any direction with WASD
- Parry projectiles with left click
- Dash with right click

### The enemies can
- Wander around when the player is far away. (Optional)
- Approach the player when within seeing distance.
- Throw projectiles at the player when within attacking distance.
- Move away from the player when too close.

### Combat
#### Player health
- The player has 25 HP.
- Taking DMG gives invincibility for `0.5s * Projectile.DMG`.
- Parrying heals the player by `RoundUp(Projectile.DMG / 2)`.
    - Parrying has `0.5s * Projectile.DMG` cooldown.
- DMG bonus `-3` when hit.

#### Player attacks
- The player deals 3 DMG by default.
- Each near-miss dodge increases DMG.
- The DMG bonus resets on dash hit.

## UI
### System Controls
- Press Space to play/pause.
- Press ESC to exit.

## Level
On each one:
- Pre-set array of enemy types to spawn.
- Pre-set range of enemies to spawn.
- 2-minute time limit.
- Unique music.

### Game End Conditions
#### Win
The player wins the level when all enemies have been defeated.

#### Lose
The player loses the level when their HP is 0 or when the timer runs out.
