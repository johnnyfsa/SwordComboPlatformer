using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    bool isFacingRight = true;

    [SerializeField]
    private Animator animator;
    [Header("Movement")]
    public float speed = 5.0f;
    private float horizontalMovement;

    public float HorizontalMovement
    {
        get
        {
            if (CanMove)
            {
                return horizontalMovement;
            }
            else
            {
                return 0;
            }
        }
    }
    private bool isMoving = false;

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    [Header("Jumping")]
    public float jumpForce = 10.0f;
    public int maxJumps = 2;
    private int jumpsLeft;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    private bool isGrounded;


    [Header("Fighting")]
    public LayerMask enemyLayer;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask wallLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(5f, 10f);


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();
        EnemyHeadCheck();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(HorizontalMovement * speed, rb.velocity.y);
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
            Flip();

        }
    }

    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //fall increasingly faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); //clamp fall speed
        }
        else
        {
            rb.gravityScale = baseGravity; //reset gravity scale to base value
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void ProcessWallSlide()
    {
        //not grounded & on a wall & movement !=0
        if (!IsGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

    }

    private bool WallCheck()
    {

        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer); //returns true if there is a wall in front of the player Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer); //returns true if there is a wall in front of the player
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        IsMoving = horizontalMovement != 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsLeft > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft--;
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y / 2)); // half the velocity on the y axis
                jumpsLeft--;
            }
            animator.SetTrigger(AnimationStrings.jumpTrigger);
        }

        //wall jumping
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpTimer = 0f;
            //force flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); //wall jump = 0.5f -- jump again = 0.6f
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsLeft = maxJumps;
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }


    private void EnemyHeadCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, enemyLayer))
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // half the velocity on the y axis
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
