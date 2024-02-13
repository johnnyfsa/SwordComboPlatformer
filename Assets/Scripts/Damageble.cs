using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageble : MonoBehaviour
{
    public UnityEvent<int, Vector2, Vector2> OnMeleeAttackHit;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool isAlive;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, isAlive);
        }
    }

    [SerializeField]
    private bool isInvencible;
    private float timeSinceHit;
    public float invencibilityTime = 0.25f;

    public bool IsInvencible
    {
        get
        {
            return isInvencible;
        }
        set
        {
            isInvencible = value;
        }
    }


    [SerializeField]
    private bool isParrying;

    public bool IsParrying
    {
        get
        {
            if (animator != null)
                return animator.GetBool(AnimationStrings.isParrying);
            return false;
        }
        set
        {
            isParrying = value;
        }

    }

    public bool IsBlocking
    {
        get
        {
            if (animator)
                return animator.GetBool(AnimationStrings.isBlocking);
            return false;
        }
    }

    [SerializeField]
    private int armorValue;
    public int ArmorValue
    {
        get
        {
            return armorValue;
        }
        set
        {
            armorValue = value;
        }
    }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        IsAlive = true;
        IsParrying = false;
    }

    public void RecieveParry()
    {
        animator.SetTrigger(AnimationStrings.AttackBlocked);
        OnMeleeAttackHit?.Invoke(0, new Vector2(1f, 0f), Vector2.zero);
    }

    public bool Hit(int damage, Vector2 knockBack, Vector2 attackDirection)
    {
        if (IsAlive && !IsInvencible && !IsParrying)
        {
            if (IsBlocking)
            {
                damage = damage - armorValue;
            }
            CurrentHealth -= damage;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            OnMeleeAttackHit?.Invoke(damage, knockBack, attackDirection);
            CharacterEvents.OnCharacterDamaged?.Invoke(this.transform, damage);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update()
    {
        if (!IsInvencible)
        {
            return;
        }
        else
        {
            if (timeSinceHit > invencibilityTime)
            {
                IsInvencible = false;
                timeSinceHit = 0.0f;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public void ResetHealth()
    {
        this.currentHealth = this.maxHealth;
        if (!IsAlive)
        {
            IsAlive = true;
        }
    }
}
