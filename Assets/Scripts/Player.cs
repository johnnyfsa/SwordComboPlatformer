using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Damageble damageble;
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
    [SerializeField]
    private int jumpsLeft;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    private bool isGrounded;


    [Header("Fighting")]
    public LayerMask enemyLayer;
    public bool IsBlocking { get { return animator.GetBool(AnimationStrings.isBlocking); } }

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

    public bool LockedVelocity { get { return animator.GetBool(AnimationStrings.lockVelocity); } }

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

    public bool IsWallSliding
    {
        get { return isWallSliding; }
        set
        {
            isWallSliding = value;
            animator.SetBool(AnimationStrings.isWallSliding, value);
        }
    }
    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(5f, 10f);

    public bool isTouchingWall = false;


    [Header("Dashing")]
    [SerializeField]
    private bool canDash;
    public bool IsDashing
    {
        get
        {
            return animator.GetBool(AnimationStrings.isDashing);
        }
        set
        {
            animator.SetBool(AnimationStrings.isDashing, value);
        }
    }
    [SerializeField]
    private float dashForce = 24.0f;
    private float dashTime = 0.3f;
    [SerializeField]
    private float dashCooldown = 1.0f;
    private void Awake()
    {
        damageble = GetComponent<Damageble>();
    }

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
        WallCheck();
        ProcessWallSlide();
        ProcessWallJump();
        EnemyHeadCheck();
        if (IsDashing)
        { return; }
        if (isWallJumping)
        { return; }
        if (!LockedVelocity)
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
        if (IsWallSliding)
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
        if (!IsGrounded && isTouchingWall && horizontalMovement != 0)
        {
            IsWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            IsWallSliding = false;
        }

    }

    private bool WallCheck()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer); //returns true if there is a wall in front of the player Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer); //returns true if there is a wall in front of the player
        return isTouchingWall;

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
                animator.SetTrigger(AnimationStrings.jumpTrigger);
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y / 2)); // half the velocity on the y axis
                jumpsLeft--;
            }

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
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 270.0f, enemyLayer) && !IsDashing)
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

    public void Block(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger(AnimationStrings.blockTrigger);
        }
        else
        {
            animator.SetBool(AnimationStrings.isBlocking, false);
        }
    }

    public void DashBtnPressed(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(0, 7, true);
        canDash = false;
        IsDashing = true;
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0);
        yield return new WaitForSeconds(dashTime);
        IsDashing = false;
        Physics2D.IgnoreLayerCollision(0, 7, false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public void TakeDamage(int damage, Vector2 knockBack, Vector2 attackDirection)
    {
        //facing left
        if (!isFacingRight)
        {
            //atack comes from right to left
            if (attackDirection.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - knockBack.x, knockBack.y); //push left
            }
            //atack comes from left to right
            else
            {
                rb.velocity = new Vector2(rb.velocity.x + knockBack.x, knockBack.y);//push right
            }
        }
        //facing right
        else
        {
            //atack comes from right to left
            if (attackDirection.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - knockBack.x, knockBack.y); //push left
            }
            //atack comes from left to right
            else
            {
                rb.velocity = new Vector2(rb.velocity.x + knockBack.x, knockBack.y);//push right
            }
        }

    }
}
