# Suave
> Bring a knife to a gunfight.

A 2D top-down game where you dodge projectiles, gain damage bonus, and defeat your enemies with finesse.

<image src="Screenieboo.jpg" alt="Suave Screenshot" width="600"/>

## Game Mechanics
### The player can
- Move in any direction with **WASD**
- Sprint with **Shift**
- Parry projectiles with **Left Click**
- Dash into enemies with **Right Click**
- Press Space to play/pause
- Press ESC to exit

### The enemies can
- Idle when the player is too far away.
- Approach the player when within seeing distance.
- Shoot at the player when within attacking distance.
- Move away when too close.

### Combat
#### Player health
- The player has 25 HP.
- Parrying heals the player.
- DMG bonus is reduced when hit.

#### Player attacks
- The player deals 3 DMG by default.
- Parrying increases DMG bonus by 3x.
- Each near-miss dodge increases DMG bonus.
- The DMG bonus is reduced on dash hit.

## Levels
- 5 levels
- Each level has a time limit (2-3 minutes).
- Each level has different enemy types.
- Each level has 1-3 waves of enemies.

### Game End Conditions
#### Win
The player wins the level when all enemies have been defeated.

#### Lose
The player loses the level when their HP is 0 or when the timer runs out.
