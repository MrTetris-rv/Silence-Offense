using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour, IControllable
{
    [Header("Move")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float sprintSpeed = 5.3f;
    private Vector3 _moveDirection;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float groundedRadius = 0.28f;
    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpTimeout = 0.50f;
    [SerializeField] private float fallTimeout = 0.15f;
    private float _velocity;
    private float _terminalVelocity = 53.0f;
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private Animator _animator;
    //private int _animIDSpeed;
    //private int _animIDGrounded;
    private int _animIDJump;
    //private int _animIDFreeFall;
    //private int _animIDMotionSpeed;
    private bool _hasAnimator;

    private CharacterController _characterController;
    private IControllable _controllable;
    private Transform _mainCamera;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main.transform;
        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        MoveInternal();
        Jump(false);
    }
    public void Jump(bool isJumping)
    {
        if (isGrounded)
        {
            _fallTimeoutDelta = fallTimeout;

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                //_animator.SetBool(_animIDFreeFall, false);
            }

            if (_velocity < 0.0f)
            {
                _velocity = -2f;
            }

            if (isJumping && _jumpTimeoutDelta <= 0.0f)
            {
                _velocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.fixedDeltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = jumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.fixedDeltaTime;
            }
            //else
            //{
            //    if (_hasAnimator)
            //    {
            //        _animator.SetBool(_animIDFreeFall, true);
            //    }
            //}

            isJumping = false;
        }

        if (_velocity < _terminalVelocity)
        {
            _velocity += gravity * Time.deltaTime;
        }
    }

    public void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }

    public void Sprint()
    {
        throw new System.NotImplementedException();
    }

    private void MoveInternal()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(_mainCamera.forward, Vector3.up).normalized;
        Vector3 cameraRight = _mainCamera.right.normalized;

        Vector3 movementDirection = cameraRight * _moveDirection.x + cameraForward * _moveDirection.z;
        _characterController.Move(movementDirection * speed * Time.fixedDeltaTime +
            new Vector3(0.0f, _velocity, 0.0f) * Time.fixedDeltaTime);
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y -
            groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
            QueryTriggerInteraction.Ignore);

        //if (_hasAnimator)
        //{
        //    _animator.SetBool(_animIDGrounded, isGrounded);
        //}
    }

    public void Fire()
    {
        Debug.DrawLine(transform.position, transform.forward, Color.red, 10f);
        Debug.Log("Fire");
    }

    private void AssignAnimationIDs()
    {
        //_animIDSpeed = Animator.StringToHash("Speed");
        //_animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("isJumping");
        //_animIDFreeFall = Animator.StringToHash("FreeFall");
        //_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnLand(AnimationEvent animationEvent)
    {

    }
}
   