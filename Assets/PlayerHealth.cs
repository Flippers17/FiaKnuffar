using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 3;
    [SerializeField] private Animator _animator;
    [SerializeField]
    private IntEventPort _updateHealthEvent;

    public UnityEvent OnDie;
    public UnityEvent OnDamageTaken;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamageTaken?.Invoke();
        _updateHealthEvent.Invoke(health);
        if(health <= 0)
        {
            OnDie?.Invoke();
            _animator.SetTrigger("Falling");
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = 3;
        _updateHealthEvent.Invoke(health);
    }
}
