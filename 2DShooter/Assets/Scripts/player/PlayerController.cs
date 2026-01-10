using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MovementX")] // Walk, run, dash, crouch
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    private float xAxis = 0f;
    private int playerDir = 1;
    private bool wasCrouching = false;
    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashTime;
    private float dashCooldownTime;
    [Space(2)]

    [Header("MovementY")] // Jump, plummet
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float jumpCancelVelocity;
    [SerializeField] private float plungeVelocity;
    [SerializeField] private float fallingTreshold;
    [Space(2)]

    [Header("Values")]
    [SerializeField] private float health;

    // references
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerState state;
    private PlayerInputHandler input;
    private TrailRenderer trailRend;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
        input = GetComponent<PlayerInputHandler>();
        trailRend = GetComponent<TrailRenderer>();
    }

    private void Update() // handles input/animations/cooldowns
    {
        // Dash cooldown
        if (dashCooldownTime > 0) dashCooldownTime -= Time.deltaTime;

        // Dash time
        if (state.isDashing)
        {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                state.isDashing = false;
            }
            else return; // if dash hasn't ended -> early return
        }

        Move(); // Updates xAxis value and flips player
        Crouch();
        Dash();
        Jump();
        Falling();
    }

    private void FixedUpdate() // applies velocity
    {
        // horizontal(x) velocities
        if (state.isDashing)
        {
            rb.linearVelocity = new Vector2(playerDir * dashVelocity, 0);
            return; // if dash hasn't ended -> early return
        }

        rb.linearVelocityX = xAxis; // xAxis value is set in Move method

        // vertical(y) velocities
        if (state.isJumping)
        {
            rb.linearVelocityY = jumpVelocity;
            state.isJumping = false;
        }
        else if (state.jumpCanceled)
        {
            rb.linearVelocityY = jumpCancelVelocity;
            state.jumpCanceled = false;
        }
        else if (state.isPlunging)
        {
            rb.linearVelocityY = plungeVelocity;
            // state.isPlunging = false is applied in the OnLanding event
        }
    }

    private void Move()
    {
        if (input.Sprint)
        {
            xAxis = input.Move.x * sprintSpeed;
        }
        else if (state.isCrouched)
        {
            xAxis = input.Move.x * crouchSpeed;
        }
        else
        {
            xAxis = input.Move.x * walkSpeed;
        }
        anim.SetFloat("Speed", Mathf.Abs(xAxis));

        if (xAxis < 0 && playerDir > 0)
        {
            Flip();
        }
        else if (xAxis > 0 && playerDir < 0)
        {
            Flip();
        }
    }

    private void Crouch()
    {
        if (input.Crouch)
        {
            state.isCrouched = true;
            wasCrouching = true;
            anim.SetBool("IsCrouching", true);
        }
        else if (wasCrouching && state.ceilingHit) // no crouch input received but character stays crouched if standing is blocked
        {
            state.isCrouched = true; // to make sure isCrouched stays true
            anim.SetBool("IsCrouching", true);
        }
        else
        {
            state.isCrouched = false;
            wasCrouching = false;
            anim.SetBool("IsCrouching", false);
        }
    }

    private void Dash()
    {
        if (input.Dash && !state.isCrouched && dashCooldownTime <= 0)
        {
            dashCooldownTime = dashCooldown; // Start cooldown for dash
            dashTime = dashDuration; // Start dash timer

            state.isDashing = true;
            TriggerAnimation("Dash");
        }
    }

    private void Jump()
    {
        if (input.JumpPressed && state.isGrounded && !state.ceilingHit && !state.isCrouched)
        {
            state.isJumping = true;
            TriggerAnimation("Jump");
        }
        else if (rb.linearVelocityY > 0f && input.JumpReleased)
        {
            if(rb.linearVelocityY > jumpCancelVelocity)
            {
                state.jumpCanceled = true;
            }
        }
    } 

    private void Falling()
    {
        if (!state.isGrounded && rb.linearVelocityY < fallingTreshold)
        {
            if (input.Plunge)
            {
                state.isPlunging = true;
                TriggerAnimation("Plunge");
            }
            else if (!state.isPlunging)
            {
                TriggerAnimation("Fall");
            }
        }
    }

    public void OnLanding()
    {
        state.isPlunging = false;
        TriggerAnimation("Land");
    }

    private void TriggerAnimation(string triggerName)
    {
        // check if the animation with the given trigger is not currently playing
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(triggerName))
        {
            anim.SetTrigger(triggerName);
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        playerDir *= -1;
    }
}
