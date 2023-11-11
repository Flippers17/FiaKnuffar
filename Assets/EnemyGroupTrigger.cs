using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGroupTrigger : MonoBehaviour
{
    [SerializeField] private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    public UnityEvent allEnemiesFell;

    private void Start()
    {
        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.OnFall.AddListener(OnEnemieDied);
        }
    }

    private void OnEnemieDied(EnemyBehaviour enemy)
    {
        enemy.OnFall.RemoveListener(OnEnemieDied);

        enemies.Remove(enemy);

        if(enemies.Count <= 0)
        {
            allEnemiesFell?.Invoke();
        }
    }
}
