using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public bool CanHide { get; set; }
    private bool CanOpenDoor { get; set; }
    public bool CanRead { get; set; }
    public bool IsReading { get; set; }

    private Door _activeDoor;
    private KeyColor _activeDoorColor;
    private Documents _activeDocument;
    public float MovementSpeedDefault { get; private set; }

    public float MovementSpeedSprint { get; private set; }

    private float MovementSpeedCrawl { get; set; }

    // INPUT HANDLER
    private PlayerInputActions _inputActions;

    //INVENTORY
    private readonly bool[] _keys = new bool[Enum.GetValues(typeof(KeyColor)).Length];
    public GameObject[] slots;
    private Vector2 _movementInput;

    [FormerlySerializedAs("_movementSpeed")]
    public float movementSpeed;

    public Vector2 MoveVelocity { get; private set; }
    private Rigidbody2D _rigidbody2D;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private SpriteRenderer _spriteRenderer;
    private Camera _camera;


    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputActions.PlayerControls.Sprint.performed += ctx => Sprint(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Crawl.performed += ctx => Crouch(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Interact.performed += ctx => Interact();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        MovementSpeedDefault = 5f;
        MovementSpeedSprint = 8f;
        MovementSpeedCrawl = 2f;
        IsReading = false;
    }

    private void Interact()
    {
        if (CanHide)
        {
            Hide();
        }

        if (CanOpenDoor)
        {
            UseKey((int) _activeDoorColor, _activeDoor);
        }

        if (!CanRead) return;
        _activeDocument.Read();
        CanRead = false;
        IsReading = true;
    }


    private void Crouch(float v)
    {
        movementSpeed = v > 0 ? MovementSpeedCrawl : MovementSpeedDefault;
    }

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>();
        movementSpeed = MovementSpeedDefault;
    }

    private void FixedUpdate()
    {
        if (!IsReading)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + MoveVelocity * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        MoveVelocity = _movementInput.normalized * movementSpeed;
        _spriteRenderer.flipX = MousePosition().x < transform.position.x;

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

    public void PickUpKeyCard(int color)
    {
        _keys[color] = true;
        Debug.Log("Picked up the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color += new Color(0, 0, 0, 1);
    }

    private void UseKey(int color, Door pDoor)
    {
        _keys[color] = false;
        Debug.Log("Used the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color -= new Color(0, 0, 0, 1);
        pDoor.OpenDoor();
    }

    public bool HasKeyOfColor(KeyColor color)
    {
        return _keys[(int) color];
    }

    private void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        CanHide = false;
    }

    public void Reveal()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void GetActiveDoor(KeyColor color, Door pDoor)
    {
        _activeDoor = pDoor;
        _activeDoorColor = color;
        CanOpenDoor = true;
    }

    public void GetActiveDocument(Documents documents)
    {
        _activeDocument = documents;
        CanRead = true;
    }

    private Vector3 MousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = _camera.ScreenToWorldPoint(mousePosition);

        return mousePosition;
    }
}