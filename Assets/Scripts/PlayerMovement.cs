using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundCheckRadius = .5f;
    [SerializeField]
    private LayerMask _groundLayers;
    
    [Space(10), SerializeField]
    private PlayerInputHandler _input;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private PlayerPush _playerPush;
    [SerializeField]
    private float _jumpVelocity = 10f;
    [SerializeField]
    private float _moveSpeed = 6f;
    [SerializeField]
    private float _gravity = 40f;

    public Vector3 velocity = Vector3.zero;


    public bool isGrounded = false;

    private Transform camTransform;
    private Vector3 facingDir = new Vector3(0, 0, 1);
    [SerializeField]
    private float _turnSpeed = 0.5f;


    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        _input.OnMove += HandleMovement;
    }

    private void OnDisable()
    {
        _input.OnMove -= HandleMovement;
    }


    private void Update()
    {
        if (isGrounded && _input.rememberJumpInput)
            Jump();
        
        HandleRotation();
        HandleVelocity();
    }


    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        
        HandleGravity();
    }

    private bool IsGrounded()
    {
        if(Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayers))
        {
            _anim.ResetTrigger("Jump");
            _anim.SetBool("Grounded", true);
            return true;
        }

        _anim.SetBool("Grounded", false);
        return false;
    }

    private void Jump()
    {
        _anim.SetTrigger("Jump");
        velocity.y = _jumpVelocity;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (_playerPush.GetPushState())
        {
            if (!_playerPush.GetFallState())
            {
                velocity.x = 0;
                velocity.z = 0;
                _anim.SetFloat("Speed", 0);
            }
            return;
        }

        moveInput = new Vector2(camTransform.forward.x, camTransform.forward.z) * moveInput.y + new Vector2(camTransform.right.x, camTransform.right.z) * moveInput.x;
        moveInput.Normalize();

        if (moveInput.magnitude != 0)
            facingDir = new Vector3(moveInput.x, 0, moveInput.y);
        
        velocity.x = moveInput.x * _moveSpeed;
        velocity.z = moveInput.y * _moveSpeed;
        _anim.SetFloat("Speed", _moveSpeed * moveInput.magnitude);
    }

    private void HandleRotation()
    {
        if (!_playerPush.GetPushState())
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facingDir, Vector3.up), _turnSpeed);
    }

    private void HandleGravity()
    {
        if(!isGrounded)
           velocity.y -= _gravity * Time.fixedDeltaTime;
        else
            velocity.y = -1;
    }

    private void HandleVelocity()
    {
        if(_controller.enabled)
            _controller.Move(velocity * Time.deltaTime);
    }
}
