using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private float crouchSpeed = 0.36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.05f;   // How much to smooth out the movement
    [SerializeField] private bool airControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask groundLayerMask;                          // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheckObj;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform ceilingCheckObj;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D crouchDisableCollider;                // A collider that will be disabled when crouching

    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float ceilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    public bool grounded;            // Whether or not the player is grounded.
    public Rigidbody2D rb2D;
    private bool facingRight = true;  // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;

    public float jumpVelocity = 15f;

    [Header("Events")]
    [Space]

    public UnityEvent onLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent onCrouchEvent;
    private bool wasCrouching = false;
    public bool jumpBlocked = false; // in case jump has to be disabled

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if (onLandEvent == null)
            onLandEvent = new UnityEvent();

        if (onCrouchEvent == null)
            onCrouchEvent = new BoolEvent();
    }

    // groundcheck is handled here
    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckObj.position, groundedRadius, groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    onLandEvent.Invoke();
            }
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching and block jump
            if (Physics2D.OverlapCircle(ceilingCheckObj.position, ceilingRadius, groundLayerMask))
            {
                crouch = true;
                jumpBlocked = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // If crouching
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    onCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= crouchSpeed;

                // Disable one of the colliders when crouching
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = true;

                if (wasCrouching)
                {
                    wasCrouching = false;
                    onCrouchEvent.Invoke(false);

                    // if jump was blocked due to being in a thight space
                    if (jumpBlocked)
                    {
                        jumpBlocked = false;
                    }
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rb2D.linearVelocity.y);
            // And then smoothing it out and applying it to the character
            rb2D.linearVelocity = Vector3.SmoothDamp(rb2D.linearVelocity, targetVelocity, ref velocity, movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (grounded && jump)
        {
            // make character jump by adding velocity on the y axis
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpVelocity);
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
