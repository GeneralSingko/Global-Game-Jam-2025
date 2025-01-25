using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemyScript : MonoBehaviour
{
    [Header("Enemy Type")]
    [Tooltip("Type1 = Slow | Type2 = Fast | Type3 = Normal")]
    [SerializeField] int enemyType; //Type1 = Slow, Type2 = Fast, Type3 = Normal

    [Header("General Settings")]
    [SerializeField] float hitTime;
    [SerializeField] float windUp;
    [SerializeField] GameObject hitBox;
    private float distFromPlayer;

    [Header("Fast Behavior")]
    [SerializeField] float fastDashDist;
    [SerializeField] float fastCD;
    [SerializeField] float hitPointMarkerDist;

    private Vector2 hitpoint;
    private bool isDashing;
    private EnemyBehavior enemyBehavior;
    private GameObject playerObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistanceFromPlayer();
        if(distFromPlayer < enemyBehavior.stopDistance && !enemyBehavior.isHitting)
        {
            Debug.Log("you are within range");
            switch(enemyType)
            {
                case 1: //Slow
                    StartCoroutine(SlowHitPlayer());
                    break;
                case 2:
                    StartCoroutine(FastHitPlayer());
                    break;
                case 3:
                    StartCoroutine(NormalHitPlayer());
                    break;
                default:
                    break;
            }
        }
        if (isDashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, hitpoint, fastDashDist);
        }
    }

    //calculates the distance of this Game Object from the player.
    void CalculateDistanceFromPlayer()
    {
        distFromPlayer = Vector2.Distance(transform.position, playerObject.transform.position);
    }

    IEnumerator SlowHitPlayer()
    {
        enemyBehavior.isHitting = true;
        yield return new WaitForSeconds(windUp);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(hitTime);
        hitBox.SetActive(false);
        enemyBehavior.isHitting = false;
    }
    IEnumerator FastHitPlayer()
    { 
        enemyBehavior.isHitting = true;
        hitpoint = playerObject.transform.position;
        yield return new WaitForSeconds(windUp);
        isDashing = true;
        yield return new WaitForSeconds(hitTime);
        enemyBehavior.isHitting = false;
        isDashing = false;
        yield return new WaitForSeconds(fastCD);
    }
    IEnumerator NormalHitPlayer()
    { 
        enemyBehavior.isHitting = true;
        yield return new WaitForSeconds(windUp);
        transform.DetachChildren();
        Destroy(gameObject);
        hitBox.SetActive(true);
        enemyBehavior.isHitting = false;
    }

}
