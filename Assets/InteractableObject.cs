using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityAction OnInteract;
    [SerializeField] private WeaponDataSO _weaponData;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;

    public WeaponDataSO weaponData => _weaponData;

    public void InRange()
    {
        Debug.Log("In Range: " + gameObject.name);
    }
    public void Interact()
    {
        _rb.useGravity = false;
        _col.enabled = false;

        OnInteract?.Invoke();
    }
    public void OutOfRange()
    {
        Debug.Log("Out Of Range: " + gameObject.name);
    }
    public void DropItem()
    {
        _rb.useGravity = true;
        _col.enabled = true;

        transform.parent = null;
    }
}
