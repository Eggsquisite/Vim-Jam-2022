using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fire : MonoBehaviour
{
    [SerializeField] private UnityEvent GameWon;

    [SerializeField] private List<Torch> torches;
    [SerializeField] private LayerMask playerLayer;
    private int index;
    private int totalTorches;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        totalTorches = torches.Count;
        anim = GetComponent<Animator>();
    }

    // EVENT FROM TORCH.CS
    public void TorchLit() {
        index++;
        if (index == totalTorches) {
            GameWon.Invoke();
            anim.Play("lit_anim");
        }
    }
}
