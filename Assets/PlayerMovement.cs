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


    public bool isGrounded = false;

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayers);
    }
}
