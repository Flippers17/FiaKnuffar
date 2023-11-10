using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityAction OnInteract;
    [SerializeField] private WeaponDataSO weaponData;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    public void InRange()
    {
        Debug.Log("In Range: " + gameObject.name);
    }
    public WeaponDataSO Interact()
    {
        rb.useGravity = false;
        col.enabled = false;

        OnInteract?.Invoke();

        return weaponData;
    }
    public void OutOfRange()
    {
        Debug.Log("Out Of Range: " + gameObject.name);
    }
    public void DropItem()
    {
        rb.useGravity = true;
        col.enabled = true;

        transform.parent = null;
    }
}
