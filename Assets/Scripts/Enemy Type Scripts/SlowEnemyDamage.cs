using Unity.VisualScripting;
using UnityEngine;

public class SlowEnemyDamage : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStatValues;
    [SerializeField] float pushForce;
    private GameObject playerGameObject;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript;

            playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            playerScript.TakeDamage(enemyStatValues.attackDamage);
        }
    }
}
