using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 2.0f;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    public bool isFalling = false;
    public float timeToFall = 2.0f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (!isFalling)
        { return; }
        rb.velocity += Vector2.down * Time.fixedDeltaTime * fallSpeed;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player) && collision.GetContact(0).normal == Vector2.down)
        {
            StartCoroutine(ActivateFallingPlatform());
        }
    }

    private IEnumerator ActivateFallingPlatform()
    {
        yield return new WaitForSeconds(timeToFall);
        isFalling = true;
        boxCollider.isTrigger = true;

    }

}
