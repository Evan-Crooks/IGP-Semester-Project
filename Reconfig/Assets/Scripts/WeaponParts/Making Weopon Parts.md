# Weapon Parts: Stats and Events (Simple)

This aims to be concise: how stats are generated and when part event hooks run.

## Stats
- Each part has three `WeaponStats`: `minCoeff`, `maxCoeff`, and `properties`.
- On `Awake()`, if `properties` is still default, `randomizeStats()` fills it based on rarity.
- Rarity: `Common(1) .. Legendary(5)`. If not set, it’s picked at runtime.
- Randomization (contiguous tiers): higher rarity never rolls worse than lower rarity.
  - `width = maxCoeff - minCoeff`
  - `min = minCoeff + width * (rarity - 1)`
  - `max = min + width`
  - For each numeric field: `Random.Range(min.field, max.field)`
- Non‑numeric fields:
  - Enums (`mode`, `reloadType`, `ammoType`) come from `minCoeff`.
  - `projSprite`/`movementPath` use `minCoeff` if set, otherwise fall back to `maxCoeff`.

Keep it simple: only set the fields your part should influence; leave others at 0.

## Event Hooks (override in your part)
- `onHitEntity(Projectile p)`: Runs when a projectile hits an `Enemy`.
- `onHitEnvironment(Projectile p)`: Runs when a projectile hits `Terrain`.
- `onExpireEffect(Projectile p)`: Runs when a projectile expires (before pooling reset).
- `onTravelEffect(Projectile p)`: Runs whenever `Projectile.onTravel()` is called (use for per‑frame travel effects like trails/spin).
- `onFireEffect(Projectile p)`: Use for immediate effects on firing (muzzle flash, sound). Call from your firing logic if needed.
- `moveProjectile(Projectile p)`: Optional direct movement hook; current movement usually comes from the `movementPath` delegate.

That’s it—define min/max, let rarity roll stats once, and override the events you need.

## how stats come together
 - the base part provdides the base stats of the weopon
 - then each modify the base stats in in the following order currently all parts are multipliers and multiplicative, so parts lower on the list are more impactful. `weaponStats += weaponStats * part.properties;`.
   - grip
   - stock
   - magazine
   - barrel

NOTICE because of how this works Base parts must have non zero values for all numeric properties.