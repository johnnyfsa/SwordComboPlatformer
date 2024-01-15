using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private Damageble target;
    public int damage;
    private Vector2 knockback = Vector2.zero;
    private Vector2 attackDirection = Vector2.zero;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageble>(out target))
        {
            target.ArmorValue = 0;
            target.Hit(damage, knockback, attackDirection);
        }
    }
}
