using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;
    private Rigidbody2D rb;
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
        get { return enemyAnimator.GetBool(AnimationStrings.canMove); }
        set
        {
            enemyAnimator.SetBool(AnimationStrings.canMove, value);
        }
    }

    private bool hasTarget = false;

    public bool HasTarget
    {
        get { return hasTarget; }
        set
        {
            hasTarget = value;
            enemyAnimator.SetBool(AnimationStrings.hasTarget, value);
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove)
        {
            Move();
            DetectEdge();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

    }

    private void Update()
    {
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

    private void DetectEdge()
    {
        if (!Physics2D.OverlapBox(edgeDetector.position, edgeDetectorSize, 0, groundLayer))
        {
            isOnEdge = true;
            Flip();
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
    }
}
