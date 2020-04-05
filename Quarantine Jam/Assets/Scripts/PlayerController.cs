﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isHidden;
    public bool CanHide { get; set; }
    private float MovementSpeedDefault { get; set; }

    private float MovementSpeedSprint { get; set; }

    private float MovementSpeedCrawl { get; set; }

    // INPUT HANDLER
    private PlayerInputActions _inputActions;

    //INVENTORY
    private readonly bool[] _keys = new bool[Enum.GetValues(typeof(KeyColor)).Length];
    public GameObject[] slots;
    private Vector2 _movementInput;
    private float _movementSpeed;
    private Vector2 _moveVelocity;
    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputActions.PlayerControls.Sprint.performed += ctx => Sprint(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Crawl.performed += ctx => Crouch(ctx.ReadValue<float>());
        _inputActions.PlayerControls.Interact.performed += ctx => Interact(ctx.ReadValue<float>());
        MovementSpeedDefault = 5f;
        MovementSpeedSprint = 8f;
        MovementSpeedCrawl = 2f;
    }

    private void Interact(float v)
    {
        if (CanHide)
        {
            Hide();
        }
    }


    private void Crouch(float v)
    {
        _movementSpeed = v > 0 ? MovementSpeedCrawl : MovementSpeedDefault;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>();
        _movementSpeed = MovementSpeedDefault;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveVelocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        _moveVelocity = _movementInput.normalized * _movementSpeed;
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

    private void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        isHidden = true;
        CanHide = false;
    }

    public void Reveal()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        isHidden = false;
    }
}