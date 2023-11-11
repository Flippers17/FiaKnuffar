using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 3;
    public UnityEvent OnDie;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
            OnDie?.Invoke();
    }

    public int GetHealth()
    {
        return health;
    }
}
