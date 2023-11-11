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
    [SerializeField] private Vector3 _pushVelocity = new Vector3(0, 3, 4);

    [Space(20), SerializeField]
    private PlayerInputHandler _input;
    [SerializeField]
    private PlayerInteractions _interactions;
    [SerializeField]
    private GameObject _quickTimeEventUI;
    [SerializeField]
    private PlayerCamera camPos;
    [SerializeField]
    private PlayerHealth _health;
    [SerializeField]
    private Animator _anim;

    
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
                FinishPush();
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

        if (_interactions.currentWeaponData != null)
        {
            float greenZoneDif =  (_interactions.currentWeaponData.greenZoneMultiplier * (_currentGreenZone.Item2 - _currentGreenZone.Item1)) - (_currentGreenZone.Item2 - _currentGreenZone.Item1);

            _currentGreenZone.Item1 -= Mathf.FloorToInt(greenZoneDif/2);
            _currentGreenZone.Item2 += Mathf.FloorToInt(greenZoneDif/2);

            quickTimeSpeed *= _interactions.currentWeaponData.quickTimeSpeedMultiplier;
        }

        _currentGreenZone.Item1 = Mathf.Max(0, _currentGreenZone.Item1);
        _currentGreenZone.Item2 = Mathf.Min(100, _currentGreenZone.Item2);

        doingPush = true;
        quickTimeValue = 1;
        _setGreenZoneEvent.Invoke(_currentGreenZone.Item1, _currentGreenZone.Item2);
        _currentEnemy = enemy;
    }


    private void FinishPush()
    {
        _anim.SetTrigger("Push");
        _quickTimeEventUI.SetActive(false);
        StartCoroutine(PushBeingFinnished());
        
    }

    //Plays when push event happens
    public void PushEnemy()
    {
        Vector3 newPushVelocity = _pushVelocity;

        if (_interactions.currentWeaponData != null)
            newPushVelocity *= _interactions.currentWeaponData.pushVelocity;

        _currentEnemy.GetPushed(newPushVelocity);
    }

    IEnumerator PushBeingFinnished()
    {
        yield return new WaitForSeconds(1.5f);
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
    
    
    public bool GetFallState()
    {
        return _falling;
    }


    public void Fall()
    {
        camPos.SetCameraFollow(false);
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
