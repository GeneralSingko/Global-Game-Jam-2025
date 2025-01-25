using System;
using System.Collections;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Movement Variables
    [Header("Player Stats")]
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCD;
    [Header("Player Stats")]
    [SerializeField] int playerMaximumHP;
    private int playerCurrentHP;

    private bool isDashing;
    private bool canDash;
    Vector3 lastMousePos;
    private Rigidbody2D playerRb;

    [Header("Controls")]
    [Header("Movement Inputs")]
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

    private float xInput;
    private float yInput;

    [Header("Shooting Inputs")]
    [SerializeField] private KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] private KeyCode fireMode1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode fireMode2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode reload = KeyCode.R;

    // Movement Particle System
    [Header("Movement Particle System")]
    [SerializeField] private ParticleSystem bubbleTrailVFX;

    //Shooting Variables 
    [Header("Shooting")]
    [SerializeField] private float singleFireBulletCD;
    [SerializeField] private float rapidFireBulletCD;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;

    //Fire State "1" is single fire and fire state "2" is rapid fire
    private int fireState;
    private bool canShoot;

    //Turning Variables
    Vector2 mousePos;
    private Transform pivot;

    [Header("Ammo System")]
    [SerializeField] private int magazineSize = 10;  // Total bullets in one magazine
    [SerializeField] private float reloadTime = 2f;  // Time to reload
    private int currentAmmo;                        // Bullets left in the magazine
    private bool isReloading = false;               // Check if the player is reloading

    private Animator animator; // Reference to Animator

    void Start()
    {
        //sets up variables
        fireState = 1; //sets fire state to single fire
        canShoot = true;
        currentAmmo = magazineSize; // Full magazine
        playerCurrentHP = playerMaximumHP; //Full HP
        playerRb = GetComponent<Rigidbody2D>();
        pivot = transform.Find("Pivot");

        // Initialize Animator
        animator = GetComponent<Animator>();

        //sets up dashing abilities
        canDash = true;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!InGameSceneManager.Instance.gameIsPaused)
        {
            //Movement Functions
            HandleInput();
            if (!isDashing)
            {
                Move();
            }

            HandleMovementParticles();

            //Shooting Functions
            Shoot();

            //Mouse Direction rotation
            LookAtMouse();

            ReloadBullets();
            if (isDashing)
            {
                transform.position = Vector2.MoveTowards(transform.position, lastMousePos, dashDistance * Time.deltaTime);
            }
            Debug.Log("Player Current HP: " + playerCurrentHP);
        }*/

        //Movement Functions
        HandleInput();
        if (!isDashing)
        {
            Move();
        }

        HandleMovementParticles();

        //Shooting Functions
        Shoot();

        //Mouse Direction rotation
        LookAtMouse();

        ReloadBullets();
        if (isDashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastMousePos, dashDistance * Time.deltaTime);
        }
        Debug.Log("Player Current HP: " + playerCurrentHP);
        // Update animation states
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        // Check if the player is moving
        bool isMoving = Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0;

        // Update Animator parameters
        animator.SetBool("IsRunning", isMoving);

        // Reset Hurt trigger if it was set
        animator.ResetTrigger("IsHurt");
    }



    //gets player input
    private void HandleInput()
    {
        //Movement Inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //Shooting Inputs
        if (Input.GetKeyDown(fireMode1))
        {
            fireState = 1;
        }
        if (Input.GetKeyDown(fireMode2))
        {
            fireState = 2;
        }
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void HandleMovementParticles()
    {
        // Check if the player is moving
        bool isMoving = Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0;

        if (isMoving && !bubbleTrailVFX.isPlaying)
        {
            bubbleTrailVFX.Play(); // Start the particle system if the player is moving
        }
        else if (!isMoving && bubbleTrailVFX.isPlaying)
        {
            bubbleTrailVFX.Stop(); // Stop the particle system if the player is idle
        }
    }

    void ReloadBullets()
    {
        if (Input.GetKeyDown(reload) && !isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    //Moves the player around
    private void Move()
    {
        //transform.Translate(new Vector3(xInput * moveSpeed * Time.deltaTime, yInput * moveSpeed * Time.deltaTime, 0), Space.World);
        Vector2 velocity = new Vector2(xInput, yInput);
        playerRb.MovePosition(playerRb.position + velocity * moveSpeed * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        lastMousePos = mousePos;
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    //Makes player look at mouse cursor
    void LookAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        pivot.up = direction;
    }

    //Shoots projectiles based on fire state
    void Shoot()
    {
        // Prevent shooting while reloading
        if (isReloading) return;

        if (currentAmmo > 0)
        {
            //Instantiate(bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
            switch (fireState)
            {
                case 1: //SingleFire
                    if (Input.GetKeyDown(primaryFire) && canShoot)
                    {
                        StartCoroutine(FireStateOne());
                    }
                    return;
                case 2: //RapidFire
                    if (Input.GetKey(primaryFire) && canShoot)
                    {
                        StartCoroutine(FireStateTwo());
                    }
                    return;
                default:
                    Debug.Log("how you done do that?");
                    return;
            }
        }
        else
        {
            Debug.Log("Out of ammo! Reloading...");
            StartCoroutine(Reload());
        }


    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize; // Refill magazine
        isReloading = false;

        Debug.Log("Reload complete!");
    }

    //Spawns bullets in designated bullet spawn point
    void SpawnBullet()
    {
        GameObject bullet = BulletPoolManager.Instance.GetBullet();
        bullet.transform.position = bulletSpawnPos.position;
        bullet.transform.rotation = bulletSpawnPos.rotation;

        currentAmmo--; // Reduce ammo count
        Debug.Log("Ammo left: " + currentAmmo);
    }

    //single fire
    IEnumerator FireStateOne()
    {
        canShoot = false;
        SpawnBullet();
        yield return new WaitForSeconds(singleFireBulletCD);
        canShoot = true;
    }

    //rapid fire
    IEnumerator FireStateTwo()
    {
        canShoot = false;
        SpawnBullet();
        yield return new WaitForSeconds(rapidFireBulletCD);
        canShoot = true;
    }

    /*public void TakeDamage(int damageTaken)
    {
        playerCurrentHP -= damageTaken;
        // Trigger the Hurt animation
        animator.SetTrigger("Hurt");
        if (playerCurrentHP <= 0)
        {
            InGameSceneManager.Instance.GameOverScreen();
            Destroy(gameObject);
        }
    }*/

    public void TakeDamage(int damageTaken)
    {
        playerCurrentHP -= damageTaken;

        if (playerCurrentHP > 0)
        {
            // Trigger the Hurt animation
            animator.SetTrigger("Hurt");
        }
        else
        {
            // Trigger the Dead animation and start destruction process
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Disable input/movement
        this.enabled = false;

        // Trigger the Dead animation
        animator.SetTrigger("Dead");

        // Wait for the animation to finish
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Destroy the GameObject
        Destroy(gameObject);
    }

    public int GetPlayerHealth()
    {
        return playerCurrentHP;
    }
}
