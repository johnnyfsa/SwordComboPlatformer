using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    [Header("GeoundDetection")]
    [SerializeField]
    private Transform groundDetector;
    public Vector2 groundDetectorSize;
    [SerializeField]
    private bool isOnGround;

    public bool IstakingHit
    {
        get
        {
            return enemyAnimator.GetBool(AnimationStrings.isTakingHit);
        }
    }
    public bool IsOnGround
    {
        get
        {
            return isOnGround;
        }
        private set
        {
            isOnGround = value;
            if (enemyAnimator != null)
                enemyAnimator.SetBool(AnimationStrings.isGrounded, isOnGround);
        }
    }
    [Header("EdgeDetection")]
    [SerializeField]
    Transform edgeDetector;
    public Vector2 edgeDetectorSize;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float moveSpeed = 3f;
    [SerializeField]
    private bool isMoving;

    public bool CanMove
    {
        get
        {
            if (enemyAnimator != null)
                return enemyAnimator.GetBool(AnimationStrings.canMove);
            else return false;
        }
        set
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool(AnimationStrings.canMove, value);
            }
        }
    }

    private bool hasTarget = false;

    public bool HasTarget
    {
        get { return hasTarget; }
        set
        {
            hasTarget = value;
            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool(AnimationStrings.hasTarget, value);
            }
        }
    }

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            enemyAnimator.SetBool(AnimationStrings.isMoving, isMoving);
        }
    }



    [Header("Attack")]
    [SerializeField]
    DetectTargets detectionZone;
    private bool isOnEdge = false;

    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove)
        {
            Move();
            if (IsOnGround)
            {
                DetectEdge();
                if (isOnEdge)
                {
                    Flip();
                }
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
        }

    }

    private void DetectGround()
    {
        if (Physics2D.OverlapBox(groundDetector.position, groundDetectorSize, 0, groundLayer))
        {
            IsOnGround = true;
        }
        else
        {
            IsOnGround = false;
        }

    }

    private void Update()
    {
        DetectGround();
        if (detectionZone.targets.Count > 0)
        {
            HasTarget = true;
        }
        else
        {
            HasTarget = false;
        }
    }

    //set methods Move and Flip
    public void Move()
    {
        if (!IstakingHit)
        {
            if (!isFacingRight)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }
    }

    private void DetectEdge()
    {
        if (!Physics2D.OverlapBox(edgeDetector.position, edgeDetectorSize, 0, groundLayer))
        {
            isOnEdge = true;
        }
        else
        {
            isOnEdge = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(edgeDetector.position, edgeDetectorSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundDetector.position, groundDetectorSize);
    }

    public void TakeDamage(int damage, Vector2 knockBack, Vector2 attackDirection)
    {
        //facing left <--
        if (!isFacingRight)
        {
            //atack comes from right to left <--
            if (attackDirection.x < 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x - knockBack.x, knockBack.y); //push left
                Flip();
            }
            //atack comes from left to right -->
            else if (attackDirection.x >= 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x + knockBack.x, knockBack.y);//push right
            }
        }
        //facing right-->
        else
        {
            //atack comes from right to left <--
            if (attackDirection.x <= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - knockBack.x, knockBack.y); //push left
            }
            //atack comes from left to right -->
            else
            {
                rb.velocity = new Vector2(rb.velocity.x + knockBack.x, knockBack.y);//push right
                Flip();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (IsOnGround)
            {
                Flip();
            }
        }
    }
}
