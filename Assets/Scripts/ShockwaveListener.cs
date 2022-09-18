using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShockwaveListener : MonoBehaviour
{
    private CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake() {
        source.GenerateImpulse();
    }
}
