using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;
    public UnityEvent<int> enemyKillPoint;

    Animator animator;

    [SerializeField]
    private int maxHealth = 100;

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    [SerializeField]
    private int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthChanged?.Invoke(health, MaxHealth);
            if (health <= 0)
            {
                IsAlive = false;
            }
                
        }
    }

    [SerializeField]
    private bool isAlive = true;
    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return isAlive = true;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                //remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    //Return whether the damageable took damage or not
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            int actualDamage = 0;
            int maxDamage = Health - damage;
            if (maxDamage < 0)
            {
                maxDamage = Mathf.Abs(maxDamage);
                actualDamage = maxDamage;
                Health = 0;
            }
            else
            {
                actualDamage = damage;
                Health -= actualDamage;
            }
            isInvincible = true;
            //Notify other subscribed components that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(actualDamage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, actualDamage);
            return true;
        }
        //Unable to be hit
        return false;
    }

    public void Heal(int healthRestore)
    {
        if (IsAlive)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
        }
    }
}
