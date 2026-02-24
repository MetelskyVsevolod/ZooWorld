# ZooWorld

A 3D top-down game where animals spawn, move, and interact with each other based on their roles. Built as a Unity test task with a focus on clean, extensible architecture.

---

## Architecture Overview

The project is designed to scale — adding a new animal requires creating one `ScriptableObject` (`AnimalConfig`) asset and selecting an existing movement strategy from the dropdown. If none of the existing strategies fit, a new `MovementStrategyBase` subclass can be created — it will automatically appear in the dropdown on any `AnimalConfig` thanks to a custom property drawer, with no other code changes needed.

### Key Patterns

**Strategy Pattern — Movement**

Each animal's movement behaviour is a `MovementStrategyBase` subclass serialized directly on its `AnimalConfig` asset via `[SerializeReference]`. A custom property drawer renders a type-picker dropdown in the inspector.

To add a new movement type:
1. Create a `[Serializable]` class extending `MovementStrategyBase`
2. Implement `OnInitialize`, `Tick`, `Reset`
3. It automatically appears in the dropdown on any `AnimalConfig`

No other changes needed.

**Data-Driven Design — AnimalConfig**

Every animal type is defined by a `ScriptableObject` asset (`AnimalConfig`) containing its name, role (Prey/Predator), prefab and movement strategy. Adding a new animal means creating a new asset, adding it to `SpawnSettings`, and creating a new prefab if new visuals are needed — no code changes required.

**Observer Pattern — Event Bus**

Systems communicate through a central `GameEventBus` injected via Zenject rather than holding direct references to each other. Events are `struct` values so publishing is allocation-free. They are marked `readonly` to enforce immutability — an event should never be modified after it is created.

| Event | Published by | Consumed by                                                                   |
|---|---|-------------------------------------------------------------------------------|
| `AnimalSpawnedEvent` | `Animal` | `AnimalRegistry`                                                              |
| `AnimalDiedEvent` | `Animal` | `AnimalRegistry`, `AnimalPool`, `TastyLabelHandler`, `AnimalDeathCounterView` |
| `AnimalCollisionEvent` | `Animal` | `CollisionResolver`                                                           |
| `AnimalAteEvent` | `CollisionResolver` | `TastyLabelHandler`                                                           |

**Object Pooling**

Both animals and "Tasty!" label canvases are pooled. `AnimalPool` maintains one `ObjectPool<Animal>` per `AnimalConfig` so species never share a pool. `TastyLabelPool` manages world-space canvas instances that parent to predators on show and unparent on hide.

**Dependency Injection — Zenject**

All services are wired in `GameInstaller`. Plain C# classes use constructor injection. `MonoBehaviour` classes use `[Inject] public void Construct(...)` method injection.

---

## Project Structure

```
Assets/
  Scripts/
    Animals/
      Core/           — Animal, AnimalConfig, AnimalRole, MovementStrategyBase
      Movement/       — JumpMovement, LinearMovement
      Editor/         — PolymorphicPickerDrawer, MovementStrategyPickerDrawer
    EventsHandling/   — GameEventBus, all event structs
    Spawning/         — AnimalSpawner, AnimalFactory, AnimalPool, SpawnSettings
    Systems/          — BoundaryChecker, AnimalRegistry, CollisionResolver
    UI/               — TastyLabelView, TastyLabelPool, TastyLabelHandler, AnimalDeathCounterView
    Extensions/       — CollectionExtensions, ComponentExtensions
    Installers/       — GameInstaller
  Data/
    Config/Animals/   — AnimalConfig assets (Frog, Snake)
    SpawnSettings     — SpawnSettings asset
  Prefabs/
    Animals/          — Animal prefabs
    UI/               — TastyLabelView prefab
```

---

## Animals

| Animal | Role | Movement |
|---|---|---|
| Frog | Prey | Periodic impulse jumps in a random direction |
| Snake | Predator | Constant linear velocity in a fixed direction |

---

## Food Chain

- **Prey vs Prey** — physics only, no game logic
- **Prey vs Predator** — prey dies, predator displays "Tasty!" label
- **Predator vs Predator** — random coin flip, loser dies, winner displays "Tasty!" label

---

## Adding a New Animal

1. Create a new `AnimalConfig` asset: right-click in Project → `Create → ZooWorld → Animal Config`
2. Set name, role and prefab
3. Pick a movement strategy from the dropdown (or create a new one — see below)
4. Add the asset to `SpawnSettings.AnimalConfigs`

**Adding a new movement strategy:**

```csharp
[Serializable]
public class FlyMovement : MovementStrategyBase
{
    public float speed = 4f;
    public float hoverHeight = 2f;

    public override void OnInitialize(Animal animal) { ... }
    public override void Tick(Animal animal) { ... }
    public override void Reset() { ... }
}
```

That's it. No other files need to change.

---

## Dependencies

- **Unity** 6000.3.6f1
- **Zenject** — Dependency injection
- **TextMeshPro** — UI text rendering