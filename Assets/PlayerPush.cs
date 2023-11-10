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
    [SerializeField]
    private FloatEventPort _updateQuickTimeValueEvent;
    [SerializeField]
    private TwoIntEventPort _setGreenZoneEvent;

    [SerializeField]
    private GameObject _quickTimeEventUI;

    [Range(1, 100)]
    private float quickTimeValue = 1;
    private int quickTimeDirection = 1;
    private float quickTimeSpeed = 50;
    private (int, int) _currentGreenZone = (40, 60);
    private EnemyBehaviour _currentEnemy;
    private bool doingPush = false;


    private void OnEnable()
    {
        _input.OnPush += TryPush;
    }

    private void OnDisable()
    {
        _input.OnPush -= TryPush;
    }

    private void Update()
    {
        if (doingPush)
        {
            quickTimeValue += quickTimeDirection *Time.deltaTime * quickTimeSpeed;
            _updateQuickTimeValueEvent.Invoke(quickTimeValue);
            if (quickTimeValue <= 1 || quickTimeValue >= 100)
                quickTimeDirection *= -1;
        }
    }


    private void TryPush()
    {
        if (!doingPush)
        {
            Collider[] enemies = Physics.OverlapBox(_pushPoint.position, halfBoxSize, Quaternion.identity, _enemyLayers);
            if (enemies.Length == 0 || enemies == null)
                return;

            InitiatePush(enemies[0].gameObject.GetComponent<EnemyBehaviour>());
        }
        else
        {
            if (quickTimeValue > _currentGreenZone.Item1 && quickTimeValue < _currentGreenZone.Item2)
                FinishPush(_currentEnemy, new Vector3(0, 3, 10));
            else
                FailPush();

        }
    }

    private void InitiatePush(EnemyBehaviour enemy)
    {
        _quickTimeEventUI.SetActive(true);
        quickTimeSpeed = enemy.quickTimeSpeed;
        doingPush = true;
        quickTimeValue = 1;
        _setGreenZoneEvent.Invoke(_currentGreenZone.Item1, _currentGreenZone.Item2);
        _currentEnemy = enemy;
    }


    private void FinishPush(EnemyBehaviour enemy, Vector3 pushVelocity)
    {
        _quickTimeEventUI.SetActive(false);
        enemy.GetPushed(pushVelocity);
        doingPush = false;
    }

    private void FailPush()
    {
        _quickTimeEventUI.SetActive(false);
        doingPush = false;
    }
}
