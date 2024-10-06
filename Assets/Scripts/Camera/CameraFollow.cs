using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Transform _zoomedPos;
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private float _slerpSpeed = 3f;

    [SerializeField] private bool _zoomedIn = false;

    public void SetZoom(bool isZoomedIn) => _zoomedIn = isZoomedIn;

    private void Update()
    {
        if (!_zoomedIn)
        {
            transform.position = Vector3.Lerp(transform.position, _playerPos.position, Time.deltaTime * _lerpSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, _playerPos.rotation, Time.deltaTime * _slerpSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _zoomedPos.position, Time.deltaTime * _lerpSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, _zoomedPos.rotation, Time.deltaTime * _slerpSpeed);
        }
    }
}
