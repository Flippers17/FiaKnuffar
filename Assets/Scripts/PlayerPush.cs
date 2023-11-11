using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private float fallSpeed = 8f;

    [Space(20), SerializeField]
    private PlayerInputHandler _input;
    [SerializeField]
    private GameObject _quickTimeEventUI;
    [SerializeField]
    private PlayerCameraPosition camPos;
    [SerializeField]
    private PlayerHealth _health;

    
    [Space(10), SerializeField]
    private FloatEventPort _updateQuickTimeValueEvent;
    [SerializeField]
    private TwoIntEventPort _setGreenZoneEvent;


    [Range(1, 100)]
    private float quickTimeValue = 1;
    private int quickTimeDirection = 1;
    private float quickTimeSpeed = 50;
    private (int, int) _currentGreenZone = (40, 60);
    private EnemyBehaviour _currentEnemy;
    private bool doingPush = false;
    private bool _falling = false;


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
        if (doingPush && !_falling)
        {
            quickTimeValue += quickTimeDirection *Time.deltaTime * quickTimeSpeed;
            _updateQuickTimeValueEvent.Invoke(quickTimeValue);
            if (quickTimeValue <= 1 || quickTimeValue >= 100)
                quickTimeDirection *= -1;
        }
    }


    private void TryPush()
    {
        if (_falling)
            return;

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
                FinishPush(_currentEnemy, new Vector3(0, 3, 4));
            else
                FailPush();

        }
    }

    private void InitiatePush(EnemyBehaviour enemy)
    {
        camPos.SetZoom(true);
        _quickTimeEventUI.SetActive(true);
        quickTimeSpeed = enemy.quickTimeSpeed;
        _currentGreenZone.Item1 = enemy.greenZone.x;
        _currentGreenZone.Item2 = enemy.greenZone.y;
        doingPush = true;
        quickTimeValue = 1;
        _setGreenZoneEvent.Invoke(_currentGreenZone.Item1, _currentGreenZone.Item2);
        _currentEnemy = enemy;
    }


    private void FinishPush(EnemyBehaviour enemy, Vector3 pushVelocity)
    {
        _quickTimeEventUI.SetActive(false);
        enemy.GetPushed(pushVelocity);
        StartCoroutine(PushBeingFinnished());
        
    }

    IEnumerator PushBeingFinnished()
    {
        yield return new WaitForSeconds(1);
        camPos.SetZoom(false);
        doingPush = false;
    }

    private void FailPush()
    {
        _health.TakeDamage(1);
        //camPos.SetZoom(false);
        //_quickTimeEventUI.SetActive(false);
        //doingPush = false;
    }

    public bool GetPushState()
    {
        return doingPush;
    }


    public void Fall()
    {
        camPos._camFollow.enabled = false;
        _quickTimeEventUI.SetActive(false);
        _falling = true;
        GetComponent<CharacterController>().enabled = false;
        StartCoroutine(Falling(_currentEnemy._fallPosition));
    }

    private IEnumerator Falling(Vector3 fallPos)
    {
        Vector3 moveDir = fallPos - transform.position;
        moveDir.Normalize();

        while (Vector3.Distance(transform.position, fallPos) > 0.1f)
        {
            Vector3 moveDif = fallSpeed * moveDir * Time.deltaTime;

            if (Vector3.Distance(transform.position, fallPos) < moveDif.magnitude)
            {
                transform.position = fallPos;
                break;
            }

            transform.position += moveDif;
            yield return null;
        }
        GetComponent<CharacterController>().enabled = true;
        GetComponent<PlayerMovement>().velocity = transform.forward * fallSpeed + transform.up * 1;
    }
}
