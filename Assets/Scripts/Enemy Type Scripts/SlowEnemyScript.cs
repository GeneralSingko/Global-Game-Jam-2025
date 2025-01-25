using UnityEngine;

public class SlowEnemyScript : MonoBehaviour
{
    [Header("Hitting Behavior")]
    [SerializeField] float distFromPlayer;
    [SerializeField] GameObject hitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //calculates the distance of this Game Object from the player.
    void CalculateDistanceFromPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        distFromPlayer = Vector2.Distance(transform.position, playerObject.transform.position);
    }
}
