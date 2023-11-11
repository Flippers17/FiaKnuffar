using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private bool _destroyOnAwake = false;
    public float destroyTime = 3f;

    private void Awake()
    {
        if(_destroyOnAwake)
            StartDestroy();
    }

    public void StartDestroy()
    {
        Destroy(gameObject, destroyTime);
    }
}
