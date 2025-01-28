using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Scriptable Object")]
    public EnemyStats enemyStatValues;       // Speed of the enemy
    public int currentHealth;

    [Header("Player Distance handler")]
    public float stopDistance = 1f; // Minimum distance to stop following
    public bool isHitting; //sets enemy to hitting mode and stops it from moving
    private Transform player;     // Reference to the player's Transform

    [Header("VFX Settings")]
    [SerializeField] ParticleSystem hitEffect; // Reference to the VFX prefab.
    [SerializeField] Animator animator;
    [SerializeField] RuntimeAnimatorController[] animStates;

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

        animator = GetComponent<Animator>();

        //Setup
        isHitting = false;
        currentHealth = enemyStatValues.maximumHealth;
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) > stopDistance && !isHitting)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyStatValues.moveSpeed * Time.deltaTime);
        } 
        Debug.Log("Enemy current HP: " + currentHealth); //replace later with ui
    }

    void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if(currentHealth <= enemyStatValues.maximumHealth * 0.75)
        {
            animator.runtimeAnimatorController = animStates[1];
        } 
        if (currentHealth <= 1)
        {
            animator.runtimeAnimatorController = animStates[2];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bulletScript;

            bulletScript = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(1);
            bulletScript.ReturnToPool();

            // Play the hit effect.
            PlayHitEffect(collision.GetContact(0).point);
        } else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript;

            playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            playerScript.TakeDamage(enemyStatValues.attackDamage);
        }
    }

    private void PlayHitEffect(Vector2 hitPosition)
    {
        if (hitEffect != null)
        {
            // Instantiate the particle system at the hit position.
            ParticleSystem effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            effect.Play();

            // Destroy the particle system after its duration.
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}
