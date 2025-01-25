/*using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Speed")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifespan = 5f;

    private void OnEnable()
    {
        // Automatically return to the pool after a set lifespan
        Invoke(nameof(ReturnToPool), lifespan);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    /*private void OnTriggerEnter(Collider other) // Use OnTriggerEnter2D for 2D
    {
        // Check for collision and return to pool
        if (other.CompareTag("EraseBullets"))
        {
            ReturnToPool();
        }
    }*/

    public void ReturnToPool()
    {
        CancelInvoke(); // Stop any pending ReturnToPool calls
        BulletPoolManager.Instance.ReturnBullet(gameObject);
    }

    
}*/

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Speed")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifespan = 5f;

    private void OnEnable()
    {
        // Automatically return to the pool after a set lifespan
        Invoke(nameof(ReturnToPool), lifespan);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) // Use OnTriggerEnter2D for 2D
    {
        // Check for collision and return to pool
        if (other.CompareTag("EraseBullets"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        CancelInvoke(); // Stop any pending ReturnToPool calls
        BulletPoolManager.Instance.ReturnBullet(gameObject);
    }
}

