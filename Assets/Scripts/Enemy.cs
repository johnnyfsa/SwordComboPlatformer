using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    Transform edgeDetector;
    public Vector2 edgeDetectorSize;
    public LayerMask groundLayer;

    private bool isOnEdge = false;

    private bool isFacingRight = true;
    public float moveSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        DetectEdge();
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
