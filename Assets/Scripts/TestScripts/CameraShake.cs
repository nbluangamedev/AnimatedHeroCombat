using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            cinemachineImpulseSource.GenerateImpulse(Camera.main.transform.forward);
        }
    }
}
