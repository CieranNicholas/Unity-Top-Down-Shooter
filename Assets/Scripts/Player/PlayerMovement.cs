using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private IA_PlayerControls playerControls;
    private CharacterController controller;
    private Animator animator;

    [Header("Player Inputs")]
    private Vector2 moveInput;
    private Vector2 aimInput;
    private Vector3 movementDirection;
    
    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float SprintSpeed = 10f;
    [SerializeField] private float Gravity = 9.81f;
    
    [Header("Aim Settings")]
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 aimDirection;
    [SerializeField] private Transform aimReticleGO;

    [Header("Info")] 
    private float verticalVelocity;
    private float currentSpeed;
    private bool bIsSprinting;

    private void Awake()
    {
        InputEvents();
        currentSpeed = MovementSpeed;
    }

    // Move into seperate input class
    private void InputEvents()
    {
        playerControls = new IA_PlayerControls();
        
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        playerControls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;
        
        playerControls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;

        playerControls.Character.Sprint.performed += ctx =>
        {
            currentSpeed = SprintSpeed;
            bIsSprinting = true;
        };
        playerControls.Character.Sprint.canceled += ctx =>
        {
            currentSpeed = MovementSpeed;
            bIsSprinting = false;
        };
        
        playerControls.Character.Fire.performed += ctx => FireWeapon();
    }

    private void Update()
    {
        ApplyMovement();
        RotateTowardsMousePosition();
        AnimtorControllers();
    }

    // Temp method until I have a weapon system
    private void FireWeapon()
    {
        animator.SetTrigger("Fire");
    }

    private void AnimtorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);
        
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
        
        var playRunAnimation = bIsSprinting && movementDirection.magnitude > 0;
        
        animator.SetBool("bIsSprinting", playRunAnimation);
    }

    private void RotateTowardsMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimLayerMask))
        {
            aimDirection = hit.point - transform.position;
            aimDirection.y = 0f;
            aimDirection.Normalize();
            
            transform.forward = aimDirection;

            aimReticleGO.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
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
            controller.Move(movementDirection * (currentSpeed * Time.deltaTime));
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
