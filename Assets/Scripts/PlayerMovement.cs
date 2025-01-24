using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;

    [Header("Controls")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;

    private CharacterController characterController;
    private Vector3 inputVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleInput()
    {
        float xInput = 0;
        float yInput = 0;

        if(Input.GetKeyDown(upKey))
        {
            yInput++;
        }
        if(Input.GetKeyDown(downKey))
        {
            yInput--;
        }
        if(Input.GetKeyDown(rightKey))
        {
            xInput++;
        }
        if(Input.GetKeyDown(upKey))
        {
            xInput--;
        }
        inputVector = new Vector3(xInput, yInput);
    }

    private void Move(Vector3 inputVector)
    {

    }
}
