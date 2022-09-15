using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPreview : MonoBehaviour
{
    [Header("Components")]
    private SpriteRenderer sp;

    [Header("Type")]
    [SerializeField] private PlatformType type;
    private bool spriteActive;

    [Header("Movement")]
    [SerializeField] private float maxFollowSpeed;
    [SerializeField] private float smoothTime;
    private float baseFollowSpeed;
    Vector2 _currentVelocity;

    private Vector3 _mousePos, _worldPos;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        spriteActive = sp.enabled;
        baseFollowSpeed = maxFollowSpeed;
    }

    // Update is called once per frame
    void Update()
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
            //transform.position = Vector3.MoveTowards(transform.position, _worldPos, followSpeed * Time.deltaTime);
            transform.position = Vector2.SmoothDamp(transform.position, _worldPos, ref _currentVelocity, smoothTime, maxFollowSpeed);
        }
        else if (type == PlatformType.Ground) { 
            
        }
    }
}
