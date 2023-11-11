using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerFollowObj;
    [SerializeField] private Transform _camHolder;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Transform _zoomedPos;
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private float _slerpSpeed = 3f;

    [SerializeField] private bool _zoomedIn = false;

    private bool _followObject = true;

    public void SetCameraFollow(bool shouldCameraFollow) => _followObject = shouldCameraFollow;
    public void SetZoom(bool isZoomed) => _zoomedIn = isZoomed;

    public void SetCamRotation(float angle)
    {
        _playerFollowObj.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    private void Update()
    {
        if (_followObject)
        {
            _playerFollowObj.position = transform.position;

            if (!_zoomedIn)
            {
                _camHolder.position = Vector3.Lerp(_camHolder.position, _playerPos.position, Time.deltaTime * _lerpSpeed);
                _camHolder.rotation = Quaternion.Slerp(_camHolder.rotation, _playerPos.rotation, Time.deltaTime * _slerpSpeed);
            }
            else
            {
                _camHolder.position = Vector3.Lerp(_camHolder.position, _zoomedPos.position, Time.deltaTime * _lerpSpeed);
                _camHolder.rotation = Quaternion.Slerp(_camHolder.rotation, _zoomedPos.rotation, Time.deltaTime * _slerpSpeed);
            }
        }
    }
}
