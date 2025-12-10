# Suave
A 2D top-down game where you dodge projectiles, gain adrenaline, and defeat your enemy in one fell swoop.

## Game Mechanics
### The player can
- Move in any direction with WASD
- Attack (and also parry) with left click
- Dash in any direction with right click

### The enemies can
- Wander around when the player is far away.
- Approach the player when within seeing distance.
- Throw projectiles at the player when within attacking distance.
- Move away from the player when too close.

### Combat
#### Player health
- The player has `10 HP`.
- Each hit deals `1 DMG` by default.
- Taking DMG gives invincibility for `0.5s * DMG`.
- Parrying heals the player by `1 HP * Projectile.DMG`.
- DMG bonus `-3` when hit.

#### Player attacks
- The player deals `1 DMG` by default.
- Each near-miss dodge increases DMG by `EnemiesInCombat * 3 * Projectile.DMG`.
- The DMG bonus resets when battle ends. (`EnemiesInCombat == 0`)

#### Notes
- Only up to 5 enemies can enter combat with the player at once.


## Enemies
### Grunt
- HP: 10
- DMG: 1
- Move Speed: Normal
- Fire delay: 1.0s
- Projectile Size: Normal
- Projectile Speed: Normal
- Attack Range: Normal

### Better Grunt
- HP: 25
- DMG: 2
- Move Speed: Normal
- Fire delay: 1.0s
- Projectile Size: Normal
- Projectile Speed: Fast
- Attack Range: Far

### Berserker
- HP: 20
- DMG: 1
- Move Speed: Very slow
- Fire delay: 0.4s
- Projectile Size: Small
- Projectile Speed: Normal
- Attack Range: Short

### Sniper
- HP: 10
- DMG: 3
- Move Speed: Very slow
- Fire delay: 2.0s
- Projectile Size: Normal
- Projectile Speed: Fast
- Attack Range: Far

### Boss
- HP: 150
- DMG: Based on fire delay (1 to 5, rounded off)
- Move Speed: Fast
- Fire delay: Random (0.8 to 2.0s)
- Projectile Size: Large
- Projectile Speed: Fast
- Attack Range: Far