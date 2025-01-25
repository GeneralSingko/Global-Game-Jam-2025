using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 2f;       // Speed of the enemy
    public float stopDistance = 1f; // Minimum distance to stop following
    private Transform player;     // Reference to the player's Transform

    private void Start()
    {
        // Find the player GameObject by its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject exists
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Make sure it has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
