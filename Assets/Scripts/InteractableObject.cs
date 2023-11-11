using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class InteractableObject : MonoBehaviour
{
    public UnityAction OnInteract;
    [SerializeField] private WeaponDataSO _weaponData;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;
    [SerializeField] private GameObject _outlineGO;
    [SerializeField] private int throwAttack = 1;
    [SerializeField] private int durabillity = 1;

    public UnityAction OnDie;

    public WeaponDataSO weaponData => _weaponData;

    private bool isThrown = false;

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
    public void ThrowItem(Vector3 dir, float throwForce)
    {
        _rb.isKinematic = false;
        _col.enabled = true;
        isThrown = true;

        _rb.AddForce(dir * throwForce, ForceMode.Impulse);

        transform.parent = null;
    }

    public void DropItem()
    {
        _rb.isKinematic = false;
        _col.enabled = true;

        transform.parent = null;
    }

    public void ReduceDurabillity()
    {
        durabillity--;
        if (durabillity <= 0)
            Die();
    }

    private void Die()
    {
        OnDie?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown)
            return;

        if(collision.collider.gameObject.layer == 8)
        {
            EnemyBehaviour enemy = collision.collider.gameObject.GetComponent<EnemyBehaviour>();

            if (enemy.quickTimeEvents.Count > throwAttack)
                return;

            enemy.GetPushed(new Vector3(0, 0, 4));
        }
    }
}
