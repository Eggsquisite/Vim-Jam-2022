using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform positionOne;
    [SerializeField] private Transform positionTwo;
    [SerializeField] private float switchDelay;
    private Vector3 savedPosOne, savedPosTwo;
    private int posFlag;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // To account for positions moving as they are children of this gameobject
        savedPosOne = positionOne.position;
        savedPosTwo = positionTwo.position;
    }


    private void Update()
    {
        switch (posFlag) {
            case 1:
                if (transform.position != savedPosOne) { 
                    transform.position = Vector2.MoveTowards(transform.position, savedPosOne, moveSpeed * Time.deltaTime);
                }
                else if (transform.position == savedPosOne) {
                    StartCoroutine(SwitchPosition(2));
                }
                break;
            case 2:
                if (transform.position != savedPosTwo) { 
                    transform.position = Vector2.MoveTowards(transform.position, savedPosTwo, moveSpeed * Time.deltaTime);
                }
                else if (transform.position == savedPosTwo) {
                    StartCoroutine(SwitchPosition(1));
                }
                break;
            default:
                if (transform.position != savedPosOne) { 
                    transform.position = Vector2.MoveTowards(transform.position, savedPosOne, moveSpeed * Time.deltaTime);
                }
                else if (transform.position == savedPosOne) {
                    StartCoroutine(SwitchPosition(2));
                }
                break;
        }
    }

    IEnumerator SwitchPosition(int flag) {

        yield return new WaitForSeconds(switchDelay);
        posFlag = flag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(this.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
