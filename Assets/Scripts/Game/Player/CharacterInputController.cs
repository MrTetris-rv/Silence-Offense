using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CharacterInputController : MonoBehaviourPunCallbacks
{
    private IControllable _controllable;
    private InputSettings _gameInput;
    private Vector2 moveInput;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            _gameInput = new InputSettings();
            _gameInput.Enable();
        }

        _controllable = GetComponent<IControllable>();
        if (_controllable == null) throw new Exception($"There is no IControllable component on the object: {gameObject.name}");
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            _gameInput.Player.Move.performed += ctx => Move(ctx);
            _gameInput.Player.Move.canceled += ctx => Stop();
        }
    }

    public override void OnEnable()
    {
        if (photonView.IsMine)
        {
            _gameInput.Player.Jump.performed += OnJumpPerformed;
            _gameInput.Player.Fire.performed += OnFirePerfomed;
            _gameInput.UI.OpenMenu.performed += OnOpenMenuPerformed;
        }
    }

    private void OnOpenMenuPerformed(InputAction.CallbackContext context)
    {
        //_gameInput.Player.Disable();
    }

    [PunRPC]
    private void Shoot()
    {
        _controllable.Fire();
    }

    private void OnFirePerfomed(InputAction.CallbackContext context)
    {
        photonView.RPC("Shoot", RpcTarget.All);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        _controllable.Jump(true);
    }

    public override void OnDisable()
    {
        if (photonView.IsMine)
        {
            _gameInput.Player.Jump.performed -= OnJumpPerformed;
            _gameInput.Player.Fire.performed -= OnFirePerfomed;
            _gameInput.Player.Move.performed -= ctx => Move(ctx);
            _gameInput.Player.Move.canceled -= ctx => Stop();

            _gameInput.Disable();
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 worldDirection = transform.TransformDirection(direction).normalized;
        _controllable.Move(worldDirection);
    }

    private void Stop()
    {
        moveInput = Vector2.zero;
        _controllable.Move(moveInput);
    }
}
