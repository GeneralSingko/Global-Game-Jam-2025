using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object is the player
        if (collision.CompareTag("Player"))
        {
            // Stop the player from exiting the map by reverting their position
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero; // Stop movement
                Debug.Log("Player");
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            // Allow enemies to pass through
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            Debug.Log("enemy");
        }
    }
}
