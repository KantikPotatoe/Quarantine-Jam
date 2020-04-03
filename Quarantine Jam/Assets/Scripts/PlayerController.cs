using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // INPUT HANDLER
    private PlayerInputActions inputActions;
    private Vector2 movementInput;

    [SerializeField] private float movementSpeed = 5f;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        float xPosition = movementInput.x;
        float yPosition = movementInput.y;

        move();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void move() 
    {
        transform.Translate(movementInput * movementSpeed * Time.deltaTime);
    }
}
