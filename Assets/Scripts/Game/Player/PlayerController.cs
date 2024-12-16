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

    [Header("Animator")]
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
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
                _animator.SetBool(_animIDFreeFall, false);
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
            else
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

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

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, isGrounded);
        }
    }

    public void Fire()
    {
        Debug.DrawLine(transform.position, transform.forward, Color.red, 10f);
        Debug.Log("Fire");
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnLand(AnimationEvent animationEvent)
    {

    }

    //    [Header("Player")]
    //    [Tooltip("Move speed of the character in m/s")]
    //    public float MoveSpeed = 2.0f;

    //    [Tooltip("Sprint speed of the character in m/s")]
    //    public float SprintSpeed = 5.335f;

    //    [Tooltip("How fast the character turns to face movement direction")]
    //    [Range(0.0f, 0.3f)]
    //    public float RotationSmoothTime = 0.12f;

    //    [Tooltip("Acceleration and deceleration")]
    //    public float SpeedChangeRate = 10.0f;

    //    [Space(10)]
    //    [Tooltip("The height the player can jump")]
    //    public float JumpHeight = 1.2f;

    //    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    //    public float Gravity = -15.0f;

    //    [Space(10)]
    //    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    //    public float JumpTimeout = 0.50f;

    //    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    //    public float FallTimeout = 0.15f;

    //    [Header("Player Grounded")]
    //    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    //    public bool Grounded = true;

    //    [Tooltip("Useful for rough ground")]
    //    public float GroundedOffset = -0.14f;

    //    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    //    public float GroundedRadius = 0.28f;

    //    [Tooltip("What layers the character uses as ground")]
    //    public LayerMask GroundLayers;

    //    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //    public float CameraAngleOverride = 0.0f;

    //    [Tooltip("For locking the camera position on all axis")]
    //    public bool LockCameraPosition = false;

    //    [Tooltip("How far in degrees can you move the camera up")]
    //    public float TopClamp = 70.0f;

    //    [Tooltip("How far in degrees can you move the camera down")]
    //    public float BottomClamp = -30.0f;

    //    [Header("Cinemachine")]
    //    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    //    public GameObject CinemachineCameraTarget;

    //    // cinemachine
    //    private float _cinemachineTargetYaw;
    //    private float _cinemachineTargetPitch;

    //    private float _speed;
    //    private float _animationBlend;
    //    private float _targetRotation = 0.0f;
    //    private float _rotationVelocity;
    //    private float _verticalVelocity;
    //    private float _terminalVelocity = 53.0f;

    //    // timeout deltatime
    //    private float _jumpTimeoutDelta;
    //    private float _fallTimeoutDelta;

    //    // animation IDs
    //    private int _animIDSpeed;
    //    private int _animIDGrounded;
    //    private int _animIDJump;
    //    private int _animIDFreeFall;
    //    private int _animIDMotionSpeed;

    //#if ENABLE_INPUT_SYSTEM
    //    private PlayerInput _playerInput;
    //#endif
    //    private Animator _animator;
    //    private CharacterController _controller;
    //    private InputSettings _input;

    //    [SerializeField]private GameObject _mainCamera;

    //    private const float _threshold = 0.01f;

    //    private bool _hasAnimator;


    //    public AudioClip LandingAudioClip;
    //    public AudioClip[] FootstepAudioClips;
    //    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    //    //private void Awake()
    //    //{
    //    //    // get a reference to our main camera
    //    //    if (_mainCamera == null)
    //    //    {
    //    //        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    //    //    }
    //    //}
    //    private void Start()
    //    {
    //        _hasAnimator = TryGetComponent(out _animator);
    //        _controller = GetComponent<CharacterController>();
    //        _input = GetComponent<InputSettings>();
    //#if ENABLE_INPUT_SYSTEM
    //        _playerInput = GetComponent<PlayerInput>();
    //#else
    //			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
    //#endif

    //        AssignAnimationIDs();

    //        // reset our timeouts on start
    //        _jumpTimeoutDelta = JumpTimeout;
    //        _fallTimeoutDelta = FallTimeout;
    //    }
    //    private void Update()
    //    {
    //        _hasAnimator = TryGetComponent(out _animator);

    //        JumpAndGravity();
    //        GroundedCheck();
    //        Move();
    //    }

    //    //private void LateUpdate()
    //    //{
    //    //    CameraRotation();
    //    //}

    //    private void AssignAnimationIDs()
    //    {
    //        _animIDSpeed = Animator.StringToHash("Speed");
    //        _animIDGrounded = Animator.StringToHash("Grounded");
    //        _animIDJump = Animator.StringToHash("Jump");
    //        _animIDFreeFall = Animator.StringToHash("FreeFall");
    //        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    //    }

    //    private void GroundedCheck()
    //    {
    //        // set sphere position, with offset
    //        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
    //            transform.position.z);
    //        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
    //            QueryTriggerInteraction.Ignore);

    //        // update animator if using character
    //        if (_hasAnimator)
    //        {
    //            _animator.SetBool(_animIDGrounded, Grounded);
    //        }
    //    }
    //    //private void CameraRotation()
    //    //{
    //    //    // if there is an input and camera position is not fixed
    //    //    if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
    //    //    {
    //    //        //Don't multiply mouse input by Time.deltaTime;
    //    //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

    //    //        _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
    //    //        _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
    //    //    }

    //    //    // clamp our rotations so our values are limited 360 degrees
    //    //    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
    //    //    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

    //    //    // Cinemachine will follow this target
    //    //    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
    //    //        _cinemachineTargetYaw, 0.0f);
    //    //}

    //    private void Move()
    //    {
    //        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

    //        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

    //        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
    //        float speedOffset = 0.1f;
    //        float inputMagnitude = 1f;

    //        // accelerate or decelerate to target speed
    //        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
    //            currentHorizontalSpeed > targetSpeed + speedOffset)
    //        {
    //            // creates curved result rather than a linear one giving a more organic speed change
    //            // note T in Lerp is clamped, so we don't need to clamp our speed
    //            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
    //                Time.deltaTime * SpeedChangeRate);

    //            // round speed to 3 decimal places
    //            _speed = Mathf.Round(_speed * 1000f) / 1000f;
    //        }
    //        else
    //        {
    //            _speed = targetSpeed;
    //        }

    //        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
    //        if (_animationBlend < 0.01f) _animationBlend = 0f;

    //        // normalise input direction
    //        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

    //        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
    //        // if there is a move input rotate player when the player is moving
    //        if (_input.move != Vector2.zero)
    //        {
    //            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
    //                              _mainCamera.transform.eulerAngles.y;
    //            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
    //                RotationSmoothTime);

    //            // rotate to face input direction relative to camera position
    //            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    //        }


    //        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

    //        // move the player
    //        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
    //                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

    //        // update animator if using character
    //        if (_hasAnimator)
    //        {
    //            _animator.SetFloat(_animIDSpeed, _animationBlend);
    //            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    //        }
    //    }

    //    private void JumpAndGravity()
    //    {
    //        if (Grounded)
    //        {
    //            // reset the fall timeout timer
    //            _fallTimeoutDelta = FallTimeout;

    //            // update animator if using character
    //            if (_hasAnimator)
    //            {
    //                _animator.SetBool(_animIDJump, false);
    //                _animator.SetBool(_animIDFreeFall, false);
    //            }

    //            // stop our velocity dropping infinitely when grounded
    //            if (_verticalVelocity < 0.0f)
    //            {
    //                _verticalVelocity = -2f;
    //            }

    //            // Jump
    //            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
    //            {
    //                // the square root of H * -2 * G = how much velocity needed to reach desired height
    //                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

    //                // update animator if using character
    //                if (_hasAnimator)
    //                {
    //                    _animator.SetBool(_animIDJump, true);
    //                }
    //            }

    //            // jump timeout
    //            if (_jumpTimeoutDelta >= 0.0f)
    //            {
    //                _jumpTimeoutDelta -= Time.deltaTime;
    //            }
    //        }
    //        else
    //        {
    //            // reset the jump timeout timer
    //            _jumpTimeoutDelta = JumpTimeout;

    //            // fall timeout
    //            if (_fallTimeoutDelta >= 0.0f)
    //            {
    //                _fallTimeoutDelta -= Time.deltaTime;
    //            }
    //            else
    //            {
    //                // update animator if using character
    //                if (_hasAnimator)
    //                {
    //                    _animator.SetBool(_animIDFreeFall, true);
    //                }
    //            }

    //            // if we are not grounded, do not jump
    //            _input.jump = false;
    //        }

    //        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
    //        if (_verticalVelocity < _terminalVelocity)
    //        {
    //            _verticalVelocity += Gravity * Time.deltaTime;
    //        }
    //    }
    //    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    //    {
    //        if (lfAngle < -360f) lfAngle += 360f;
    //        if (lfAngle > 360f) lfAngle -= 360f;
    //        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    //    }

    //    private void OnDrawGizmosSelected()
    //    {
    //        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
    //        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

    //        if (Grounded) Gizmos.color = transparentGreen;
    //        else Gizmos.color = transparentRed;

    //        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
    //        Gizmos.DrawSphere(
    //            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
    //            GroundedRadius);
    //    }
    //    private void OnFootstep(AnimationEvent animationEvent)
    //    {
    //        if (animationEvent.animatorClipInfo.weight > 0.5f)
    //        {
    //            if (FootstepAudioClips.Length > 0)
    //            {
    //                var index = Random.Range(0, FootstepAudioClips.Length);
    //                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
    //            }
    //        }
    //    }
    //    private void OnLand(AnimationEvent animationEvent)
    //    {
    //        if (animationEvent.animatorClipInfo.weight > 0.5f)
    //        {
    //            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
    //        }
    //    }
}
