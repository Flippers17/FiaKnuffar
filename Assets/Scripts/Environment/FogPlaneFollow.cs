using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPlaneFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float yOffset = -30;
    [SerializeField]
    private float _lerpSpeed = .01f;

    [SerializeField]
    float minHeight = 0;


    private void Update()
    {
        float y = Mathf.Max(Mathf.Lerp(transform.position.y, target.position.y + yOffset, _lerpSpeed), minHeight);
        transform.position = new Vector2(0, y);
    }
}
