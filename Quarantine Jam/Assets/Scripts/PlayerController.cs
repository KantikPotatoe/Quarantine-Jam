using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // INPUT HANDLER
    private PlayerInputActions inputActions;
    private Vector2 movementInput;

    [SerializeField] private readonly float movementSpeedDefault = 5f;
    [SerializeField] private readonly float movementSpeedSprint = 8f;
    [SerializeField] private readonly float movementSPeedCrawl = 2f;
    private float movementSpeed;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.PlayerControls.Sprint.performed += ctx => sprint(ctx.ReadValue<float>());
        inputActions.PlayerControls.Crawl.performed += ctx => crouch(ctx.ReadValue<float>());
    }

    private void crouch(float v)
    {
        if (v > 0)
        {
            movementSpeed = movementSPeedCrawl;
        }
        else
        {
            movementSpeed = movementSpeedDefault;
        }
    }

    private void Start()
    {
        movementSpeed = movementSpeedDefault;
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

    private void sprint(float b)
    {
        if (b > 0)
        {
            movementSpeed = movementSpeedSprint;
        }
        else
        {
            movementSpeed = movementSpeedDefault;
        }
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
