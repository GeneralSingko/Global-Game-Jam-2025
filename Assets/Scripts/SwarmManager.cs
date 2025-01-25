using UnityEngine;
using System.Collections.Generic;

public class SwarmManager : MonoBehaviour
{
    public List<SwarmEnemy> enemies = new List<SwarmEnemy>();

    void Update()
    {
        foreach (var enemy in enemies)
        {
            Vector2 cohesion = CalculateCohesion(enemy);
            Vector2 alignment = CalculateAlignment(enemy);
            Vector2 separation = CalculateSeparation(enemy);

            enemy.UpdateBehavior(cohesion, alignment, separation);
        }
    }

    Vector2 CalculateCohesion(SwarmEnemy enemy)
    {
        Vector2 center = Vector2.zero;
        int count = 0;

        foreach (var other in enemies)
        {
            if (other == enemy) continue;
            if (Vector2.Distance(other.transform.position, enemy.transform.position) < enemy.neighborRadius)
            {
                center += (Vector2)other.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            center /= count;
            return (center - (Vector2)enemy.transform.position).normalized;
        }

        return Vector2.zero;
    }

    Vector2 CalculateAlignment(SwarmEnemy enemy)
    {
        Vector2 averageVelocity = Vector2.zero;
        int count = 0;

        foreach (var other in enemies)
        {
            if (other == enemy) continue;
            if (Vector2.Distance(other.transform.position, enemy.transform.position) < enemy.neighborRadius)
            {
                averageVelocity += other.GetComponent<Rigidbody2D>().linearVelocity;
                count++;
            }
        }

        if (count > 0)
        {
            averageVelocity /= count;
            return averageVelocity.normalized;
        }

        return Vector2.zero;
    }

    Vector2 CalculateSeparation(SwarmEnemy enemy)
    {
        Vector2 separation = Vector2.zero;

        foreach (var other in enemies)
        {
            if (other == enemy) continue;
            float distance = Vector2.Distance(other.transform.position, enemy.transform.position);
            if (distance < enemy.avoidRadius)
            {
                separation -= ((Vector2)other.transform.position - (Vector2)enemy.transform.position).normalized / distance;
            }
        }

        return separation.normalized;
    }
}
