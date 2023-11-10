using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField][Min(0.1f)][Tooltip("Range for interactive items!")] private float _raduis = 1;
    [SerializeField] private LayerMask _interactiveLayer;
    [SerializeField] private PlayerInputHandler _input;
    [SerializeField] private Transform weaponHolder;

    public UnityEvent<WeaponDataSO> OnPickup;

    private InteractableObject _currentInteractable;
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
        if (!_currentInteractable)
            return;
        if (!_currentHeldObject)
        {
            _currentHeldObject = _currentInteractable;

            EquipItem();
        }
        else
        {
            _currentHeldObject.DropItem();
            _currentHeldObject = _currentInteractable;

            EquipItem();
        }
    }

    private void EquipItem()
    {
        _currentHeldObject.transform.parent = weaponHolder;
        _currentHeldObject.transform.localPosition = Vector3.zero;

        WeaponDataSO data = _currentHeldObject.Interact();
        if (data)
            OnPickup?.Invoke(data);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _raduis);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _raduis, _interactiveLayer);

        if (hits.Length > 0)
        {
            InteractableObject closestInteractable = null;

            for (int i = 0; i < hits.Length; i++)
            {
                float closestDistance = float.MaxValue;

                if(hits[i].TryGetComponent(out InteractableObject interactable))
                {
                    float distance = Vector3.Distance(transform.position, hits[i].transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            if(closestInteractable != _currentInteractable)
            {
                if(_currentInteractable)
                    _currentInteractable.OutOfRange();
                _currentInteractable = closestInteractable;
                _currentInteractable.InRange();
            }
        }
        else
        {
            if(_currentInteractable)
            {
                _currentInteractable.OutOfRange();
                _currentInteractable = null;
            }
        }
    }
}
