using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private IA_PlayerControls playerControls;

    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    private void Awake()
    {
        playerControls = new IA_PlayerControls();

        playerControls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;
        
        playerControls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
