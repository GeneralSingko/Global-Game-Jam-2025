using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Controls")]
    [Header("Shooting Inputs")]
    [SerializeField] private KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] private KeyCode fireMode1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode fireMode2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode reload = KeyCode.R;

    [Header("Shooting")]
    [SerializeField] private float singleFireBulletCD;
    [SerializeField] private float rapidFireBulletCD;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;

    private int fireState;
    private bool canShoot;

    [Header("Ammo System")]
    [SerializeField] private int magazineSize = 10;
    [SerializeField] private float reloadTime = 2f;
    private int currentAmmo;
    private bool isReloading;

    Vector2 mousePos;

    void Start()
    {
        fireState = 1;
        canShoot = true;
        currentAmmo = magazineSize;
    }

    void Update()
    {
        HandleInput();
        Shoot();
        ReloadBullets();

        LookAtMouse();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(fireMode1)) fireState = 1;
        if (Input.GetKeyDown(fireMode2)) fireState = 2;
    }

    void Shoot()
    {
        if (isReloading || currentAmmo <= 0) return;

        switch (fireState)
        {
            case 1:
                if (Input.GetKeyDown(primaryFire) && canShoot) StartCoroutine(FireStateOne());
                break;
            case 2:
                if (Input.GetKey(primaryFire) && canShoot) StartCoroutine(FireStateTwo());
                break;
        }
    }

    IEnumerator FireStateOne()
    {
        canShoot = false;
        SpawnBullet();
        yield return new WaitForSeconds(singleFireBulletCD);
        canShoot = true;
    }

    IEnumerator FireStateTwo()
    {
        while (Input.GetKey(primaryFire) && currentAmmo > 0 && !isReloading)
        {
            canShoot = false;
            SpawnBullet();
            yield return new WaitForSeconds(rapidFireBulletCD);
            canShoot = true;
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = BulletPoolManager.Instance.GetBullet();
        if (bullet == null)
        {
            Debug.LogWarning("No bullets available in the pool!");
            return;
        }

        // Set the bullet's position and rotation
        bullet.transform.position = bulletSpawnPos.position;

        // Calculate direction towards the mouse
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Ensure the latest position is used
        Vector2 direction = (mousePos - (Vector2)bulletSpawnPos.position).normalized;

        // Apply velocity to the bullet's Rigidbody2D
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f; // Adjust speed as needed
        }

        currentAmmo--;
        Debug.Log("Ammo left: " + currentAmmo);
    }

    void ReloadBullets()
    {
        if (Input.GetKeyDown(reload) && !isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
        Debug.Log("Reload complete!");
    }

    void LookAtMouse()
    {
        // Update the mouse position without rotating the player.
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
