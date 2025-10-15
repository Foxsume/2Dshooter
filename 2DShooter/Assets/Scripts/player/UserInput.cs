using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    // Inputs to detect; Jumping is adaptive -> 3 different jump inputs
    public Vector2 Move {  get; private set; }
    public bool JumpPressed {  get; private set; }
    public bool JumpBeingHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool Attack { get; private set; }
    public bool Crouch { get; private set; }
    public bool Dash {  get; private set; }
    public bool Plummet { get; private set; }
    public bool Sprint { get; private set; }
    public bool Primary { get; private set; }
    public bool Secondary { get; private set; }
    public bool Melee { get; private set; }
    public bool Holster { get; private set; }
    public bool Interact {  get; private set; }
    public bool Menu { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _move;
    private InputAction _jump;
    private InputAction _attack;
    private InputAction _crouch;
    private InputAction _dash;
    private InputAction _plummet;
    private InputAction _sprint;
    private InputAction _primary;
    private InputAction _secondary;
    private InputAction _melee;
    private InputAction _holster;
    private InputAction _interact;
    private InputAction _menu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetUpInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetUpInputActions()
    {
        _move = _playerInput.actions["Move"];
        _jump = _playerInput.actions["Jump"];
        _attack = _playerInput.actions["Attack"];
        _crouch = _playerInput.actions["Crouch"];
        _dash = _playerInput.actions["Dash"];
        _plummet = _playerInput.actions["Plummet"];
        _sprint = _playerInput.actions["Sprint"];
        _primary = _playerInput.actions["Primary"];
        _secondary = _playerInput.actions["Secondary"];
        _melee = _playerInput.actions["Melee"];
        _holster = _playerInput.actions["Holster"];
        _interact = _playerInput.actions["Interact"];
        _menu = _playerInput.actions["Menu"];
    }

    private void UpdateInputs()
    {
        Move = _move.ReadValue<Vector2>();
        JumpPressed = _jump.WasPressedThisFrame();
        JumpBeingHeld = _jump.IsPressed();
        JumpReleased = _jump.WasReleasedThisFrame();
        Attack = _attack.WasPressedThisFrame();
        Crouch = _crouch.WasPressedThisFrame();
        Dash = _dash.WasPressedThisFrame();
        Plummet = _plummet.WasPressedThisFrame();
        Sprint = _sprint.WasPressedThisFrame();
        Primary = _primary.WasPressedThisFrame();
        Secondary = _secondary.WasPressedThisFrame();
        Melee = _melee.WasPressedThisFrame();
        Holster = _holster.WasPressedThisFrame();
        Interact = _interact.WasPressedThisFrame();
        Menu = _menu.WasPressedThisFrame();
    }
}
