using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Entities;
using Suave.Scripts.Implementations;
using Suave.Scripts.Tools;
namespace Suave.Scripts.Managers;

internal static class EntityManager {
	public static List<Entity> Entities = [];

	public static List<Character> Characters = [];

	public static List<Projectile> Projectiles = [];

	public static List<Particle> Particles = [];

	public static Player? Player = null;

	private static readonly Queue<Entity> RegisterQueue = [];

	private static readonly Queue<Entity> UnregisteredEntities = [];

	/// <summary>
	/// Registers an entity and spawns it in the game world.
	/// <br/><br/>
	/// <b>Important:</b> Do not call this method directly. Instead, use <see cref="Entity.Instantiate()"/> to ensure proper handling.
	/// </summary>
	/// <remarks>
	/// Consider this an equivalent to <c>ParentNode.AddChild(node)</c> in Godot.
	/// </remarks>
	/// <param name="entity">The entity to add to the game world.</param>
	public static void RegisterEntity(Entity entity) {
		if (RegisterQueue.Contains(entity)) {
			Log.Warn(() => $"Entity '{entity.EntityId}' is already marked for registration.");
			return;
		}

		RegisterQueue.Enqueue(entity);
	}

	/// <summary>
	/// Unregisters the specified entity from the system, removing it from all relevant entity collections.
	/// <br/><br/>
	/// <b>Important:</b> Do not call this method directly. Instead, use <see cref="Entity.Despawn()"/> to ensure proper handling.
	/// </summary>
	/// <remarks>
	/// This does not handle any cleanup or disposal of the entity itself; it merely removes it from the management system.
	/// If there are any references to this entity elsewhere, those references will remain valid until explicitly cleared.
	/// <br/><br/>
	/// This does not clear <see cref="Entity.InstanceId"/>.
	/// <br/><br/>
	/// Consider this an equivalent to <c>ParentNode.RemoveChild(node)</c> in Godot.
	/// </remarks>
	/// <param name="entity">The entity to unregister. Must implement <see cref="Entity"/> and be previously registered.</param>
	public static void UnregisterEntity(Entity entity) {
		if (UnregisteredEntities.Contains(entity)) {
			Log.Warn(() => $"Entity '{entity.EntityId}' is already marked for unregistration.");
			return;
		}

		UnregisteredEntities.Enqueue(entity);
	}

	public static void ProcessEntityRegistration(float _) {

		#region Registration

		List<Entity> doneRegistration = [];

		while (RegisterQueue.Count > 0) {
			Entity entity = RegisterQueue.Dequeue();

			AssignInstanceId(entity);

			switch (entity) {
				case Character character:
					if (Characters.Contains(character)) {
						Log.Warn(() => $"Character entity '{character.InstanceId}' is already registered.");
						return;
					}

					if (character is Player playerCharacter) {
						Log.Me(() => $"Registered {character.InstanceId} as the Player character.");
						Player = playerCharacter;
					}

					Characters.Add(character);
					break;

				case Projectile projectile:
					if (Projectiles.Contains(projectile)) {
						Log.Warn(() => $"Projectile entity '{projectile.InstanceId}' is already registered.");
						return;
					}

					Projectiles.Add(projectile);
					break;

				case Particle particle:
					if (Particles.Contains(particle)) {
						Log.Warn(() => $"Particle entity '{particle.InstanceId}' is already registered.");
						return;
					}

					Particles.Add(particle);
					break;

				default:
					Log.Err(() => $"Entity '{entity.InstanceId}' of type '{entity.GetType().Name}' is already categorized specifically.");
					break;
			}

			Entities.Add(entity);
		}

		#endregion

		#region Unregistration

		while (UnregisteredEntities.Count > 0) {
			Entity entity = UnregisteredEntities.Dequeue();

			switch (entity) {
				case Character character:
					if (!Characters.Contains(character)) {
						Log.Warn(() => $"Character entity '{character.InstanceId}' is not registered.");
						return;
					}

					if (character is Player playerCharacter) Player = null;
					Characters.Remove(character);
					break;

				case Projectile projectile:
					if (!Projectiles.Contains(projectile)) {
						Log.Warn(() => $"Projectile entity '{projectile.InstanceId}' is not registered.");
						return;
					}

					Projectiles.Remove(projectile);
					break;

				case Particle particle:
					if (!Particles.Contains(particle)) {
						Log.Warn(() => $"Particle entity '{particle.InstanceId}' is not registered.");
						return;
					}

					Particles.Remove(particle);
					break;

				default:
					Log.Err(() => $"Entity '{entity.InstanceId}' of type '{entity.GetType().Name}' is not categorized specifically.");
					break;
			}

			Entities.Remove(entity);
		}

		#endregion

	}

