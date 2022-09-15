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
    private string currentState;

    [Header("Type")]
    [SerializeField] private PlatformType type;

    [Header("Launch")]
    [SerializeField] private float forceUp;
    [SerializeField] private LayerMask colliderLayer;
    [SerializeField] private List<Transform> positions;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (type == PlatformType.Ground) 
            Debug.DrawRay(positions[0].position, Vector2.up, Color.red);
    }

    public void DeletePlatform() {
        AnimHelper.ChangeAnimationState(anim, ref currentState, "delete_anim");
    }

    // Animation Events
    private void Launch() {
        if (type == PlatformType.Air)
            return;

        foreach (Transform raycastPos in positions) {
            RaycastHit2D hit = Physics2D.Raycast(raycastPos.position, Vector2.up, 1f, colliderLayer);
            if (hit.collider != null) {

                Debug.Log("Collider hit! " + hit.collider.name);
                var rb = hit.collider.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0f, forceUp * rb.gravityScale));
            }
        }
    }

    private void DestroyPlatform() {
        Destroy(gameObject);
    }
}
