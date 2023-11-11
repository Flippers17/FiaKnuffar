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
    private float mashSpeed = 10;
    private (int, int) _currentGreenZone = (40, 60);
    private QuickTimeType _currentQuickTimeType;

    private EnemyBehaviour _currentEnemy;
    private bool doingPush = false;
    private bool finishingPush = false;
    private bool _falling = false;

    private int quickTimeEventsLeft = 0;

    [SerializeField]
    private float inputDelay = 0.3f;
    private float timeSinceSwitched = 0;


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
        if (timeSinceSwitched < inputDelay)
            timeSinceSwitched += Time.deltaTime;

        if (doingPush && !_falling)
        {

            if (_currentQuickTimeType == QuickTimeType.timing)
                UpdateTiming();
            else if (_currentQuickTimeType == QuickTimeType.mashing)
                UpdateMashing();
        }
    }


    private void TryPush()
    {
        if (_falling || timeSinceSwitched < inputDelay)
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
            if(_currentQuickTimeType == QuickTimeType.timing)
            {
                if (quickTimeValue > _currentGreenZone.Item1 && quickTimeValue < _currentGreenZone.Item2)
                    FinishPush();
                else
                    FailPush();
            }
            else if(_currentQuickTimeType == QuickTimeType.mashing)
            {
                quickTimeValue += mashSpeed;
                _updateQuickTimeValueEvent.Invoke(quickTimeValue);
                if(quickTimeValue >= 100)
                    FinishPush();
            }

        }
    }

    private void InitiatePush(EnemyBehaviour enemy)
    {
        camPos.SetZoom(true);
        _quickTimeEventUI.SetActive(true);
        _health.ResetHealth();

        finishingPush = false;
        doingPush = true;
        _currentEnemy = enemy;

        CharacterController character = GetComponent<CharacterController>();
        character.enabled = false;
        timeSinceSwitched = 10;

        Vector3 playerPos = _currentEnemy.transform.position - _currentEnemy.transform.forward;
        transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        transform.rotation = Quaternion.LookRotation(_currentEnemy.transform.forward, Vector3.up);

        character.enabled = true;

        SetQuickTimeStats(enemy.quickTimeEvents[0]);
        quickTimeEventsLeft = enemy.quickTimeEvents.Count;
        
    }


    private void FinishPush()
    {
        if (finishingPush)
            return;

        if(quickTimeEventsLeft > 1)
        {
            timeSinceSwitched = 0;
            quickTimeEventsLeft--;
            quickTimeValue = 1;
            SetQuickTimeStats(_currentEnemy.quickTimeEvents[_currentEnemy.quickTimeEvents.Count - quickTimeEventsLeft]);
            //Play audio queue
            return;
        }

        finishingPush = true;

        _anim.SetTrigger("Push");
        _quickTimeEventUI.SetActive(false);
        StartCoroutine(PushBeingFinnished());
        
    }

    private void SetQuickTimeStats(QuickTimeEvent qtE)
    {
        _quickTimeEventUI.GetComponent<PushQuickTimeEventUI>().quickTimeType = qtE.type;
        quickTimeSpeed = qtE.quickTimeSpeed;
        _currentGreenZone.Item1 = qtE.greenZone.x;
        _currentGreenZone.Item2 = qtE.greenZone.y;

        if (_interactions.currentWeaponData != null)
        {
            float greenZoneDif = (_interactions.currentWeaponData.greenZoneMultiplier * (_currentGreenZone.Item2 - _currentGreenZone.Item1)) - (_currentGreenZone.Item2 - _currentGreenZone.Item1);

            _currentGreenZone.Item1 -= Mathf.FloorToInt(greenZoneDif / 2);
            _currentGreenZone.Item2 += Mathf.FloorToInt(greenZoneDif / 2);

            quickTimeSpeed *= _interactions.currentWeaponData.quickTimeSpeedMultiplier;
        }

        _currentGreenZone.Item1 = Mathf.Max(0, _currentGreenZone.Item1);
        _currentGreenZone.Item2 = Mathf.Min(100, _currentGreenZone.Item2);
        _currentQuickTimeType = qtE.type;
        
        _setGreenZoneEvent.Invoke(_currentGreenZone.Item1, _currentGreenZone.Item2);

        if (_currentQuickTimeType == QuickTimeType.mashing)
            quickTimeValue = 20;
        else if (_currentQuickTimeType == QuickTimeType.timing)
            quickTimeValue = 1;
    }


    //Plays when push event happens
    public void PushEnemy()
    {
        Vector3 newPushVelocity = _pushVelocity;

        if (_interactions.currentWeaponData != null)
        {
            newPushVelocity *= _interactions.currentWeaponData.pushVelocity;
            _interactions.ReduceCurrentWeaponDurabillity();
        }
        CameraShake.TriggerShake(.1f, .05f, .9f);
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
        if (_falling)
            return;

        camPos.SetCameraFollow(false);
        _quickTimeEventUI.SetActive(false);
        GetComponent<CharacterController>().enabled = false;
        if(_currentEnemy && !_falling)
            StartCoroutine(Falling(_currentEnemy._fallPosition));
        else if(!_falling)
            StartCoroutine(Falling(transform.position));

        _falling = true;
    }

    private IEnumerator Falling(Vector3 fallPos)
    {
        Vector3 moveDir = fallPos - transform.position;
        moveDir.Normalize();
        
        yield return new WaitForSeconds(.6f);

        if(_currentEnemy)
            _currentEnemy.GetPushed(new Vector3(0,0,fallSpeed));

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


    private void UpdateTiming()
    {
        quickTimeValue += quickTimeDirection * Time.deltaTime * quickTimeSpeed;
        _updateQuickTimeValueEvent.Invoke(quickTimeValue);
        if (quickTimeValue <= 1)
        {
            quickTimeDirection *= -1;
            quickTimeValue = 1;
        }
        else if (quickTimeValue >= 100)
        {
            quickTimeDirection *= -1;
            quickTimeValue = 100;
        }
    }

    private void UpdateMashing()
    {
        quickTimeValue -= Time.deltaTime * quickTimeSpeed;
        _updateQuickTimeValueEvent.Invoke(quickTimeValue);
        if (quickTimeValue <= 1)
        {
            FailPush();
            quickTimeValue = 20;
        }
    }
}
