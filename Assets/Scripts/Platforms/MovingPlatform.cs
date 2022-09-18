using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform platform;
    [SerializeField] private Transform positionOne;
    [SerializeField] private Transform positionTwo;
    [SerializeField] private float switchDelay;
    private int posFlag;
    private bool switchFlag = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        platform.position = new Vector2(positionOne.position.x, positionOne.position.y);
    }

    private void Update()
    {
        switch (posFlag) {
            case 1:
                if (platform.position != positionOne.position) { 
                    platform.position = Vector2.MoveTowards(platform.position, positionOne.position, moveSpeed * Time.deltaTime);
                }
                else if (platform.position == positionOne.position && switchFlag) {
                    StartCoroutine(SwitchPosition(2));
                }
                break;
            case 2:
                if (platform.position != positionTwo.position) {
                    platform.position = Vector2.MoveTowards(platform.position, positionTwo.position, moveSpeed * Time.deltaTime);
                }
                else if (platform.position == positionTwo.position && switchFlag) {
                    StartCoroutine(SwitchPosition(1));
                }
                break;
            default:
                if (platform.position != positionOne.position) {
                    platform.position = Vector2.MoveTowards(platform.position, positionOne.position, moveSpeed * Time.deltaTime);
                }
                else if (platform.position == positionOne.position && switchFlag) {
                    StartCoroutine(SwitchPosition(2));
                }
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
