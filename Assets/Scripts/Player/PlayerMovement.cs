using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private IA_PlayerControls playerControls;
    private CharacterController controller;

    [Header("Player Inputs")]
    private Vector2 moveInput;
    private Vector2 aimInput;
    private Vector3 movementDirection;
    
    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float Gravity = 9.81f;

    [Header("Info")] private float verticalVelocity;

    private void Awake()
    {
        playerControls = new IA_PlayerControls();
        
        controller = GetComponent<CharacterController>();

        playerControls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;
        
        playerControls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        ApplyGravity();
        if (movementDirection.magnitude > 0)
        {
            controller.Move(movementDirection * (MovementSpeed * Time.deltaTime));
        }
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            verticalVelocity -= Gravity * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
        }
    }
}
