using UnityEngine;

public class NormalExplosion : MonoBehaviour
{
    public EnemyStats enemyStatValues;
    public int amountHealed;
    public float deathTimer;

    private void Update()
    {
        deathTimer -= Time.deltaTime;
        if(deathTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript;

            playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            playerScript.TakeDamage(enemyStatValues.attackDamage);
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            //Bro my brain is cooked I feel this is so ineffecient but it works
            EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
            if(enemyBehavior.enemyStatValues.maximumHealth > enemyBehavior.currentHealth)
            {
                Debug.Log("Detects that enemy is below max health");
                enemyBehavior.currentHealth += amountHealed;
                if(enemyBehavior.enemyStatValues.maximumHealth < enemyBehavior.currentHealth)
                {
                    enemyBehavior.currentHealth = enemyBehavior.enemyStatValues.maximumHealth; //this aint it chief my brains out of braincells im on my last legs
                }
            }
        }
    }
}
