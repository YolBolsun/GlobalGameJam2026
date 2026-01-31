using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    InputAction moveAction;
    

    [Header("Movement setup variables")]
    [Tooltip("Movement Speed - Player Speed")]
    [SerializeField] private float movementSpeed;
    [Header("Sprint setup variables")]
    [Tooltip("Sprint Modifier - How much faster the player moves when sprinting")]
    [SerializeField] private float sprintModifier;
    InputAction sprintAction;
    // Will be using this for dash
    InputAction jumpAction;
    // Dash variables (remove if not wanted)
    [Header("Dash setup variables")]
    [Tooltip("Dash Distance - How far the dash is aiming")]
    [SerializeField] private float dashDistance;
    [Tooltip("Dash Cooldown - Length Between dashes")]
    [SerializeField] private float dashCooldown;
    [Tooltip("Dash Duration - How long the players move towards that target... maybe?")]
    [SerializeField] private float dashDuration;
    public Vector3 dashTarget;
    private float dashCooldownActive = 0f;
    private Vector3 prevPos;
    private float sprint;
    public Vector3 forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashTarget = transform.position;
        prevPos = transform.position;
        // Place holder for dash target
    }

    // Update is called once per frame
    void Update()
    {
        if (sprintAction.ReadValue<float>() > 0)
        {
            sprint = sprintModifier;
        }
        else
        {
            sprint = movementSpeed;
        }

        Vector3 moveValue = moveAction.ReadValue<Vector2>();
        transform.Translate(movementSpeed * Time.deltaTime * moveValue * sprint);
        // Dash logic (romove if not wanted)
        Vector3 moveDelta = transform.position - prevPos;
        moveDelta.Normalize();
        forward = moveDelta  + transform.position;
        dashTarget = transform.position + dashDistance * moveDelta;
        prevPos = transform.position;
        if (jumpAction.triggered & dashCooldownActive == 0)
        {
            Debug.Log("Jump triggered");
            transform.position = Vector2.Lerp(transform.position, dashTarget, dashDuration);
            dashCooldownActive = dashCooldown;
        }
        else if (dashCooldownActive > 0)
        {
            //Debug.Log("Time left on cooldown: " + dashCooldownActive);
            dashCooldownActive -= Time.deltaTime;
            if (dashCooldownActive < 0)
            {
                dashCooldownActive = 0;
            }
        }
    }
}
