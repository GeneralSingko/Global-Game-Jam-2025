using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    public float minX, maxX, minY, maxY; // Define the boundaries manually in the Inspector

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D component found on the player! Please add one.");
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // Get the player's current position
            Vector2 currentPosition = rb.position;

            // Clamp the position within the defined boundaries
            float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);

            // Apply clamped position only if the player exceeds boundaries
            if (currentPosition.x != clampedX || currentPosition.y != clampedY)
            {
                rb.position = new Vector2(clampedX, clampedY);
            }
        }
    }
}
