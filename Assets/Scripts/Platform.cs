using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType { 
    Air,
    Ground
}

public class Platform : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private SpriteRenderer sp;
    private PlayerController pc;
    private string currentState;

    [Header("Type")]
    [SerializeField] private PlatformType type;

    [Header("Launch")]
    [SerializeField] private LayerMask allObjsLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform rayCastPos;
    [SerializeField] private float rayCastLength = 1.5f;
    [SerializeField] private float forceUp = 300f;
    private bool isExpanding;

    private void Awake() {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (type == PlatformType.Ground) { 
            Debug.DrawRay(rayCastPos.position, Vector2.right * rayCastLength, Color.red);

            if (isExpanding) {
                RaycastHit2D hitPlayer = Physics2D.Raycast(rayCastPos.position, Vector2.right, rayCastLength, playerLayer);
                if (hitPlayer.collider != null) { 
                    pc = hitPlayer.collider.GetComponent<PlayerController>();
                    pc.SetUnableToJump(true);    
                }
            }
        }
    }

    public void DeleteAnimation() {
        sp.sortingOrder -= 1;
        AnimHelper.ChangeAnimationState(anim, ref currentState, "delete_anim");
    }

    // Animation Events
    private void Launch() {
        if (type == PlatformType.Air)
            return;

        RaycastHit2D[] hit = Physics2D.RaycastAll(rayCastPos.position, Vector2.right, rayCastLength, allObjsLayer);
        foreach(RaycastHit2D launchedObj in hit) {
            var rb = launchedObj.collider.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, forceUp));
        }
    }

    private void SetIsExpanding(int flag) {
        switch(flag) {
            case 0:
                isExpanding = false;
                if (pc != null)
                    pc.SetUnableToJump(false);
                break;
            case 1:
                isExpanding = true;
                break;
        
        }
    }

    private void DestroyPlatform() {
        Destroy(gameObject);
    }
}
