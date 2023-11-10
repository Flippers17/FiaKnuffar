using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField][Min(0.1f)][Tooltip("Range for interactive items!")] private float _raduis = 1;
    [SerializeField] private LayerMask _interactiveLayer;
    [SerializeField] private PlayerInput _input;

    public UnityEvent<IInteractable> OnInteractableObjectFound; 

    private IInteractable _currentInteractable;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Interact()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _raduis);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _raduis, _interactiveLayer);
        if(hits.Length > 0)
        {
            IInteractable closestInteractable = null;

            for (int i = 0; i < hits.Length; i++)
            {
                float closestDistance = float.MaxValue;

                if(hits[i].TryGetComponent(out IInteractable interactable))
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
                _currentInteractable.OutOfRange();
                _currentInteractable = closestInteractable;
                OnInteractableObjectFound?.Invoke(_currentInteractable);
                _currentInteractable.InRange();
            }
        }
        else
        {
            if(_currentInteractable != null)
            {
                _currentInteractable.OutOfRange();
                _currentInteractable = null;
            }
        }
    }
}
