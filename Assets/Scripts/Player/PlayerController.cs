using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private enum JumpState { 
        Level,
        Jumping,
        Falling,
        Landing
    }

    public TMPro.TextMeshProUGUI _title;

    [Header("Input Variables")]
    private PlayerInputControls inputControls;
    private InputAction movementInput;

    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerAnimations anim;

    [Header("Movement Variables")]
    private bool grounded;
    private Vector2 moveVector;
    private float currentHorizontalSpeed;

    [Header("Jump Input")]
    private float baseGravityScale;
    private bool jumpPressed, earlyJumpPressed, endJumpEarly;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimations>();
        inputControls = new PlayerInputControls();

        baseGravityScale = rb.gravityScale;
        jumpState = JumpState.Level;
    }

    #region Events

    private void OnEnable() {
        movementInput = inputControls.Player.Movement;
        movementInput.Enable();

        inputControls.Player.Jump.performed += OnJump;
        inputControls.Player.Jump.canceled += OnJump;
        inputControls.Player.Jump.canceled += EndJumpEarly;
        inputControls.Player.Jump.Enable();
    }

    private void OnDisable() {
        movementInput.Disable();
        inputControls.Player.Jump.Disable();
    }

    private void OnJump(InputAction.CallbackContext obj) {
        jumpPressed = obj.action.triggered;
        if (jumpPressed) jumpButtonPressedTime = Time.time;
        if (jumpPressed && jumpState == JumpState.Jumping || jumpState == JumpState.Falling) earlyJumpPressed = true;
        //Jump();
    }

    private void EndJumpEarly(InputAction.CallbackContext obj) {
        EndJumpEarly();
    }

    #endregion

    void Update() {
        GetInput();
        CalculateMovement();
        CheckGrounded();
        ResetGravity();
        Animate();
        Jump();

        _title.text = (Time.time - lastGroundedTime).ToString();
    }

    void FixedUpdate() {
        Movement(); 
    }

    #region Get Input

    private void GetInput() {
        moveVector = movementInput.ReadValue<Vector2>().normalized;
    }

    #endregion

    #region Grounded

    [Header("Grounded")]
    [SerializeField] private float _distance = 1.1f;
    [SerializeField] private Transform _downBack;
    [SerializeField] private Transform _downFront;
    [SerializeField] private LayerMask _groundMask;
    private RaycastHit2D downBack, downFront;
    private float? lastGroundedTime;

    private void CheckGrounded() {
        downBack = Physics2D.Raycast(_downBack.position, Vector2.down, _distance, _groundMask);
        downFront = Physics2D.Raycast(_downFront.position, Vector2.down, _distance, _groundMask);

        Debug.DrawRay(_downBack.position, Vector2.down * _distance, Color.green);
        Debug.DrawRay(_downFront.position, Vector2.down * _distance, Color.red);

        if (downBack.collider != null || downFront.collider != null && !grounded) { 
            grounded = true;
            //lastGroundedTime = null;
            lastGroundedTime = Time.time;
        }
        else { 
            grounded = false;
        }
    }

    #endregion

    #region Jump

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 100f;
    [SerializeField] private float _addedGravity = 2f;
    [SerializeField] private float _gravityDuration = 0.25f;
    [SerializeField] private float _jumpButtonGracePeriod = 0.2f;
    [SerializeField] private float _coyoteTimeGracePeriod = 0.2f;
    private float apexPoint; // Becomes 1 at apex of jump
    private float? jumpButtonPressedTime;

    private void Jump() {
        if (rb.velocity.y == 0 
                && Time.time - lastGroundedTime <= _coyoteTimeGracePeriod
                && Time.time - jumpButtonPressedTime <= _jumpButtonGracePeriod) {

            if (jumpPressed && earlyJumpPressed) {
                // Trying to avoid double force jump (not working)
                earlyJumpPressed = false;
                return;
            }
            else { 
                earlyJumpPressed = false;
                lastGroundedTime = null;
                jumpButtonPressedTime = null;
                rb.AddForce(new Vector2(0f, _jumpForce * rb.gravityScale));
            }
        }
    }

    private void EndJumpEarly() { 
        if (jumpState == JumpState.Jumping && !jumpPressed && !endJumpEarly && rb.velocity.y > 0) {
            endJumpEarly = true;
            StartCoroutine(AddGravity());
        }
    }

    IEnumerator AddGravity() {
        rb.gravityScale += _addedGravity;
        yield return new WaitForSeconds(_gravityDuration);

        rb.gravityScale = baseGravityScale;
        endJumpEarly = false;
    }
    private void ResetGravity() { 
        if (rb.velocity.y < 0 && endJumpEarly) {
            StopCoroutine(AddGravity());
            rb.gravityScale = baseGravityScale;
            endJumpEarly = false;
        }
    }

    private void CalculateJumpApex() {
        if (!grounded) {
            // Gets stronger the closer to the top of the jump
            apexPoint = Mathf.InverseLerp(_jumpForce, 0, Mathf.Abs(rb.velocity.y));
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
            
            //Debug.Log("Velocity.y: " + rb.velocity.y + "And apexMoveBonus: " + apexMoveBonus);
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

    #region Animation
    [Header("Animation")]
    private JumpState jumpState;

    private void Animate() {
        if (moveVector.x < 0) {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
        else if (moveVector.x > 0) {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }

        if (rb.velocity.y > 0 && !grounded && (jumpState == JumpState.Level || jumpState == JumpState.Landing)) {
            jumpState = JumpState.Jumping;
            anim.Jump();
        }
        else if (rb.velocity.y < 0 && !grounded) {
            // To prevent the jump buffer from constantly jumping if jump is held
            //jumpPressed = false;
            jumpState = JumpState.Falling;
            anim.Fall();
        }
        else if (rb.velocity.y == 0 && grounded && jumpState == JumpState.Jumping || jumpState == JumpState.Falling) {
            jumpState = JumpState.Landing;
            anim.Land();
        }
        else if (jumpState == JumpState.Level) { 
            if (moveVector.x != 0) {
                anim.Run();
            } 
            else {
                anim.Idle();
            }
        }
    }

    // Animation Events
    private void SetJumpStateLevel() {
        jumpState = JumpState.Level;
    }

    #endregion
}
