using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public float neighborRadius = 5f; // Radius to detect nearby enemies
    public float avoidRadius = 1f; // Minimum separation distance

    private Vector2 velocity;

    public void UpdateBehavior(Vector2 cohesion, Vector2 alignment, Vector2 separation)
    {
        // Combine behaviors with weights
        Vector2 swarmForce = cohesion * 1.5f + alignment + separation * 2f;
        velocity = Vector2.Lerp(velocity, swarmForce, Time.deltaTime * 2f);

        // Move the enemy
        transform.position += (Vector3)velocity * speed * Time.deltaTime;

        // Rotate towards movement direction
        if (velocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}
