using System.Numerics;
using Suave.Scripts.Objects;

namespace Suave.Scripts.Entities;

internal abstract class Particle(string name, string entityId, Vector2 position, Animation animation, float lifespan = 5f) : AnimatedEntity(name, entityId, position, animation) {

	public float Lifespan { get; protected set; } = lifespan;
}
