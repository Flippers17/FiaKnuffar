using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private bool _touched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_touched)
            return;

        if(other.TryGetComponent(out PlayerHealth health))
        {
            _touched = true;
            health.TakeDamage(99);
        }
    }
}
