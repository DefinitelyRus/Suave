using Suave.Scripts.Objects;
namespace Suave.Scripts.Entities;

internal abstract class AnimatedEntity(string name, string entityId, Animation animation) : Entity(name, entityId) {

	/// <summary>
	/// The current animation being played by the entity.
	/// </summary>
	public Animation CurrentAnimation { get; protected set; } = animation;
}
