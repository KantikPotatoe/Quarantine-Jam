using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeedDefault { get; set; }

    public float MovementSpeedSprint { get; set; }

    public float MovementSpeedCrawl { get; set; }

    // INPUT HANDLER
    private PlayerInputActions _inputActions;

    //INVENTORY
    private readonly bool[] _keys = new bool[Enum.GetValues(typeof(KeyColor)).Length];
    public GameObject[] slots;
    private Vector2 _movementInput;
    private float _movementSpeed;


    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputActions.PlayerControls.Sprint.performed += ctx => Sprint(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Crawl.performed += ctx => Crouch(ctx.ReadValue<float>());
        MovementSpeedDefault = 5f;
        MovementSpeedSprint = 8f;
        MovementSpeedCrawl = 2f;
    }


    private void Crouch(float v)
    {
        _movementSpeed = v > 0 ? MovementSpeedCrawl : MovementSpeedDefault;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>();
        _movementSpeed = MovementSpeedDefault;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void Sprint(float b)
    {
        _movementSpeed = b > 0 ? MovementSpeedSprint : MovementSpeedDefault;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Move()
    {
        transform.Translate(_movementInput * (_movementSpeed * Time.fixedDeltaTime));
    }

    public void PickUpKeyCard(int color)
    {
        _keys[color] = true;
        Debug.Log("Picked up the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color += new Color(0, 0, 0, 1);
    }

    public void UseKey(int color)
    {
        _keys[color] = false;
        Debug.Log("Used the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color -= new Color(0, 0, 0, 1);
    }

    public bool HasKeyOfColor(KeyColor color)
    {
        return _keys[(int) color];
    }
}