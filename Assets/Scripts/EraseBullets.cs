using UnityEngine;

public class EraseBullets : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collided with: {other.name}, Tag: {other.tag}");

        // Check if the colliding object has the "Bullet" tag
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet destroyed");
            Destroy(other.gameObject); // Destroy the bullet
        }
    }
}
