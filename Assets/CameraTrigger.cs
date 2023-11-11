using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private float _angle = 90;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerCamera cam))
        {
            cam.SetCameraRotation(_angle);
        }
    }
}
