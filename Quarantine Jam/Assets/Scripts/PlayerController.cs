using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.WSA.Input;

public class PlayerController : MonoBehaviour
{
    // INPUT HANDLER
    private PlayerInputActions _inputActions;
    private Vector2 _movementInput;

    private const float MovementSpeedDefault = 5f;
    private const float MovementSpeedSprint = 8f;
    private const float MovementSPeedCrawl = 2f;
    private float _movementSpeed;

    //KEYCARDS
    private bool[] _keys = new bool[Enum.GetValues(typeof(KeyColor)).Length];


    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputActions.PlayerControls.Sprint.performed += ctx => Sprint(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Crawl.performed += ctx => Crouch(ctx.ReadValue<float>());
    }


    private void Crouch(float v)
    {
        _movementSpeed = v > 0 ? MovementSPeedCrawl : MovementSpeedDefault;
    }

    private void Start()
    {
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
        transform.Translate(_movementInput * (_movementSpeed * Time.deltaTime));
    }

    public void PickUpKeyCard(int color)
    {
        _keys[color] = true;
        Debug.Log("Picked up the " + (KeyColor) color + " card.");
    }
}