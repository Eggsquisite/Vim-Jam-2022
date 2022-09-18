using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class ShockwaveListener : MonoBehaviour
{
    //private CinemachineImpulseSource source;
    [SerializeField] private UnityEvent cameraShake;

    public void CameraShakeEvent() {
        cameraShake.Invoke();
        //source.GenerateImpulseWithForce(shakeForce);
    }
}
