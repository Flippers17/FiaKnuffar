using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraPosition : MonoBehaviour
{
    [SerializeField] private Transform playerFollowObj;
    [SerializeField] private CameraFollow _camFollow;

    public void SetZoom(bool isZoomed) => _camFollow.SetZoom(isZoomed);

    private void Update()
    {
        playerFollowObj.position = transform.position;
    }
}
