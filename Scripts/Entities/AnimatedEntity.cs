using System.Numerics;
using Suave.Scripts.Objects;
namespace Suave.Scripts.Entities;

internal abstract class AnimatedEntity(string name, string entityId, Vector2 position, Animation animation) : Entity(name, entityId, position) {

	/// <summary>
	/// The current animation being played by the entity.
	/// </summary>
	public Animation CurrentAnimation { get; protected set; } = animation;
}
