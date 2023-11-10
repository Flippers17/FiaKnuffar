using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;

    public UnityAction<Vector2> OnMove;

    public bool rememberJumpInput = false;
    [SerializeField]
    private float _jumpRememberTime = .4f;
    private float _timeSincePressedJump = 1f;

    private void OnEnable()
    {
        _input.actions["Move"].performed += OnMoveInput;
        _input.actions["Jump"].performed += OnJumpInput;
    }

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }
    
    private void OnJumpInput(InputAction.CallbackContext ctx)
    {
        _timeSincePressedJump = 0;
        rememberJumpInput = true;
    }

    private void Update()
    {
        if (_timeSincePressedJump < _jumpRememberTime)
            _timeSincePressedJump += Time.deltaTime;
        else if (rememberJumpInput)
            rememberJumpInput = false;
    }

}
