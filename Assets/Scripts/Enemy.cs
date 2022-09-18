using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyType { 
        Buzz,
        Soldier
    }

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Enemy Type")]
    [SerializeField] private EnemyType type;

    [Header("Attack")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackLength;

    public bool canMove = true;
    private bool canAttack = true;
    private bool isAttacking, isDead;
    private string currentState;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        if (!isAttacking) FaceDirection();
        if (type == EnemyType.Soldier) Animate();
    }

    private void FaceDirection() { 
        if (rb.velocity.x < 0f) {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
        else if (rb.velocity.x >= 0f) {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
    }

    private void Animate() { 
        CheckForAttack();
        if (!isAttacking) {
            AnimHelper.ChangeAnimationState(anim, ref currentState, "idle_anim");
        }
    }

    private void CheckForAttack() {
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, Vector2.right, attackLength, playerLayer);
        RaycastHit2D backHit = Physics2D.Raycast(transform.position, Vector2.left, attackLength, playerLayer);
        Debug.DrawRay(transform.position, Vector2.right * attackLength, Color.blue);
        Debug.DrawRay(transform.position, Vector2.left * attackLength, Color.red);

        if (frontHit.collider != null) {
            // Face direction and attack
            transform.localScale = new Vector2(1f, transform.localScale.y);
            AnimHelper.ChangeAnimationState(anim, ref currentState, "attack_anim");
        }
        else if (backHit.collider != null)
        {
            // Face direction and attack
            transform.localScale = new Vector2(-1f, transform.localScale.y);
            AnimHelper.ChangeAnimationState(anim, ref currentState, "attack_anim");
        }
    }

    // GETTERS / SETTERS ///////////////////////////////////////////////////////////////////////
    public bool GetCanMove() {
        return canMove;
    }

    public void SetCanAttack(bool flag) {
        canAttack = flag;
    }

    // Animation Events
    public void SetIsAttacking(int flag) {
        switch (flag) {
            case 0:
                isAttacking = false;
                break;
            case 1:
                isAttacking = true;
                break;
        }
    }
}
