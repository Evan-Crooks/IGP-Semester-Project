using UnityEngine;
using System;

public static class Paths
{
    //class to hold predefined paths for projectiles
    //weapon parts can still use custom paths but these are the most common ones i can think of
    // usage example:
    // p.movementPath = Paths.StraightPath;
    // p.movementPath(p, Time.deltaTime);
    // does not handle projectile experation or range, just the movement path
    public static Vector3 StraightPath(Projectile p, float dt)
    {
        Vector2 move = p.direction * p.speed * dt;
        Vector2 newPos = (Vector2)p.transform.position + move;
        return new Vector3(newPos.x, newPos.y, 0f);
    }
    public static Vector3 GravityPath(Projectile p, float dt)
    {
        Vector2 velocity = p.direction * p.speed;
        float gravityEffect = p.gravity * dt;
        return p.transform.position + new Vector3(velocity.x, velocity.y - gravityEffect, 0) * dt;
    }

    public static Vector3 SineWavePath(Projectile p, float dt)
    {
        float frequency = 10f;
        float amplitude = 0.1f;

        Vector2 perp = new Vector2(-p.direction.y, p.direction.x);
        Vector2 offset = amplitude * Mathf.Cos(p.age * frequency) * perp;

        Vector2 move = dt * p.speed * p.direction;
        Vector2 newPos = (Vector2)p.transform.position + move + offset;

        return new Vector3(newPos.x, newPos.y, 0f);
    }
    public static Vector3 SpiralPath(Projectile p, float dt)
    {
        float spinRate = 10f;
        float spiralGrowth = 0.1f;

        Vector2 perp = new Vector2(-p.direction.y, p.direction.x);
        float amplitude = spiralGrowth * p.age;
        Vector2 offset = perp * Mathf.Sin(p.age * spinRate) * amplitude;

        Vector2 move = p.direction * p.speed * dt;
        Vector2 newPos = (Vector2)p.transform.position + move + offset;

        return new Vector3(newPos.x, newPos.y, 0f);
    }
    public static Vector3 ZigZagPath(Projectile p, float dt)
    {
        float period = 0.2f;
        float zigzagDistance = 1f;

        Vector2 perp = new Vector2(-p.direction.y, p.direction.x);
        float flip = Mathf.Floor(p.age / period) % 2 == 0 ? 1f : -1f;
        Vector2 offset = perp * flip * zigzagDistance * dt;

        Vector2 move = p.direction * p.speed * dt;
        Vector2 newPos = (Vector2)p.transform.position + move + offset;

        return new Vector3(newPos.x, newPos.y, 0f);
    }

    public static Vector3 RandomPath(Projectile p, float dt)
    {
        float randomAngle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 move = randomDirection * p.speed * dt;
        Vector2 newPos = (Vector2)p.transform.position + move;
        return new Vector3(newPos.x, newPos.y, 0f);
    }


}