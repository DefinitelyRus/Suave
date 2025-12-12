using System.Numerics;

namespace Suave.Scripts.Entities;

internal abstract class PhysicalEntity(string name, string entityId, Vector2 position, float hitRadius = 16f) : Entity(name, entityId, position) {
	public float HitRadius { get; protected set; } = hitRadius;

	public bool CollidesAt(Vector2 point) {
		float distance = Vector2.Distance(Position, point);
		return distance <= HitRadius;
	}

	public bool CollidesWith(PhysicalEntity other) {
		float combinedRadius = HitRadius + other.HitRadius;
		float distance = Vector2.Distance(Position, other.Position);
		return distance <= combinedRadius;
	}
}
