using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 3;
    [SerializeField] private Animator _animator;
    public UnityEvent OnDie;
    public UnityEvent OnDamageTaken;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamageTaken?.Invoke();

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
}
