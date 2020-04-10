using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private Door _activeDoor;
    private KeyColor _activeDoorColor;
    private Documents _activeDocument;
    public float MovementSpeedDefault { get; private set; }

    public float MovementSpeedSprint { get; private set; }

    private float MovementSpeedCrawl { get; set; }

    // INPUT HANDLER
    private PlayerInputActions _inputActions;
    private Vector2 _movementInput;

    private PlayerInteractionController _playerInteractionController;

    [FormerlySerializedAs("_movementSpeed")]
    public float movementSpeed;

    public Vector2 MoveVelocity { get; private set; }
    private Rigidbody2D _rigidbody2D;
    private static readonly int Speed = Animator.StringToHash("Speed");


    private void Awake()
    {
        _playerInteractionController = GetComponent<PlayerInteractionController>();
        _inputActions = new PlayerInputActions();
        _inputActions.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputActions.PlayerControls.Sprint.performed += ctx => Sprint(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Crawl.performed += ctx => Crouch(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Interact.performed += ctx => _playerInteractionController.Interact();
        MovementSpeedDefault = 5f;
        MovementSpeedSprint = 8f;
        MovementSpeedCrawl = 2f;
    }


    private void Crouch(float v)
    {
        movementSpeed = v > 0 ? MovementSpeedCrawl : MovementSpeedDefault;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>();
        movementSpeed = MovementSpeedDefault;
    }

    private void FixedUpdate()
    {
        if (!_playerInteractionController.IsReading)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + MoveVelocity * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        MoveVelocity = _movementInput.normalized * movementSpeed;
        anim.SetFloat(Speed, MoveVelocity.magnitude);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void Sprint(float b)
    {
        movementSpeed = b > 0 ? MovementSpeedSprint : MovementSpeedDefault;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}