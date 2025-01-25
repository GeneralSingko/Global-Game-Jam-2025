using UnityEngine;

public class SampleEnemy : MonoBehaviour
{
    [Header("Scriptable Object")]
    [SerializeField] EnemyStats enemyStatValues;

    int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemyStatValues.maximumHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("current hp " + currentHealth);
    }

    void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bulletScript;

        if (collision.gameObject.CompareTag("Bullet")) 
        {
            bulletScript = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(5);
            bulletScript.ReturnToPool();
        }

    }
}
