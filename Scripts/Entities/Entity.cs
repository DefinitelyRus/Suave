using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Managers;
using Suave.Scripts.Tools;

namespace Suave.Scripts.Entities;

internal abstract class Entity(string name, string entityId) {

	#region Entity Info

	/// <summary>
	/// The visual name of the entity.
	/// </summary>
	public string Name { get; set; } = name;

	/// <summary>
	/// The unique name for the entity type.
	/// </summary>
	public string EntityId { get; protected set; } = entityId;

	/// <summary>
	/// The current position of the entity in the game world.
	/// </summary>
	public Vector2 Position { get; set; } = Vector2.Zero;

	/// <summary>
	/// Updates the entity's state every frame.
	/// </summary>
	/// <param name="delta"></param>
	public abstract void Update(float delta);

	#endregion

	#region Instance Handling

	/// <summary>
	/// Whether this entity has been added to the game world.
	/// </summary>
	public bool IsInstantiated { get; set; } = false;

	/// <summary>
	/// The unique instance identifier for this specific entity.
	/// </summary>
	/// <remarks>
	/// This is assigned upon registration with the <see cref="EntityManager"/> during instantiation.
	/// </remarks>
	public string InstanceId { get; set; } = string.Empty;

	/// <summary>
	/// Creates a new instance of the entity in the game world.
	/// </summary>
	/// <remarks>
	/// <see cref="Entity"/> objects are considered templates by default that must be instantiated.
	/// Upon instantiation, the entity is registered with the <see cref="EntityManager"/> and assigned a unique <see cref="InstanceId"/>.
	/// </remarks>
	public virtual void Instantiate(Vector2 position) {
		EntityManager.RegisterEntity(this);
		IsInstantiated = true;
		Position = position;
	}

	/// <summary>
	/// Unregisters the entity from the <see cref="EntityManager"/>,
	/// effectively removing it from the game world.
	/// </summary>
	public void Despawn() {
		EntityManager.UnregisterEntity(this);
		IsInstantiated = false;
	}

	/// <summary>
	/// Sets the unique instance identifier for this entity.
	/// </summary>
	/// <remarks>
	/// Not to be confused with <see cref="EntityManager.AssignInstanceId(Entity)"/>.
	/// </remarks>
	/// <param name="instanceId">The Instance ID to assign to this entity.</param>
	public void SetInstanceId(string instanceId) {
		bool hasInstanceId = !string.IsNullOrEmpty(InstanceId);
		if (hasInstanceId) {
			Log.Warn(() => $"Entity '{EntityId}' already has an InstanceId '{InstanceId}'.");
			return;
		}

		InstanceId = instanceId;
	}

	#endregion

	#region Visual Handling

	/// <summary>
	/// The current texture of the entity, usually the current frame from this instance's <see cref="CurrentAnimation"/>.
	/// </summary>
	public Texture2D CurrentTexture { get; set; }

	/// <summary>
	/// Sets the position of the entity where the given position is the center of the entity.
	/// </summary>
	/// <param name="centeredPosition">The center point of the entity.</param>
	public void SetCenteredPosition(Vector2 centeredPosition) {
		Vector2 textureSize = new(CurrentTexture.Width, CurrentTexture.Height);
		Position = centeredPosition - (textureSize / 2);
	}

	#endregion
}
