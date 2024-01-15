using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Collider2D))]
public class MeleeAttack : MonoBehaviour
{
    public UnityEvent OnTargetParry;
    private Vector2 attackDirection = Vector2.zero;
    Collider2D meleeAttackCollider;
    public int attackDamage;
    public Vector2 knockBack = Vector2.zero;

    private void Awake()
    {
        meleeAttackCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackDirection = (collision.transform.position - transform.position).normalized;
        //check if it has the damageble component
        Damageble damageble = collision.GetComponent<Damageble>();
        if (damageble != null)
        {
            //preferable to be an Interface
            if (damageble.Hit(attackDamage, knockBack, attackDirection))
            {

            }
            else if (damageble.IsParrying)
            {
                OnTargetParry?.Invoke();
            }
        }
    }
}
