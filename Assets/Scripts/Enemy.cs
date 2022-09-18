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
    [SerializeField] private float knockBackSpeed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectAttackLength;
    [SerializeField] private Transform forwardAttackPoint;
    [SerializeField] private Vector2 forwardAttackRange;
    [SerializeField] private Transform upwardAttackPoint;
    [SerializeField] private float upwardAttackRange;

    public bool canMove = true;
    private bool canAttack = true;
    private bool isAttacking, isDead, destroyThisObject;
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
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, Vector2.right, detectAttackLength, playerLayer);
        RaycastHit2D backHit = Physics2D.Raycast(transform.position, Vector2.left, detectAttackLength, playerLayer);
        Debug.DrawRay(transform.position, Vector2.right * detectAttackLength, Color.blue);
        Debug.DrawRay(transform.position, Vector2.left * detectAttackLength, Color.red);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == EnemyType.Buzz) { 
            if (collision.collider.tag == "Fire Head") {
                isDead = true;
                canMove = false;
                AnimHelper.ChangeAnimationState(anim, ref currentState, "death_anim");
            }
            else if (collision.collider.tag == "Player" && !isDead) {
                var direction = (collision.transform.position - transform.position).normalized;
                collision.transform.GetComponent<PlayerController>().Hurt(direction, knockBackSpeed);
            }
        }
    }

    // Animation Events
    private void ForwardAttack() {
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(forwardAttackPoint.position, forwardAttackRange, playerLayer);

        foreach(Collider2D player in hitPlayer) {
            if (player.tag == "Player") { 
                var direction = (player.transform.position - transform.position).normalized;
                player.GetComponent<PlayerController>().Hurt(direction, knockBackSpeed);
            }
        }
    }

    private void UpwardAttack() {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(upwardAttackPoint.position, upwardAttackRange, playerLayer);

        foreach(Collider2D player in hitPlayer) {
            if (player.tag == "Player") { 
                var direction = (player.transform.position - transform.position).normalized;
                player.GetComponent<PlayerController>().Hurt(direction, knockBackSpeed);
            }
        }
    }
    
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

    private void Death() {
        destroyThisObject = true;
    }

    // GETTERS / SETTERS ///////////////////////////////////////////////////////////////////////
    public bool GetCanMove() {
        return canMove;
    }

    public void SetCanAttack(bool flag) {
        canAttack = flag;
    }

    public bool GetDestroyThisObject() {
        return destroyThisObject;
    }

    // DRAW LINES //////////////////////
    private void OnDrawGizmosSelected()
    {
        if (upwardAttackPoint == null || forwardAttackPoint == null) return;

        Gizmos.DrawWireSphere(upwardAttackPoint.position, upwardAttackRange);
        Gizmos.DrawWireCube(forwardAttackPoint.position, forwardAttackRange);
    }
}
