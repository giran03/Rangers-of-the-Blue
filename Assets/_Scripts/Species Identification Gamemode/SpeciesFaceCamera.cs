using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesFaceCamera : MonoBehaviour
{
    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
