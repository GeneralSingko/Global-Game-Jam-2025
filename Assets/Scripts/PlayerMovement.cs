using System.Transactions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Variables
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

    [Header("Controls")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;

    private CharacterController characterController;
    private Vector3 inputVector;
    private float currentSpeed;

    //Turning Variables
    Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();      
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Stuff
        HandleInput();
        Move(inputVector);

        //Mouse Direction rotation
        LookAtMouse();
    }

    //gets player input
    private void HandleInput()
    {
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
}
