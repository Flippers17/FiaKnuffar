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

        Debug.Log("Yes");
        if (!_currentHeldObject)
        {
            _currentHeldObject = _currentInteractable;

            _currentHeldObject.transform.parent = weaponHolder;
            _currentHeldObject.transform.position = Vector3.zero;

            WeaponDataSO data = _currentHeldObject.Interact();
            if (data)
                OnPickup?.Invoke(data);
        }
        else
        {
            _currentHeldObject.DropItem();
            _currentHeldObject = _currentInteractable;

            WeaponDataSO data = _currentHeldObject.Interact();
            if (data)
                OnPickup?.Invoke(data);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _raduis);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _raduis, _interactiveLayer);

        Debug.Log(hits.Length);

        if (hits.Length > 0)
        {
            InteractableObject closestInteractable = null;

            for (int i = 0; i < hits.Length; i++)
            {
                float closestDistance = float.MaxValue;

                if(hits[i].TryGetComponent(out InteractableObject interactable))
                {
                    Debug.Log("Interactable: " + interactable.gameObject.name);

                    float distance = Vector3.Distance(transform.position, hits[i].transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            Debug.Log("Current: " + _currentInteractable);

            if(closestInteractable != _currentInteractable)
            {
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
