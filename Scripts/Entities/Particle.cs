using System.Numerics;
using Suave.Scripts.Objects;

namespace Suave.Scripts.Entities;

internal abstract class Particle(string name, string entityId, Vector2 position, Animation animation, float lifespan = 5f) : Entity(name, entityId, position) {

	#region General

	public float Lifespan { get; protected set; } = lifespan;

	#endregion

	#region Visual

	/// <summary>
	/// The current animation being played by the entity.
	/// </summary>
	public Animation Animation { get; protected set; } = animation;

	#endregion

}
