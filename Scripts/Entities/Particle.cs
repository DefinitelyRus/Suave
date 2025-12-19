using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Implementations;
using Suave.Scripts.Managers;
using Suave.Scripts.Objects;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Entities;

internal abstract class Particle(string name, string entityId, Vector2 position, Vector2 rotation, Animation animation, float lifespan = 5f) : Entity(name, entityId, position) {

	#region General

	public Vector2 Rotation { get; protected set; } = rotation;

	public float Lifespan { get; protected set; } = lifespan;

	public override void Update(float delta) {
		Animation.Update(delta);
		CurrentTexture = Animation.CurrentTexture;
		Render(delta);
		Lifespan -= delta;

		if (Animation.IsFinished) Despawn();
	}

	public override void Render(float _) {
		SpriteRenderer.Render(CurrentTexture, Position, Rotation, 1f, Color.White);
	}

	#endregion

	#region Visual

	/// <summary>
	/// The current animation being played by the entity.
	/// </summary>
	public Animation Animation { get; protected set; } = animation;

	#endregion

}
