using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform _fallPoint;
    [SerializeField]
    private Rigidbody _rb;

    private Vector3 _fallPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        _fallPosition = _fallPoint.position;
        _rb.isKinematic = true;
    }


    public void GetPushed(Vector2 pushVelocity)
    {
        StartCoroutine(GettingPushed(pushVelocity));

    }

    IEnumerator GettingPushed(Vector2 pushVelocity)
    {
        float moveSpeed = pushVelocity.magnitude;

        Vector3 moveDir = _fallPosition - transform.position;
        moveDir.Normalize();

        while (Vector3.Distance(transform.position, _fallPosition) < 0.1f)
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

        _rb.velocity = new Vector3(pushVelocity.x, 2, pushVelocity.y);
        _rb.isKinematic = false;
    }
}
