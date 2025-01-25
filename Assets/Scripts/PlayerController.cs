using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

    private float xInput;
    private float yInput;

    [Header("Movement Particle System")]
    [SerializeField] private ParticleSystem movementParticles;

    Vector2 mousePos;

    void Start()
    {
        playerCurrentHP = playerMaximumHP;
        playerRb = GetComponent<Rigidbody2D>();
        canDash = true;
        isDashing = false;
    }

    void Update()
    {
        HandleInput();
        if (!isDashing) Move();
        HandleMovementParticles();

        if (isDashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastMousePos, dashDistance * Time.deltaTime);
        }
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(dashKey) && canDash) StartCoroutine(Dash());
    }

    private void HandleMovementParticles()
    {
        bool isMoving = Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0;
        if (isMoving && !movementParticles.isPlaying) movementParticles.Play();
        else if (!isMoving && movementParticles.isPlaying) movementParticles.Stop();
    }

    private void Move()
    {
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

    public void TakeDamage(int damageTaken)
    {
        playerCurrentHP -= damageTaken;
        if (playerCurrentHP <= 0) Destroy(gameObject);
    }

    public int GetPlayerHealth() => playerCurrentHP;
}
