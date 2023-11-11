using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 _targetPos;

    [SerializeField]
    private float lerpSpeed = 3f;
    private bool _isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _targetPos = target.position;
    }


    private void Update()
    {
        if (_isMoving && Vector3.Distance(transform.position, _targetPos) > 0.1f)
            transform.position = Vector3.Slerp(transform.position, _targetPos, lerpSpeed * Time.deltaTime);
    }

    public void MoveToPosition()
    {

    }
}
