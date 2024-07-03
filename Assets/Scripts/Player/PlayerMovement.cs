using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Move();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Move()
    {
        movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        if (movementDirection.magnitude > 0)
        {
            controller.Move(movementDirection * (MovementSpeed * Time.deltaTime));
        }
    }
}
