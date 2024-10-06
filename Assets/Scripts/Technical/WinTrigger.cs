using UnityEngine;
using UnityEngine.Events;

public class WinTrigger : MonoBehaviour
{
    public UnityEvent OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement movement))
        {
            OnTriggered?.Invoke();
            movement.enabled = false;
        }
    }
}
