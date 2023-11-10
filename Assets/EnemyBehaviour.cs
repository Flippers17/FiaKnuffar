using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform _fallPoint;
    [SerializeField]
    private Rigidbody _rb;

    private Vector3 _fallPosition;
    private bool _pushed = false;

    public int defense = 1;
    public float quickTimeSpeed = 50;

    public UnityEvent OnFall;
    

    // Start is called before the first frame update
    void Start()
    {
        _fallPosition = _fallPoint.position;
        _rb.isKinematic = true;
    }


    public void GetPushed(Vector3 pushVelocity)
    {
        if (_pushed)
            return;

        _pushed = true;
        StartCoroutine(GettingPushed(pushVelocity));

    }

    IEnumerator GettingPushed(Vector3 pushVelocity)
    {
        float moveSpeed = pushVelocity.magnitude;

        Vector3 moveDir = _fallPosition - transform.position;
        moveDir.Normalize();

        while (Vector3.Distance(transform.position, _fallPosition) > 0.1f)
        {
            Vector3 moveDif = moveSpeed * moveDir * Time.deltaTime;

            if (Vector3.Distance(transform.position, _fallPosition) < moveDif.magnitude)
            {
                transform.position = _fallPosition;
                break;
            }

            transform.position += moveDif;
            yield return null;
        }

        pushVelocity = transform.forward * pushVelocity.z + pushVelocity.x * transform.right + transform.up * pushVelocity.y;

        _rb.velocity = pushVelocity;
        _rb.isKinematic = false;
        OnFall?.Invoke();
    }
}
