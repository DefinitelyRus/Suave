using Suave.Scripts.Entities;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Implementations;

internal class Bullet : Projectile {
	public Bullet(Character owner) : base("Bullet", "Bullet", owner) {
		CurrentTexture = ResourceManager.GetTexture("Bullet Regular 01");
	}
}
