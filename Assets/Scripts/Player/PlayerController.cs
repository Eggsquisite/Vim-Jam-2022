using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Variables")]
    private PlayerInputControls inputControls;
    private InputAction movementInput;

    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerAnimations anim;

    [Header("Movement Variables")]
    private bool grounded;
    private Vector2 moveVector;
    private float currentHorizontalSpeed, currentVerticalSpeed;

    [Header("Jump Input")]
    private bool jumpPressed, endJumpEarly;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimations>();
        inputControls = new PlayerInputControls();
    }

    #region Events

    private void OnEnable() {
        movementInput = inputControls.Player.Movement;
        movementInput.Enable();

        inputControls.Player.Jump.performed += OnJump;
        inputControls.Player.Jump.canceled += OnJump;
        inputControls.Player.Jump.Enable();
    }

    private void OnDisable() {
        movementInput.Disable();
        inputControls.Player.Jump.Disable();
    }

    private void OnJump(InputAction.CallbackContext obj) {
        jumpPressed = obj.action.triggered;
        Jump();
    }

    #endregion

    void FixedUpdate() {
        GetInput();
        CalculateMovement();
        Movement();
    }

    #region Get Input

    private void GetInput() {
        moveVector = movementInput.ReadValue<Vector2>().normalized;
    }

    #endregion


    #region Jump
    [Header("Jump")]
    [SerializeField] private float jumpForce = 20f;
    private float apexPoint; // Becomes 1 at apex of jump

    private void Jump() {
        grounded = true;
        if (jumpPressed && grounded) {
            rb.AddForce(new Vector2(0f, jumpForce * rb.gravityScale));
        }
    }

    private void CalculateJumpApex() {
            if (grounded) {
                // Gets stronger the closer to the top of the jump
                apexPoint = Mathf.InverseLerp(jumpForce, 0, Mathf.Abs(rb.velocity.y));
            }
            else {
                apexPoint = 0;
            }
        }

    #endregion

    #region Movement

    [Header("Movement")]
    [SerializeField] private float acceleration = 90;
    [SerializeField] private float moveClamp = 15;
    [SerializeField] private float deAcceleration = 60;
    [SerializeField] private float apexBonus = 2;

    private void CalculateMovement() { 
        if (moveVector.x != 0) { 
            // Set horizontal move speed
            currentHorizontalSpeed += moveVector.x * acceleration * Time.deltaTime;

            // Clamp max movement
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);

            // Apply bonus at the apex of a jump
            var apexMoveBonus = Mathf.Sign(moveVector.x) * apexBonus * apexPoint;
            currentHorizontalSpeed += apexMoveBonus * Time.deltaTime;
            
            Debug.Log("Velocity.y: " + rb.velocity.y + "And apexMoveBonus: " + apexMoveBonus);
        }
        else {
            // No input, slow character down
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, deAcceleration * Time.deltaTime);
        }
    }

    private void Movement() {
        //rb.MovePosition(rb.position + new Vector2(currentHorizontalSpeed * playerSpeed, transform.position.y) * Time.deltaTime);
        rb.velocity = new Vector2(currentHorizontalSpeed, rb.velocity.y);
    }

    #endregion

}
