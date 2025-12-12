using System.Numerics;
using Suave.Scripts.Objects;
namespace Suave.Scripts.Entities;

internal abstract class AnimatedEntity(string name, string entityId, Animation animation, Vector2 position) : Entity(name, entityId, position) {

	/// <summary>
	/// The current animation being played by the entity.
	/// </summary>
	public Animation CurrentAnimation { get; protected set; } = animation;
}
