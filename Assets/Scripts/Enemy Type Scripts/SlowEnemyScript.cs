using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemyScript : MonoBehaviour
{
    [Header("Hitting Behavior")]
    private float distFromPlayer;
    [SerializeField] float hitTime;
    [SerializeField] float windUp;
    [SerializeField] GameObject hitBox;

    private EnemyBehavior enemyBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistanceFromPlayer();
        if(distFromPlayer < enemyBehavior.stopDistance)
        {
            Debug.Log("hitting");
            StartCoroutine(HitPlayer());
        }
    }

    //calculates the distance of this Game Object from the player.
    void CalculateDistanceFromPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        distFromPlayer = Vector2.Distance(transform.position, playerObject.transform.position);
    }

    IEnumerator HitPlayer()
    {
        enemyBehavior.isHitting = true;
        yield return new WaitForSeconds(windUp);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(hitTime);
        hitBox.SetActive(false);
        enemyBehavior.isHitting = false;
    }
}
