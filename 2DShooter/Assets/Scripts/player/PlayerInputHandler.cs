using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    // Sprint and crouch can be set to toggle TODO: add UI elements to control settings menu
    [Header("Input toggle options")]
    [SerializeField] private bool toggleSprint = false;
    [SerializeField] private bool toggleCrouch = false;

    public Vector2 Move {  get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }

    public bool Sprint { get; private set; }
    public bool Crouch { get; private set; }

    public bool AttackPressed { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool AttackReleased { get; private set; }

    public bool Dash { get; private set; }
    public bool Plunge { get; private set; }
    public bool Reload { get; private set; }
    public bool Holster { get; private set; }
    public bool Primary { get; private set; }
    public bool Secondary { get; private set; }
    public bool Melee { get; private set; }
    public bool Interact { get; private set; }
    public bool Menu { get; private set; }


    // Internal states for toggling
    private bool sprintToggled = false;
    private bool crouchToggled = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        // Movement
        playerInput.actions["Move"].performed += ctx => Move = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => Move = Vector2.zero;

        // Jump
        playerInput.actions["Jump"].started += ctx => JumpPressed = true;
        playerInput.actions["Jump"].performed += ctx => JumpHeld = true;
        playerInput.actions["Jump"].canceled += ctx => { JumpHeld = false; JumpReleased = true; };

        // Sprint
        playerInput.actions["Sprint"].started += ctx =>
        {
            if (toggleSprint)
            {
                sprintToggled = !sprintToggled;
                Sprint = sprintToggled;
            }
            else
            {
                Sprint = true;
            }
        };
        playerInput.actions["Sprint"].canceled += ctx =>
        {
            if (!toggleSprint)
            {
                Sprint = false;
            }
        };

        // Crouch
        playerInput.actions["Crouch"].started += ctx =>
        {
            if (toggleCrouch)
            {
                crouchToggled = !crouchToggled;
                Crouch = crouchToggled;
            }
            else
            {
                Crouch = true;
            }
        };
        playerInput.actions["Crouch"].canceled += ctx =>
        {
            if (!toggleCrouch)
            {
                Crouch = false;
            }
        };

        // Attack
        playerInput.actions["Attack"].started += ctx =>
        {
            AttackPressed = true;
            AttackHeld = true;
        };
        playerInput.actions["Attack"].canceled += ctx =>
        {
            AttackHeld = false;
            AttackReleased = true;
        };

        // One-shot actions
        playerInput.actions["Dash"].performed += ctx => Dash = true;
        playerInput.actions["Plunge"].performed += ctx => Plunge = true;
        playerInput.actions["Primary"].performed += ctx => Primary = true;
        playerInput.actions["Secondary"].performed += ctx => Secondary = true;
        playerInput.actions["Melee"].performed += ctx => Melee = true;
        playerInput.actions["Reload"].performed += ctx => Reload = true;
        playerInput.actions["Holster"].performed += ctx => Holster = true;
        playerInput.actions["Interact"].performed += ctx => Interact = true;
        playerInput.actions["Menu"].performed += ctx => Menu = true;
    }

    private void LateUpdate()
    {
        // Reset one-shot inputs here
        Dash = false;
        Plunge = false;
        AttackPressed = false;
        AttackReleased = false;
        Primary = false;
        Secondary = false;
        Melee = false;
        Reload = false;
        Holster = false;
        Interact = false;
        Menu = false;

        // Reset jump state tracking
        JumpPressed = false;
        JumpReleased = false;
    }
}
