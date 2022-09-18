using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Components")]
    private Enemy enemyScript;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float moveClamp;
    [SerializeField] private float switchDelay;
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform positionOne;
    [SerializeField] private Transform positionTwo;
    private int posFlag = 1;
    private Rigidbody2D enemyRb;
    private bool switchFlag = true;
    private Vector2 direction;

    private void Start()
    {
        enemyRb = enemy.GetComponent<Rigidbody2D>();
        enemyScript = enemy.GetComponent<Enemy>();
        enemy.position = new Vector2(enemy.position.x, positionOne.position.y);
    }

    private void FixedUpdate()
    {
        if (positionOne == null || positionTwo == null)
            return;

        if (!enemyScript.GetCanMove()) {
            direction = Vector2.zero;
            enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);
            return;
        }

        switch (posFlag) {
            case 1:
                if (enemyRb.position.x > positionOne.position.x + 0.5f || enemyRb.position.x < positionOne.position.x - 0.5f) {
                    direction = ((Vector2)positionOne.position - enemyRb.position).normalized;
                    //enemyRb.MovePosition(enemyRb.position + new Vector2(direction.x, 0f) * moveSpeed * Time.deltaTime);
                    enemyRb.velocity = new Vector2(Mathf.Clamp(direction.x * acceleration * Time.deltaTime, -moveClamp, moveClamp), enemyRb.velocity.y);
                }
                else {
                    direction = Vector2.zero;
                    enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);
                    if (switchFlag)
                        StartCoroutine(SwitchPosition(2));
                }
                break;
            case 2:
                if (enemyRb.position.x > positionTwo.position.x + 0.5f || enemyRb.position.x < positionTwo.position.x - 0.5f) {
                    direction = ((Vector2)positionTwo.position - enemyRb.position).normalized;
                    //enemyRb.MovePosition(enemyRb.position + new Vector2(direction.x, 0f) * moveSpeed * Time.deltaTime);
                    enemyRb.velocity = new Vector2(Mathf.Clamp(direction.x * acceleration * Time.deltaTime, -moveClamp, moveClamp), enemyRb.velocity.y);
                }
                else {
                    direction = Vector2.zero;
                    enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);
                    if (switchFlag)
                        StartCoroutine(SwitchPosition(1));
                }
                break;
            default:
                posFlag = 1;
                break;
        }
    }

    IEnumerator SwitchPosition(int flag) {
        switchFlag = false;
        yield return new WaitForSeconds(switchDelay);
        switchFlag = true;
        posFlag = flag;
    }
}
