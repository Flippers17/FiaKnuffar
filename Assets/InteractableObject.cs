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
    [SerializeField] private GameObject _outlineGO;

    public WeaponDataSO weaponData => _weaponData;

    public void InRange()
    {
        if (_outlineGO)
            _outlineGO.SetActive(true);
    }
    public void Interact()
    {
        _rb.isKinematic = true;
        _col.enabled = false;

        OnInteract?.Invoke();
    }
    public void OutOfRange()
    {
        if (_outlineGO)
            _outlineGO.SetActive(false);
    }
    public void DropItem()
    {
        _rb.isKinematic = false;
        _col.enabled = true;

        transform.parent = null;
    }
}
