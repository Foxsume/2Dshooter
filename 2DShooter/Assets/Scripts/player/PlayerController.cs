using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MovementX")] // Walk, run, dash, crouch
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private float xAxis = 0f;
    private float defaultGravity;
    private bool facingRight = true;
    private bool wasCrouching = false;
    private bool canDash = true;
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

    private void Start()
    {
        defaultGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (state.isDashing) return; // stops update function short if player is dashing

        Move(); // Updates xAxis value and flips player
        Crouch();
        Jump();
        Dash();
        Falling();
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = xAxis;

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
            state.isPlunging = false;
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
        rb.linearVelocityX = xAxis;

        if (xAxis < 0 && facingRight)
        {
            Flip();
        }
        else if (xAxis > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Crouch() // TODO: animation for crouch
    {
        if (input.Crouch)
        {
            state.isCrouched = true;
            anim.SetBool("Crouching", true);
            wasCrouching = true;
        }
        // no crouch input received but character stays crouched if standing is blocked
        else if (wasCrouching && state.ceilingHit)
        {
            state.isCrouched = true; // to make sure isCrouched stays true
            anim.SetBool("Crouching", true);
        }
        else
        {
            state.isCrouched = false;
            anim.SetBool("Crouching", false);
            wasCrouching = false;
        }
    }

    private void Dash() // TODO: fix dash
    {
        if (input.Dash && canDash)
        {
            StartCoroutine(Dashing());
        }
    }

    private IEnumerator Dashing() // TODO: animation for dash
    {
        canDash = false;
        state.isDashing = true;
        // anim.SetTrigger("Dash");
        trailRend.emitting = true;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashVelocity, 0);
        yield return new WaitForSeconds(dashTime);

        // After dashing
        trailRend.emitting = false;
        rb.gravityScale = defaultGravity;
        state.isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Jump() // TODO: Fix Jump Animation not triggering correctly
    {
        if (input.JumpPressed && state.isGrounded && !state.ceilingHit && !state.isCrouched)
        {
            anim.SetTrigger("Jump");
            state.isJumping = true;
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
        if (rb.linearVelocityY < fallingTreshold)
        {
            anim.SetTrigger("Fall");
            if (input.Plunge)
            {
                state.isPlunging = true;
            }
        }
    }

    public void OnLanding()
    {
        anim.SetTrigger("hitGround");
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
