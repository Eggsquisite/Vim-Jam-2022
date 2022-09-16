using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPreview : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer sp;

    [Header("Type")]
    [SerializeField] private PlatformType type;
    private bool spriteActive;

    [Header("Raycasts (For Ground Platform)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform frontRay;
    [SerializeField] private Transform backRay;
    [SerializeField] private float length;
    [SerializeField] private Vector2 offset;

    [Header("Movement (For Air Platform)")]
    [SerializeField] private float maxFollowSpeed;
    [SerializeField] private float smoothTime;
    private float baseFollowSpeed;
    private Vector2 _currentVelocity, airPos, groundPos;

    private Vector3 _mousePos, _worldPos;

    private void Awake()
    {
        if (sp == null) sp = GetComponent<SpriteRenderer>();
        spriteActive = sp.enabled;
        baseFollowSpeed = maxFollowSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sp.enabled == false && spriteActive) {
            spriteActive = false;
            maxFollowSpeed = 1000f;
        }
        else if (sp.enabled == true && !spriteActive) {
            spriteActive = true;
            maxFollowSpeed = baseFollowSpeed;
        }

        _mousePos = Mouse.current.position.ReadValue();
        _mousePos.z = Camera.main.nearClipPlane;
        _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);

        if (type == PlatformType.Air) {
            //transform.position = Vector2.SmoothDamp(transform.position, _worldPos, ref _currentVelocity, smoothTime, maxFollowSpeed);
            airPos = new Vector2(_worldPos.x, _worldPos.y);
            transform.position = Vector2.MoveTowards(transform.position, airPos, maxFollowSpeed * Time.deltaTime);
        }
        else if (type == PlatformType.Ground) {

            transform.position = Vector2.SmoothDamp(transform.position, _worldPos, ref _currentVelocity, smoothTime, maxFollowSpeed);
            RaycastHit2D frontHit = Physics2D.Raycast(frontRay.position, Vector2.down, length, groundLayer);
            RaycastHit2D backHit = Physics2D.Raycast(backRay.position, Vector2.down, length, groundLayer);
            if (frontHit.collider != null && frontHit.collider.tag == "Ground" 
                    && backHit.collider != null && backHit.collider.tag == "Ground")
                CheckRaycasts(frontHit, backHit);
        }
    }

    #region Raycasts
    [SerializeField] private float raycastOffset;
    private bool frontLower, backLower;
    private float initialPos;

    private void CheckRaycasts(RaycastHit2D frontRay, RaycastHit2D backRay) {
        var frontTransform = frontRay.collider.transform;
        var backTransform = backRay.collider.transform;

        if (frontTransform.position.y > backTransform.position.y) { 
            switch (backLower) {
                case false:
                    backLower = true;
                    initialPos = backRay.transform.position.x + raycastOffset;
                    break;
                default:
                    groundPos = new Vector2(initialPos, backTransform.position.y + offset.y);
                    break;
            }
        }
        else if (frontTransform.position.y < backTransform.position.y) {
            switch (frontLower) {
                case false:
                    frontLower = true;
                    initialPos = frontRay.transform.position.x - raycastOffset;
                    break;
                default:
                    groundPos = new Vector2(initialPos, frontTransform.position.y + offset.y);
                    break;
            }
        }
        else if (frontTransform.position.y == backTransform.position.y) {
            initialPos = 0f;
            frontLower = backLower = false;
            groundPos = new Vector2(_worldPos.x, backTransform.position.y + offset.y);
        }

        //sp.transform.position = Vector2.MoveTowards(sp.transform.position, groundPos, maxFollowSpeed * Time.deltaTime);
        sp.transform.position = new Vector2(groundPos.x, groundPos.y);
    }

    #endregion
}
