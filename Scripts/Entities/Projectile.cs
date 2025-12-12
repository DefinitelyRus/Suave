using System.Numerics;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Entities;

internal abstract class Projectile : PhysicalEntity {

	#region General

	/// <summary>
	/// The character that fired this projectile.
	/// </summary>
	public Character Owner { get; protected set; }

	/// <summary>
	/// How long (in seconds) this projectile lasts before disappearing.
	/// </summary>
	public float Lifespan { get; protected set; }

	private float LifespanRemaining { get; set; }

	/// <summary>
	/// If the projectile has been launched into motion.
	/// </summary>
	public bool IsLaunched = false;

	public Projectile(string name, string entityId, Character owner, float hitRadius = 16f, float speed = 160f, float lifespan = 4f) : base(name, entityId, hitRadius) {
		Name = name;
		EntityId = entityId;
		Owner = owner;
		HitRadius = hitRadius;
		Speed = speed;
		Lifespan = lifespan;
		LifespanRemaining = lifespan;

		Position = owner.Position;
	}

	public override void Update(float delta) {
		if (!IsLaunched) return;

		// Only render if within distance bounds.
		float distanceToOwner = Vector2.Distance(Position, Owner.Position);
		bool tooClose = distanceToOwner < RenderMinDistance;
		bool tooFar = distanceToOwner > RenderMaxDistance;
		if (!tooClose & !tooFar) Render();

		Position += Direction * Speed * delta;

		LifespanRemaining -= delta;
		if (LifespanRemaining <= 0) {
			Despawn();
			return;
		}

		// Check for collision

		//TODO: AVFX here.

		Despawn();
	}

	#endregion

	#region Movement & Collision

	/// <summary>
	/// Number of pixels per second this projectile travels.
	/// </summary>
	public float Speed { get; protected set; }

	/// <summary>
	/// The direction the projectile is traveling in.
	/// </summary>
	public Vector2 Direction { get; protected set; } = Vector2.Zero;

	protected float RenderMinDistance = 32f;

	protected float RenderMaxDistance = 2000f;

	public void Launch(Vector2 direction) {
		Direction = Vector2.Normalize(direction);
		IsLaunched = true;

		//TODO: AVFX here.
	}

	public void Launch(PhysicalEntity target) {
		Vector2 targetPosition = target.Position;
		Vector2 directionToTarget = Vector2.Normalize(targetPosition - Position);
		Launch(directionToTarget);
	}

	#endregion
}
