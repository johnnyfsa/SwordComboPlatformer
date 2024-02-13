using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public float springForce = 10;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private bool isOnSpring = false;
    public bool IsOnSpring
    {
        get
        {
            return isOnSpring;
        }
        set
        {
            isOnSpring = value;
            animator.SetBool(AnimationStrings.isOnSpring, value);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (collision.GetContact(0).normal == Vector2.down)
            {
                float correctionFactor = 1.5f;
                IsOnSpring = true;
                rb.AddForce((Vector2.up * springForce * correctionFactor), ForceMode2D.Impulse);
            }
            else
            {
                IsOnSpring = true;
                rb.AddForce((Vector2.up * springForce), ForceMode2D.Impulse);
            }
            AudioManager.Instance.PlaySFX(SoundType.Bounce);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsOnSpring = false;
    }
}
