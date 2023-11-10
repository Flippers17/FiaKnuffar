using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField][Min(0.1f)][Tooltip("Range for interactive items!")] private float _raduis = 1;
    [SerializeField] private Transform _interactionCenter;
    [SerializeField] private LayerMask _interactiveLayer;
    [SerializeField] private PlayerInputHandler _input;
    [SerializeField] private Transform weaponHolder;

    public WeaponDataSO currentWeaponData => _currentHeldObject.weaponData;

    private InteractableObject _currentClosestInteractable;
    private InteractableObject _currentHeldObject;

    private void OnEnable()
    {
        _input.OnInteract += Interact;
    }

    private void OnDisable()
    {
        _input.OnInteract -= Interact;
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
        else if (!_currentClosestInteractable)
        {
            _currentHeldObject.DropItem();
            _currentHeldObject = null;
        }
        else
        {
            _currentHeldObject.DropItem();
            _currentHeldObject = _currentClosestInteractable;

            EquipItem();
        }
    }

    private void EquipItem()
    {
        _currentHeldObject.transform.parent = weaponHolder;
        _currentHeldObject.transform.localPosition = Vector3.zero;

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
