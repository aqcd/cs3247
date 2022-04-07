using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif

public class MultiplayerThirdPersonController : NetworkBehaviour {
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 10.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private Animator _animator;
    private CharacterController _controller;
    private MultiplayerInputs _input;
    private GameObject _mainCamera;
    private GameObject _slider;

    public Material transparentMaterial;
    public Material opaqueMaterial;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;

    private Health health;

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    
    public override void OnStartLocalPlayer()
    {
        CinemachineVirtualCamera vCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        Transform cameraRootTransform = transform.GetChild(0).transform;
        vCam.Follow = cameraRootTransform;
        vCam.LookAt = cameraRootTransform;

        GameObject canvas = GameObject.Find("UI_Canvas");
        canvas.GetComponent<Canvas>().enabled = true;
        // Link UI Inputs to Character
        MultiplayerUICanvasControllerInput canvasController = canvas.GetComponent<MultiplayerUICanvasControllerInput>();
        canvasController.AttachMultiplayerInputs(gameObject.GetComponent<MultiplayerInputs>());

        Canvas skillCanvas = gameObject.transform.GetChild(5).gameObject.GetComponent<Canvas>();
        canvas.transform.GetChild(1).gameObject.GetComponent<SkillJoystickController>().AttachSkillCanvas(skillCanvas);
        canvas.transform.GetChild(2).gameObject.GetComponent<SkillJoystickController>().AttachSkillCanvas(skillCanvas);
        canvas.transform.GetChild(3).gameObject.GetComponent<SkillJoystickController>().AttachSkillCanvas(skillCanvas);
        canvas.transform.GetChild(4).gameObject.GetComponent<SkillJoystickController>().AttachSkillCanvas(skillCanvas);
        
        MultiplayerMobileDisableAutoSwitchControls disableSwitch = canvas.GetComponent<MultiplayerMobileDisableAutoSwitchControls>();
        // disableSwitch.AttachPlayerInput(gameObject.GetComponent<PlayerInput>());

        Loadout loadout = GameManager.instance.loadout;
        this.MoveSpeed = loadout.GetLoadoutStats().GetAttributeValue(Attribute.MS);

        // enable local audiolistener 
        GetComponent<AudioListener>().enabled = true;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<MultiplayerInputs>();
        _slider = _controller.GetComponentInChildren<HealthBar>().healthBarSlider.gameObject;

        health = GetComponent<Health>();

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        _animator.SetBool("BasicAttack", false);
        _animator.SetBool("isHeal", false);
        _animator.SetBool("isDead", false);
        _animator.SetFloat("ASModifier", 1.0f);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _hasAnimator = TryGetComponent(out _animator);

        JumpAndGravity();
        GroundedCheck();
        Move();

        if (health.currentHealth <= 0) {
            _animator.SetBool("isDead", true);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    // Event called by punch animation clop to set boolean back to false.
    public void SetAttackFalse() {
        _animator.SetBool("BasicAttack", false);
    }

    // Event called by casting animation clop to set boolean back to false.
    public void SetHealingFalse() {
        _animator.SetBool("isHeal", false);
    }

    public void SetDeathFalse() {
        _animator.SetBool("isDead", false);
    }

    public void PlayDeathAnimation() {
        _animator.SetBool("isDead", true);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            //_animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void Move()
    {
        if (!gameObject.GetComponent<PlayerManager>().GetCanMove()) {
            return;
        }
        // set target speed based on move speed and movement buffs
        float targetSpeed = MoveSpeed + gameObject.GetComponent<PlayerManager>().GetAttributeBuff(Attribute.MS);
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.GetChild(2).eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.GetChild(2).rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (_hasAnimator)
            {
                //_animator.SetBool(_animIDJump, false);
                //_animator.SetBool(_animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                if (_hasAnimator)
                {
                    //_animator.SetBool(_animIDJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    //_animator.SetBool(_animIDFreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            _input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;
        
        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }

    // Transcluent to self and completely invisible to others 
    private void SetPlayerInvisible() {
        if (this.isLocalPlayer) {
            SetSelfInvisible();
        } else {
            float distance = Vector3.Distance(MatchManager.instance.GetPlayer().transform.position, transform.position);
            if (distance < 3.0f) {
                SetSelfInvisible();
                foreach (SkinnedMeshRenderer s in _controller.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                    s.enabled = true;
                }
                _slider.SetActive(true);
                this.GetComponentInChildren<MeshRenderer>().enabled = true; // Minimap
            } else {
                foreach (SkinnedMeshRenderer s in _controller.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                    s.enabled = false;
                }
                _slider.SetActive(false);
                this.GetComponentInChildren<MeshRenderer>().enabled = false; // Minimap
            }

        }
    }

    private void SetSelfInvisible() {
        this.GetComponentInChildren<SkinnedMeshRenderer>().material = transparentMaterial;
        Color color = transparentMaterial.color;
        color.a = 0.8f;
        this.GetComponentInChildren<SkinnedMeshRenderer>().material.color = color;

        Color barColor = _slider.GetComponent<Image>().color;
        barColor.a = 0.2f;
        _slider.GetComponent<Image>().color = barColor;
    }

    // Fully visible
    private void SetPlayerVisible() {
        this.GetComponentInChildren<SkinnedMeshRenderer>().material = opaqueMaterial;

        Color barColor = _slider.GetComponent<Image>().color;
        barColor.a = 1.0f;
        _slider.GetComponent<Image>().color = barColor;

        foreach (SkinnedMeshRenderer s in _controller.GetComponentsInChildren<SkinnedMeshRenderer>()) {
            s.enabled = true;
        }
        _slider.SetActive(true);

        this.GetComponentInChildren<MeshRenderer>().enabled = true; // Minimap
    }

    private void Heal() {
        if (this.isLocalPlayer) {
            _controller.GetComponent<Health>().TakeHealing(20);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Brush") {
            SetPlayerInvisible();
        } 
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.tag == "Brush") {
            SetPlayerInvisible();
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag == "Brush") {
            SetPlayerVisible();
        } 
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }
}
