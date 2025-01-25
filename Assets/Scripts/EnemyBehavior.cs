using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Scriptable Object")]
    [SerializeField] EnemyStats enemyStatValues;       // Speed of the enemy
    int currentHealth;

    [Header("Player Distance handler")]
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

        currentHealth = enemyStatValues.maximumHealth;
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyStatValues.moveSpeed * Time.deltaTime);
        }
        Debug.Log("current hp " + currentHealth); //replace later with ui
    }

    void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("is Colliding");
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bulletScript;

            bulletScript = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(5);
            bulletScript.ReturnToPool();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bulletScript;

            bulletScript = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(5);
            bulletScript.ReturnToPool();
        } else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript;

            playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            playerScript.TakeDamage(enemyStatValues.attackDamage);
        }
    }
}
