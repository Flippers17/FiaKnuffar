using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField]
    private Transform _pushPoint;

    [SerializeField]
    private LayerMask _enemyLayers;
    
    [SerializeField]
    private Vector3 halfBoxSize = new Vector3 (1f, 1f, 1f);

    [SerializeField]
    private PlayerInputHandler _input;

    private void OnEnable()
    {
        _input.OnPush += TryPush;
    }

    private void OnDisable()
    {
        _input.OnPush -= TryPush;
    }


    private void TryPush()
    {
        Collider[] enemies = Physics.OverlapBox(_pushPoint.position, halfBoxSize, Quaternion.identity, _enemyLayers);
        if (enemies.Length == 0 || enemies == null)
            return;

        InitiatePush(enemies[0].gameObject.GetComponent<EnemyBehaviour>(), new Vector3(0, 3, 10));
    }

    private void InitiatePush(EnemyBehaviour enemy, Vector3 pushVelocity)
    {
        enemy.GetPushed(pushVelocity);
    }
}
