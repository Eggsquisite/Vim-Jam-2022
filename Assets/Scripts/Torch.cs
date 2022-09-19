using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Torch : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLight;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Light() {
        OnLight.Invoke();
        anim.Play("lit_anim");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire Head") {
            Light();
        }
    }
}
