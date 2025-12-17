using System.Numerics;
using Suave.Scripts.Entities;
using Suave.Scripts.Objects;

namespace Suave.Scripts.Implementations;

internal class ParticleCharacterHurt(Vector2 position, Vector2 rotation, float lifespan = 0.4f) : Particle(
	name: "Character - Hurt Particle",
	entityId: "CharacterHurtParticle",
	position: position,
	rotation: rotation,
	animation: new Animation("Character - Hurt Particle", lifespan, false),
	lifespan: lifespan
	) {
}

internal class ParticleParry(Vector2 position, Vector2 rotation, float lifespan = 0.2f) : Particle(
	name: "Player - Parry Particle",
	entityId: "ParryParticle",
	position: position,
	rotation: rotation,
	animation: new Animation("Player - Parry Particle", lifespan, false),
	lifespan: lifespan
	) {
}

internal class ParticleNearMiss(Vector2 position, Vector2 rotation, float lifespan = 0.3f) : Particle(
	name: "Player - Near Miss Particle",
	entityId: "NearMissParticle",
	position: position,
	rotation: rotation,
	animation: new Animation("Player - Near Miss Particle", lifespan, false),
	lifespan: lifespan
	) {
}

internal class ParticleDash(Vector2 position, Vector2 rotation, float lifespan = 1.2f) : Particle(
	name: "Player - Dash Particle",
	entityId: "DashParticle",
	position: position,
	rotation: rotation,
	animation: new Animation("Player - Dash Particle", 0.5f, false),
	lifespan: lifespan
	) {
}

internal class ParticleTeleport(Vector2 position, Vector2 rotation, float lifespan = 1.0f) : Particle(
	name: "Player - Teleport Particle",
	entityId: "TeleportParticle",
	position: position,
	rotation: rotation,
	animation: new Animation("Player - Teleport Particle", lifespan, false),
	lifespan: lifespan
	) {
}