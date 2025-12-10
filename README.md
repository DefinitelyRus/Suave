# Suave
A 2D top-down game where you dodge projectiles, gain adrenaline, and defeat your enemy in one fell swoop.

## Game Mechanics
### The player can
- Move in any direction with WASD
- Attack (and also parry) with left click
- Dash in any direction with right click

### The enemies can
- Wander around when the player is far away. (Optional)
- Approach the player when within seeing distance.
- Throw projectiles at the player when within attacking distance.
- Move away from the player when too close.

### Combat
#### Player health
- The player has `15 HP`.
- Taking DMG gives invincibility for `0.5s * Projectile.DMG`.
- Parrying heals the player by `RoundUp(Projectile.DMG / 2)`.
    - Parrying has `0.5s * Projectile.DMG` cooldown.
- DMG bonus `-3` when hit.

#### Player attacks
- The player deals `1 DMG` by default.
- Each near-miss dodge increases DMG by `3 * Projectile.DMG`.
- The DMG bonus resets when battle ends. (`EnemiesInCombat == 0`)

## UI
### Main Menu
- Press Space to play.
- Press ESC to exit.

### HUD
- Health bar appears directly above the player.
- DMG Bonus appears at the bottom right of the HUD.
- Current level appears at the bottom left of the HUD.
- Time remaining appears at the top-center of the HUD.


## Level
On each one:
- Pre-set array of enemy types to spawn.
- Pre-set range of enemies to spawn.
- 2-minute time limit.
- Unique music.

### Level 1
Focus on simplicity. Allow the player to learn the controls.
- Enemy count: `3`
- Enemy types: Grunt

### Level 2
Increase difficulty. Allow the player to begin strategizing.
- Enemy count: `6`
- Enemy types: Grunt, Better Grunt

### Level 3
Introduce more enemy types.
- Enemy count: `4`
- Enemy types: Berserker, Sniper

### Level 4
Combine all enemy types.
- Enemy count: `8`
- Enemy types: Grunt, Better Grunt, Berserker, Sniper

### Level 5
Boss fight.
- Enemy count: `2`
- Enemy types: Boss

### Game End Conditions
#### Win
The player wins the level when all enemies have been defeated.

#### Lose
The player loses the level when `HP == 0` or `Timer <= 0`.



## Enemies
### Grunt
- HP: `10`
- DMG: `1`
- Move Speed: Normal
- Fire delay (s): `1.0`
- Projectile Size: Normal
- Projectile Speed: Normal
- Attack Range: Normal

### Better Grunt
- HP: `25`
- DMG: `2`
- Move Speed: Normal
- Fire delay (s): `1.0`
- Projectile Size: Normal
- Projectile Speed: Fast
- Attack Range: Far

### Berserker
- HP: `20`
- DMG: `1`
- Move Speed: Very slow
- Fire delay (s): `0.4`
- Projectile Size: Small
- Projectile Speed: Normal
- Attack Range: Short

### Sniper
- HP: `10`
- DMG: `3`
- Move Speed: Very slow
- Fire delay (s): `2.0`
- Projectile Size: Normal
- Projectile Speed: Fast
- Attack Range: Far

### Boss
- HP: `150`
- DMG: Based on fire delay (`1` to `5`, rounded off)
- Move Speed: Fast
- Fire delay (s): Random (`0.8` to `2.0`)
- Projectile Size: Large
- Projectile Speed: Fast
- Attack Range: Far