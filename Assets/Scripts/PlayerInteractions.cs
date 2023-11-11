using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField][Min(0.1f)][Tooltip("Range for interactive items!")] private float _raduis = 1;
    [SerializeField] private Transform _interactionCenter;
    [SerializeField] private LayerMask _interactiveLayer;
    [SerializeField] private PlayerInputHandler _input;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float _throwForce = 10;
    [SerializeField] private float _dropDistance = 1.5f;
    [SerializeField] private Animator _anim;

    public WeaponDataSO currentWeaponData
    {
        get
        {
            if (_currentHeldObject == null)
                return null;

            return _currentHeldObject.weaponData;
        }
    }

    private InteractableObject _currentClosestInteractable;
    private InteractableObject _currentHeldObject;

    private void OnEnable()
    {
        _input.OnInteract += Interact;
        _input.OnDrop += DropItem;
        _input.OnThrow += ThrowItem;
    }

    private void OnDisable()
    {
        _input.OnInteract -= Interact;
        _input.OnDrop -= DropItem;
        _input.OnThrow -= ThrowItem;
    }

    private void Interact()
    {
        if (!_currentClosestInteractable)
            return;
        if (!_currentHeldObject)
        {
            _currentHeldObject = _currentClosestInteractable;

            EquipItem();
        }
        else
        {
            _currentHeldObject.DropItem();
            _currentHeldObject = _currentClosestInteractable;

            EquipItem();
        }
    }

    private void ThrowItem()
    {
        if (_currentHeldObject)
        {
            Vector3 distance = transform.position + transform.forward * _dropDistance;
            _anim.SetBool("HasWeapon", false);
            _currentHeldObject.transform.position = new Vector3(distance.x, weaponHolder.position.y, distance.z);

            _currentHeldObject.ThrowItem(transform.forward, _throwForce);
            _currentHeldObject = null;
        }
    }

    private void DropItem()
    {
        if (_currentHeldObject)
        {
            Vector3 distance = transform.position + transform.forward * _dropDistance;
            _anim.SetBool("HasWeapon", false);
            _currentHeldObject.transform.position = new Vector3(distance.x, weaponHolder.position.y, distance.z);
            _currentHeldObject.DropItem();
            _currentHeldObject = null;
        }
    }

    private void EquipItem()
    {
        _currentHeldObject.transform.parent = weaponHolder;
        _currentHeldObject.transform.localPosition = Vector3.zero;
        _currentHeldObject.transform.rotation = Quaternion.identity;

        _anim.SetBool("HasWeapon", true);

        _currentHeldObject.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_interactionCenter.position, _raduis);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(_interactionCenter.position, _raduis, _interactiveLayer);

        if (hits.Length > 0)
        {
            InteractableObject closestInteractable = null;

            for (int i = 0; i < hits.Length; i++)
            {
                float closestDistance = float.MaxValue;

                if(hits[i].TryGetComponent(out InteractableObject interactable))
                {
                    float distance = Vector3.Distance(_interactionCenter.position, hits[i].transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            if(closestInteractable != _currentClosestInteractable)
            {
                if(_currentClosestInteractable)
                    _currentClosestInteractable.OutOfRange();
                _currentClosestInteractable = closestInteractable;
                _currentClosestInteractable.InRange();
            }
        }
        else
        {
            if(_currentClosestInteractable)
            {
                _currentClosestInteractable.OutOfRange();
                _currentClosestInteractable = null;
            }
        }
    }
}
