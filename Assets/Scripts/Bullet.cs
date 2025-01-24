using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Speed")]
    [SerializeField] private float bulletSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, bulletSpeed * Time.deltaTime, 0);
    }
}
