using UnityEngine;

// Handles player movement inputs + most animations
public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    public float walkSpeed = 15f;
    public float runSpeed = 35f;

    float horizontalMove = 0f;
    bool jump = false;
    bool jumpCancel = false;
    bool crouch = false;

    float jumpCancelVelocity = 5f;
    float fastfallGravity = 6;
    float defaultGravity;
    float fallingTreshold = -10f;

    private void Start()
    {
        defaultGravity = controller.rb2D.gravityScale;
    }

    void Update()
    {
        // character's speed is calculated here
        if (Input.GetButton("Sprint"))
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        else
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * walkSpeed;
        }
        // Animator will play different animations according to the calculated travel speed. 
        // Mathf.Abs will always make the value a positive number
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && !controller.jumpBlocked)
        {
            if (controller.grounded == true)
            {
                animator.SetTrigger("Jump");
            }
            jump = true;
        }
        else if (controller.rb2D.linearVelocity. y > 0 && Input.GetButtonUp("Jump"))
        {
            jumpCancel = true;
        }

        // crouching
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        // fall animations + faster falling
        if (controller.rb2D.linearVelocity.y < fallingTreshold)
        {
            animator.SetTrigger("Fall");
            if (Input.GetKeyDown(KeyCode.E))
            {
                controller.rb2D.gravityScale = fastfallGravity;
            }
        }
    }

    public void OnLanding()
    {
        controller.rb2D.gravityScale = defaultGravity;
        animator.SetTrigger("hitGround");
    }

    // crouch animation is handled trough events
    public void OnCrouching (bool Crouching)
    {
        animator.SetBool("Crouching", Crouching);
    }

    private void FixedUpdate()
    {
        // Move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        if (jumpCancel)
        {
            if (controller.rb2D.linearVelocity.y > jumpCancelVelocity)
            {
                controller.rb2D.linearVelocity = new Vector2(controller.rb2D.linearVelocity.x, jumpCancelVelocity);
            }
            jumpCancel = false;
        }
    }
}
