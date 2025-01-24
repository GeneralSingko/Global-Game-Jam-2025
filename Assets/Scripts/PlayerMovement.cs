using System.Collections;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Variables
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

    private CharacterController characterController;
    private Vector3 inputVector;
    private float currentSpeed;

    [Header("Controls")]
    [Header("Movement Inputs")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [Header("Shooting Inputs")]
    [SerializeField] private KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] private KeyCode fireMode1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode fireMode2 = KeyCode.Alpha2;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //grabs character components
        characterController = GetComponent<CharacterController>();

        //sets up variables
        fireState = 1; //sets fire state to single fire
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Functions
        HandleInput();
        Move(inputVector);

        //Shooting Functions
        Shoot();

        //Mouse Direction rotation
        LookAtMouse();

        //if(Input.GetKey(KeyCode.H))
        //{
        //    Debug.Log("Pressed");
        //}
    }

    //gets player input
    private void HandleInput()
    {
        //Movement Inputs
        float xInput = 0;
        float yInput = 0;

        if(Input.GetKey(upKey))
        {
            yInput++;
        }
        if(Input.GetKey(downKey))
        {
            yInput--;
        }
        if(Input.GetKey(rightKey))
        {
            xInput++;
        }
        if(Input.GetKey(leftKey))
        {
            xInput--;
        }
        inputVector = new Vector3(xInput, yInput);

        //Shooting Inputs
        if (Input.GetKeyDown(fireMode1))
        {
            fireState = 1;
        }
        if (Input.GetKeyDown(fireMode2))
        {
            fireState = 2;
        }
    }

    //Moves the player around
    private void Move(Vector3 inputVector)
    {
        if(inputVector == Vector3.zero)
        {
            if(currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0) * Time.deltaTime;
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * acceleration);
        }

        Vector3 movement = inputVector.normalized * currentSpeed * Time.deltaTime;
        characterController.Move(movement);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    //Makes player look at mouse cursor
    void LookAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    //Shoots projectiles based on fire state
    void Shoot()
    {
        //Instantiate(bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
        switch (fireState)
        {
            case 1:
                if(Input.GetKeyDown(primaryFire) && canShoot)
                {
                    StartCoroutine(FireStateOne());
                }
                return;
            case 2:
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

    //Spawns bullets in designated bullet spawn point
    void SpawnBullet()
    {
        Instantiate(bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
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
}