	/// <summary>
	/// Unregisters all entities from the system, clearing all relevant entity collections.
	/// </summary>
	/// <remarks>
	/// This does not handle any cleanup or disposal of the entity itself; it merely removes it from the management system.
	/// If there are any references to this entity elsewhere, those references will remain valid until explicitly cleared.
	/// <br/><br/>
	/// This does not clear <see cref="Entity.InstanceId"/>.
	/// <br/><br/>
	/// Consider this an equivalent to <c>ParentNode.RemoveChild(node)</c> in Godot.
	/// </remarks>
	public static void ClearAllEntities() {
		Entities.Clear();
		Characters.Clear();
		Projectiles.Clear();
		Particles.Clear();
	}

	public static void Update(float deltaTime) {
		foreach (Entity entity in Entities) {
			entity.Update(deltaTime);
		}
	}

	/// <summary>
	/// Checks for a physical entity at the specified position.
	/// </summary>
	/// <remarks>
	/// In cases where there are multiple entities found at the specific position,
	/// the first matching entity from <see cref="Entities"/> will be returned.
	/// </remarks>
	/// <param name="position">The point where to search for an entity.</param>
	/// <returns></returns>
	public static PhysicalEntity? GetEntityAtPosition(Vector2 position) {
		foreach (Entity entity in Entities) {
			if (entity is not PhysicalEntity physicalEntity) continue;
			bool isHit = physicalEntity.CollidesAt(position);

			if (isHit) return physicalEntity;
		}

		return null;
	}

	/// <summary>
	/// Checks for all entities of type T within a specified radius from a given position.
	/// </summary>
	/// <typeparam name="T">The type of <see cref="PhysicalEntity"/> to search for.</typeparam>
	/// <param name="position">The center position of the search radius.</param>
	/// <param name="radius">How far to search.</param>
	/// <returns>The instances of type T found within the given radius of the given position.</returns>
	public static T[] GetAllEntitiesInRadius<T>(Vector2 position, float radius) where T : PhysicalEntity {

		List<T> foundEntities = [];

		// Use the type parameter to determine which list to search on.
		switch (typeof(T)) {
			// Characters
			case Type t when t == typeof(Character):
				foreach (Character character in Characters) {
					float distanceFromCenter = Vector2.Distance(position, character.Position);
					float distanceWithRadius = distanceFromCenter - character.HitRadius;
					if (distanceWithRadius <= radius && character is T casted) foundEntities.Add(casted);
				}

				return [.. foundEntities];

			// Projectiles
			case Type t when t == typeof(Projectile):
				foreach (Projectile projectile in Projectiles) {
					float distanceFromCenter = Vector2.Distance(position, projectile.Position);
					float distanceWithRadius = distanceFromCenter - projectile.HitRadius;
					if (distanceWithRadius <= radius && projectile is T casted) foundEntities.Add(casted);
				}

				return [.. foundEntities];

			// Fall back to searching all entities.
			default:
				foundEntities = [.. Entities
					.OfType<T>()
					.Where(e => Vector2.Distance(position, e.Position) - e.HitRadius <= radius)
					.OrderBy(e => Vector2.Distance(position, e.Position))
				];

				return [.. foundEntities];
		}
	}

	/// <summary>
	/// Checks for all physical entities within a specified radius from a given position.
	/// </summary>
	/// <param name="position">The center position of the search radius.</param>
	/// <param name="radius">How far to search.</param>
	/// <returns>The <see cref="PhysicalEntity"/> instances found within the given radius of the given position.</returns>
	public static PhysicalEntity[] GetAllEntitiesInRadius(Vector2 position, float radius) {
		List<PhysicalEntity> foundEntities = [];
		foreach (Entity entity in Entities) {
			if (entity is not PhysicalEntity physicalEntity) continue;
			float distanceFromCenter = Vector2.Distance(position, physicalEntity.Position);
			float distanceWithRadius = distanceFromCenter - physicalEntity.HitRadius;
			if (distanceWithRadius <= radius) foundEntities.Add(physicalEntity);
		}

		return [.. foundEntities];
	}

	/// <summary>
	/// Generates and assigns a unique instance identifier to the specified entity.
	/// </summary>
	/// <remarks>
	/// Not to be confused with <see cref="Entity.SetInstanceId(string)"/>.
	/// </remarks>
	/// <param name="entity">The entity to assign an Instance ID to.</param>
	private static void AssignInstanceId(Entity entity) {
		GenerateId:
		int randomNumber = Raylib.GetRandomValue(0, 99999);
		string instanceId = $"{entity.EntityId}#{randomNumber:D5}";

		foreach (Entity other in Entities) {
			if (other == entity) continue;

			if (other.InstanceId == entity.InstanceId) {
				Log.Warn(() => $"Instance ID collision detected: '{entity.InstanceId}' == '{other.InstanceId}'. Generating a new one.");
				goto GenerateId;
			}
		}

		entity.SetInstanceId(instanceId);
	}
}
