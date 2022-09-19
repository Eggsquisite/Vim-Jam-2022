using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Components")]
    private SpriteRenderer sp;

    [Header("Movement")]
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private bool moveLeft;
    private Vector2 originalPos;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        originalPos = transform.position;
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft) {
            transform.Translate(new Vector2(-1f, 0f) * moveSpeed * Time.deltaTime);
        }
        else {
            transform.Translate(new Vector2(1f, 0f) * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RespawnCollider") {
            transform.position = originalPos;
        }
    }
}
