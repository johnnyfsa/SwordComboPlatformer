using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageble>(out Damageble damageble))
        {
            damageble.Hit(damageble.CurrentHealth, Vector2.zero, Vector2.zero);
        }
    }
}
