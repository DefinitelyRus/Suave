using System.Numerics;
using Raylib_cs;
using Suave.Scripts.Implementations;
using Suave.Scripts.Managers;

namespace Suave.Scripts.Entities;

internal abstract class Character : PhysicalEntity {

	#region General

	public Character(
		string name,
		string entityId,
		Vector2 position,
		float hitRadius = 16,
		int maxHealth = 10,
		int damage = 1,
		float attackRange = 64,
		float attackCooldown = 1f,
		float moveSpeed = 100f
	) :
	base (
		name,
		entityId,
		position,
		hitRadius
	) {
		Health = maxHealth;
		MaxHealth = maxHealth;
		Damage = damage;
		AttackRange = attackRange;
		AttackCooldown = attackCooldown;
		MoveSpeed = moveSpeed;

		float x = (float) Raylib.GetRandomValue(-100, 100) / 100;
		float y = (float) Raylib.GetRandomValue(-100, 100) / 100;
		FaceDirection = Vector2.Normalize(new Vector2(x, y));
	}

	public override void Update(float delta) {
		if (AttackCooldownRemaining > 0) {
			AttackCooldownRemaining -= delta;
			if (AttackCooldownRemaining < 0) AttackCooldownRemaining = 0;
		}

		Render(delta);
	}

	public override void Render(float _) {
		Vector2 renderPosition = Position + new Vector2(0, GetWobbleOffset());
		SpriteRenderer.Render(EntityId, renderPosition, FaceDirection, 0.15f);
	}

	public virtual void ResetContemporaryValues() {
		Position = Vector2.Zero;
		FaceDirection = new Vector2(0, -1);
		MoveDirection = Vector2.Zero;
		AttackCooldownRemaining = 0f;
		Health = MaxHealth;
		_wobbleTimer = 0f;
	}
	#endregion

	#region Combat

	public int Health { get; protected set; }
	public int MaxHealth { get; protected set; }
	public int Damage { get; protected set; }
	public float AttackRange { get; protected set; }
	public float AttackCooldown { get; protected set; }
	protected float AttackCooldownRemaining { get; set; } = 0f;

	public void TakeDamage(int amount) {
		Health -= amount;

		SoundPlayer.Play(ResourceManager.GetSound("Character - Hurt"));
		_ = new ParticleCharacterHurt(Position, FaceDirection);

		if (Health <= 0) {
			Health = 0;
			Despawn();
		}
	}

	public override void Despawn() {
		base.Despawn();

		SoundPlayer.Play(ResourceManager.GetSound("Character - Death"));
	}

	#endregion

	#region Movement

	public Vector2 FaceDirection { get; protected set; } = new Vector2(0, -1);
	public Vector2 MoveDirection { get; protected set; } = Vector2.Zero;
	public float MoveSpeed { get; protected set; }

	// Wobble animation properties
	private float _wobbleTimer = 0f;
	private const float WobbleSpeed = 8f; // How fast the wobble oscillates
	private const float WobbleAmount = 2f; // How far to wobble (in pixels)

	/// <summary>
	/// Face towards the target position.
	/// </summary>
	/// <param name="targetPosition">The position to face towards.</param>
	/// <param name="_">An unused delta parameter for enforcing use only within <see cref="Update(float)"/>.</param>
	public void FaceTowards(Vector2 targetPosition, float _) => FaceDirection = Vector2.Normalize(targetPosition - Position);

	/// <summary>
	/// Face towards the target position.
	/// </summary>
	/// <param name="target">The position to face towards.</param>
	/// <param name="delta">An unused delta parameter for enforcing use only within <see cref="Update(float)"/>.</param>
	public void FaceTowards(Entity target, float delta) {
		FaceTowards(target.Position, delta);
	}

	public virtual void MoveTowardsDirection(Vector2 direction, float delta) {
		MoveDirection = Vector2.Normalize(direction);
		Position += MoveDirection * MoveSpeed * delta;
		UpdateWobble(delta, true);
	}

	public void StopMoving() {
		MoveDirection = Vector2.Zero;
	}

	/// <summary>
	/// Move towards the target position.
	/// </summary>
	/// <param name="targetPosition">The position to move towards.</param>
	/// <param name="delta">The time the current and the previous frame.</param>
	public virtual void MoveTowardsPosition(Vector2 targetPosition, float delta) {
		MoveDirection = Vector2.Normalize(targetPosition - Position);
		Position += MoveDirection * MoveSpeed * delta;
		UpdateWobble(delta, true);
	}

	/// <summary>
	/// Updates the wobble animation based on movement state.
	/// </summary>
	private void UpdateWobble(float delta, bool isMoving) {
		if (isMoving) {
			_wobbleTimer += delta * WobbleSpeed;
		} else {
			// Reset wobble when not moving
			_wobbleTimer = 0f;
		}
	}

	/// <summary>
	/// Gets the wobble offset to apply to the Y position for animation.
	/// </summary>
	public float GetWobbleOffset() {
		return (float)Math.Sin(_wobbleTimer * Math.PI) * WobbleAmount;
	}

	#endregion
}
