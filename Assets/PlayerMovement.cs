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
    private float _jumpVelocity = 10f;
    [SerializeField]
    private float _moveSpeed = 6f;
    [SerializeField]
    private float _gravity = 40f;

    private Vector3 _velocity = Vector3.zero;


    public bool isGrounded = false;

    private Transform camTransform;

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
    }


    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        HandleGravity();
        HandleVelocity();
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayers);
    }

    private void Jump()
    {
        _velocity.y = _jumpVelocity;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        moveInput = new Vector2(camTransform.forward.x, camTransform.forward.z) * moveInput.y + new Vector2(camTransform.right.x, camTransform.right.z) * moveInput.x;
        moveInput.Normalize();
        _velocity.x = moveInput.x * _moveSpeed;
        _velocity.z = moveInput.y * _moveSpeed;
    }

    private void HandleGravity()
    {
        _velocity.y -= _gravity * Time.fixedDeltaTime;
    }

    private void HandleVelocity()
    {
        _controller.Move(_velocity * Time.fixedDeltaTime);
    }
}
