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

    //public TMPro.TextMeshProUGUI _title;

    [Header("Input Variables")]
    private PlayerInputControls inputControls;
    private InputAction movementInput;
    private InputAction mousePosition;

    [Header("Respawn")]
    private Vector2 spawnPos;

    [Header("Components")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private HealthUI healthUI;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private PlayerAnimations anim;
    private ShockwaveListener shake;
    private Dialogue dialogue;

    [Header("Movement Variables")]
    private bool grounded;
    private float currentHorizontalSpeed;

    [Header("Jump Input")]
    private float baseGravityScale;
    private bool jumpPressed, earlyJumpPressed, endJumpEarly;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<PlayerAnimations>();
        shake = GetComponent<ShockwaveListener>();
        inputControls = new PlayerInputControls();

        spawnPos = transform.position;
        baseGravityScale = rb.gravityScale;
        jumpState = JumpState.Level;
    }

    #region Events

    private void OnEnable() {
        movementInput = inputControls.Player.Movement;
        movementInput.Enable();

        mousePosition = inputControls.Player.MousePosition;
        mousePosition.Enable();

        inputControls.Player.Jump.performed += OnJump;
        inputControls.Player.Jump.canceled += EndJumpEarly;
        inputControls.Player.Jump.Enable();

        inputControls.Player.SelectAirPlatform.performed += AirPreview;
        inputControls.Player.SelectAirPlatform.Enable();

        inputControls.Player.SelectGroundPlatform.performed += GroundPreview;
        inputControls.Player.SelectGroundPlatform.Enable();

        inputControls.Player.Spawn.performed += SpawnPlatform;
        inputControls.Player.Spawn.Enable();

        inputControls.Player.Despawn.performed += DespawnPlatform;
        inputControls.Player.Despawn.Enable();

        inputControls.Player.Interact.performed += Interact;
        inputControls.Player.Interact.Enable();
    }

    private void OnDisable() {
        movementInput.Disable();
        mousePosition.Disable();

        inputControls.Player.Jump.performed -= OnJump;
        inputControls.Player.Jump.canceled -= EndJumpEarly;
        inputControls.Player.Jump.Disable();

        inputControls.Player.SelectAirPlatform.performed -= AirPreview;
        inputControls.Player.SelectAirPlatform.Disable();

        inputControls.Player.SelectGroundPlatform.performed -= GroundPreview;
        inputControls.Player.SelectGroundPlatform.Disable();

        inputControls.Player.Spawn.performed -= SpawnPlatform;
        inputControls.Player.Spawn.Disable();

        inputControls.Player.Despawn.performed -= DespawnPlatform;
        inputControls.Player.Despawn.Disable();

        inputControls.Player.Interact.performed -= Interact;
        inputControls.Player.Interact.Disable();
    }

    private void OnJump(InputAction.CallbackContext obj) {
        jumpPressed = obj.action.triggered;
        if (jumpPressed) jumpButtonPressedTime = Time.time;
        if (jumpPressed && jumpState == JumpState.Falling && lastGroundedTime == null) earlyJumpPressed = obj.action.triggered;
    }

    private void EndJumpEarly(InputAction.CallbackContext obj) {
        jumpPressed = obj.action.triggered;
        EndJumpEarly(0f);
    }

    private void AirPreview(InputAction.CallbackContext obj) {
        spawner.SetAirPreview();
    }

    private void GroundPreview(InputAction.CallbackContext obj) {
        spawner.SetGroundPreview();
    }

    private void SpawnPlatform(InputAction.CallbackContext obj) {
        spawner.SpawnPlatform();
    }

    private void DespawnPlatform(InputAction.CallbackContext obj) {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null && (hit.collider.tag == "AirPlatform" || hit.collider.tag == "GroundPlatform")) {
            hit.collider.GetComponent<Platform>().DeleteAnimation();
        }
    }

    private void Interact(InputAction.CallbackContext obj) {
        if (dialogue != null) {
            dialogue.gameObject.SetActive(true);
            dialogue.StartDialogue();
        }
    }

    #endregion

    void Update() {
        if (isDead) return;

        Jump();
        Animate();
        GetInput();
        HurtFlash();
        CheckGrounded();
        ResetGravity();
        CalculateJumpApex();
        CalculateMovement();

        //_title.text = (Time.time - lastGroundedTime).ToString();
    }

    void FixedUpdate() {
        if (isHurt || isDead) return;

        Movement(); 
    }

    #region Input Values
    private Vector2 _moveVector;
    private Vector3 _mousePos, _worldPos;

    private void GetInput() {
        _moveVector = movementInput.ReadValue<Vector2>().normalized;
    }

    public void GetDialogue(Dialogue _dialogue) {
        dialogue = _dialogue;
    }

    #endregion

    #region Hurt
    [Header("Hurt")]
    [SerializeField] private float hurtDuration;
    [SerializeField] private float invincibleDuration;
    [SerializeField] private float knockbackMod;
    [SerializeField] private float spriteFlashMaxDuration;
    private float spriteFlashTimer;
    private bool isInvincible, isHurt, isDead;
    private Coroutine invincibleRoutine;

    public void Hurt(Vector2 direction, float knockBackSpeed) {
        if (isInvincible || isDead) return;

        // Shake camera event
        shake.CameraShakeEvent();

        rb.velocity = Vector2.zero;
        if (rb.velocity.y == 0f)
            rb.AddForce(new Vector2(direction.x * knockbackMod, 1f * knockbackMod) * knockBackSpeed * Time.deltaTime, ForceMode2D.Impulse);

        if (direction.x < 0) {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
        else if (direction.x > 0) {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }

        if (healthUI.TakeDamage() <= 0) {
            Death();
        }
        else { 
            if (invincibleRoutine != null) StopCoroutine(invincibleRoutine);
            invincibleRoutine = StartCoroutine(HurtDelays());
        }
    }

    IEnumerator HurtDelays() {
        isHurt = true;
        isInvincible = true;
        anim.Hurt();

        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
        anim.Idle();

        yield return new WaitForSeconds(invincibleDuration - hurtDuration);
        isInvincible = false;
    }

    private void HurtFlash() { 
        if (isInvincible) { 
            if (spriteFlashTimer < spriteFlashMaxDuration) {
                spriteFlashTimer += Time.deltaTime;
            }
            else if (spriteFlashTimer >= spriteFlashMaxDuration) {
                spriteFlashTimer = 0f;
                sp.enabled = !sp.enabled;
            }
        }
        else {
            sp.enabled = true;
        }
    }

    public void Death() {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine() {
        isDead = true;
        anim.Death();
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        healthUI.Respawn();
        Respawn();
    }

    private void Respawn() {
        isDead = false;
        anim.Idle();
        transform.position = spawnPos;
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

        if (downBack.collider != null || downFront.collider != null) { 
            grounded = true;
            lastGroundedTime = Time.time;

            // Check to see if player is standing on a created platform, if so stop them from creating that same type of platform
            CheckReadyToSpawn();
        }
        else { 
            grounded = false;

            // Disallow player from spawning platforms in the air
            spawner.ReadyToSpawnAir(false);
            spawner.ReadyToSpawnGround(false);
        }
    }

    private void CheckReadyToSpawn() {
        if (downBack.collider != null && downFront.collider == null) {
            if (downBack.collider.tag == "AirPlatform") {
                spawner.ReadyToSpawnAir(false);
                spawner.ReadyToSpawnGround(true);
            }
            else if (downBack.collider.tag == "GroundPlatform") {
                spawner.ReadyToSpawnGround(false);
                spawner.ReadyToSpawnAir(true);
            }
            else {
                unableToJump = false;
                spawner.ReadyToSpawnAir(true);
                spawner.ReadyToSpawnGround(true);
            }
        }
        else if (downBack.collider == null && downFront.collider != null) {
            if (downFront.collider.tag == "AirPlatform") {
                spawner.ReadyToSpawnAir(false);
                spawner.ReadyToSpawnGround(true);
            }
            else if (downFront.collider.tag == "GroundPlatform") {
                spawner.ReadyToSpawnGround(false);
                spawner.ReadyToSpawnAir(true);
            }
            else {
                unableToJump = false;
                spawner.ReadyToSpawnAir(true);
                spawner.ReadyToSpawnGround(true);
            }
        }
        else if (downBack.collider != null && downFront.collider != null) {
            if (downBack.collider.tag == "AirPlatform" || downFront.collider.tag == "AirPlatform") {
                spawner.ReadyToSpawnAir(false);
                spawner.ReadyToSpawnGround(true);

                if (downBack.collider.tag == "GroundPlatform" || downFront.collider.tag == "GroundPlatform") {
                    spawner.ReadyToSpawnGround(false);
                }
            } 
            else if (downBack.collider.tag == "GroundPlatform" || downFront.collider.tag == "GroundPlatform") {
                spawner.ReadyToSpawnGround(false);
                spawner.ReadyToSpawnAir(true);

                if (downBack.collider.tag == "AirPlatform" || downFront.collider.tag == "AirPlatform") {
                    spawner.ReadyToSpawnAir(false);
                }
            }
            else {
                unableToJump = false;
                spawner.ReadyToSpawnAir(true);
                spawner.ReadyToSpawnGround(true);
            }
        }
    }

    #endregion

    #region Jump

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 100f;
    [SerializeField] private float _jumpButtonGracePeriod = 0.2f;
    [SerializeField] private float _coyoteTimeGracePeriod = 0.2f;
    [SerializeField] private float _addedGravity = 2f;
    [SerializeField] private float _endJumpEarlyDelay = 0.05f;
    [SerializeField] private float _gravityDuration = 0.25f;
    private float apexPoint; // Becomes 1 at apex of jump
    private bool unableToJump; // used when groundedPlatform is launching
    private float? jumpButtonPressedTime;

    private void Jump() {
        if (unableToJump)
            return;

        if (jumpState != JumpState.Jumping
            && Time.time - lastGroundedTime <= _coyoteTimeGracePeriod
            && Time.time - jumpButtonPressedTime <= _jumpButtonGracePeriod) {

            //  Reset buffer timers
            lastGroundedTime = null;
            jumpButtonPressedTime = null;

            // Cause short hop if buffering jump without holding it
            if (!jumpPressed && earlyJumpPressed) {
                endJumpEarly = true;
                StartCoroutine(AddGravity(_endJumpEarlyDelay));
            }

            // Reset y velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            // Case for jump buffer
            if (earlyJumpPressed && grounded && rb.velocity.y == 0) {
                earlyJumpPressed = false;
                rb.AddForce(new Vector2(0f, _jumpForce * rb.gravityScale));
            }
            // Case for coyote time buffer and regular jump
            else if (!earlyJumpPressed && jumpPressed && rb.velocity.y <= 0) {
                rb.AddForce(new Vector2(0f, _jumpForce * rb.gravityScale));
            }
        }
    }

    private void EndJumpEarly(float delay) { 
        if (jumpState == JumpState.Jumping && !jumpPressed && !endJumpEarly && rb.velocity.y > 0) {
            endJumpEarly = true;
            StartCoroutine(AddGravity(delay));
        }
    }

    IEnumerator AddGravity(float delay) {
        yield return new WaitForSeconds(delay);
        rb.gravityScale += _addedGravity;
        yield return new WaitForSeconds(_gravityDuration);

        rb.gravityScale = baseGravityScale;
        endJumpEarly = false;
    }

    private void ResetGravity() { 
        if (rb.velocity.y < 0 && endJumpEarly) {
            StopCoroutine(AddGravity(0f));
            rb.gravityScale = baseGravityScale;
            endJumpEarly = false;
        }
    }

    private void CalculateJumpApex() {
        if (jumpState == JumpState.Jumping || jumpState == JumpState.Falling) {
            // Gets stronger the closer to the top of the jump
            apexPoint = Mathf.InverseLerp(30f, 0f, Mathf.Abs(rb.velocity.y));
        }
        else {
            apexPoint = 0;
        }
    }

    public void SetUnableToJump(bool flag) {
        unableToJump = flag;
    }

    #endregion

    #region Movement

    [Header("Movement")]
    [SerializeField] private float acceleration = 90;
    [SerializeField] private float deAcceleration = 60;
    [SerializeField] private float moveClamp = 15;
    [SerializeField] private float apexBonus = 2;

    private void CalculateMovement() { 
        if (_moveVector.x != 0) { 
            // Set horizontal move speed
            currentHorizontalSpeed += _moveVector.x * acceleration * Time.deltaTime;

            // Clamp max movement
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);

            // Apply bonus at the apex of a jump
            var apexMoveBonus = Mathf.Sign(_moveVector.x) * apexBonus * apexPoint;
            currentHorizontalSpeed += apexMoveBonus * Time.deltaTime;
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
        if (isHurt) {
            return;
        }
        else {
            if (_moveVector.x < 0) {
                transform.localScale = new Vector2(-1f, transform.localScale.y);
            }
            else if (_moveVector.x > 0) {
                transform.localScale = new Vector2(1f, transform.localScale.y);
            }

            if (rb.velocity.y > 0 && !grounded && (jumpState == JumpState.Level || jumpState == JumpState.Landing)) {
                jumpState = JumpState.Jumping;
                lastGroundedTime = null;
                anim.Jump();
            }
            else if (rb.velocity.y < 0 && !grounded) {
                jumpState = JumpState.Falling;
                anim.Fall();
            }
            else if (rb.velocity.y == 0 && grounded && jumpState == JumpState.Jumping || jumpState == JumpState.Falling) {
                jumpState = JumpState.Landing;
                anim.Land();
            }
            else if (jumpState == JumpState.Level) {
                if (_moveVector.x != 0) {
                    anim.Run();
                }
                else {
                    anim.Idle();
                }
            }
        }
    }

    // Animation Events
    private void SetJumpStateLevel() {
        jumpState = JumpState.Level;
    }

    #endregion
}
