using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    // PlayerState holds information for both PlayerController and Weapon scripts. 
    // On some cases PlayerStates' values are modified by PlayerController. 
    // PlayerState itself has no access to other scripts 

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform ceilingCheckPoint;
    [SerializeField] private float VerticalCheck = 0.2f;
    [SerializeField] private float HorizontalCheck = 0.2f;
    [SerializeField] private Collider2D standingCollider; // Gets disabled when crouching

    // states that are updated by PlayerController
    public bool isJumping = false;
    public bool jumpCanceled = false;
    public bool isPlunging = false;
    public bool isDashing = false;
    public bool isCrouched = false;
    public bool isGrounded = false;
    public bool ceilingHit = false;

    public UnityEvent OnLandEvent;

    private void Update()
    {
        Grounded();
        CeilingHit();

        if (isCrouched)
        {
            standingCollider.enabled = false;
        }
        else if (!isCrouched)
        {
            standingCollider.enabled = true;
        }
    }

    // Ground check
    public void Grounded()
    {
        bool wasGrounded = isGrounded;

        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, VerticalCheck, groundLayer)
         || Physics2D.Raycast(groundCheckPoint.position + new Vector3(HorizontalCheck, 0, 0), Vector2.down, VerticalCheck, groundLayer)
         || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-HorizontalCheck, 0, 0), Vector2.down, VerticalCheck, groundLayer))
        {
            isGrounded = true;
            if (wasGrounded)
            {
                OnLandEvent.Invoke();
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    // Ceiling check
    public void CeilingHit()
    {
        if (Physics2D.Raycast(ceilingCheckPoint.position, Vector2.down, VerticalCheck, groundLayer))
        {
            ceilingHit = true;
        }
        else 
        { 
            ceilingHit = false; 
        }
    }
}
