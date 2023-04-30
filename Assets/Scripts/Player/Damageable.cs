using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int> enemyKillPoint;

    Animator animator;

    public int maxHealth;
    public int health;

    [SerializeField] bool isAlive = true;
    [SerializeField] bool isInvincible = false;

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

        if (GameManager.HasInstance)
        {
            maxHealth = GameManager.Instance.Health;
            health = GameManager.Instance.Health;
        }
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

        if (GameManager.HasInstance)
        {
            maxHealth = GameManager.Instance.MaxHealth;
            health = GameManager.Instance.Health;
            GameManager.Instance.healthChanged?.Invoke(health, maxHealth);
            if (health <= 0)
            {
                isAlive = false;
            }
        }
    }

    //Return whether the damageable took damage or not
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            int actualDamage = 0;
            int maxDamage = health - damage;
            if (maxDamage < 0)
            {
                maxDamage = Mathf.Abs(maxDamage);
                actualDamage = maxDamage;
                if (GameManager.HasInstance)
                {
                    GameManager.Instance.Health = 0;
                    GameManager.Instance.healthChanged?.Invoke(health, maxHealth);
                }
            }
            else
            {
                actualDamage = damage;
                if (GameManager.HasInstance)
                {
                    GameManager.Instance.Health -= actualDamage;
                    GameManager.Instance.healthChanged?.Invoke(health, maxHealth);
                }
            }
            isInvincible = true;
            //Notify other subscribed components that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(actualDamage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, actualDamage);
            AudioManager.Instance.PlaySE(AUDIO.SE_61_HIT_03);
            return true;
        }
        //Unable to be hit
        return false;

    }

    public void Heal(int healthRestore)
    {
        if (IsAlive)
        {
            if (GameManager.HasInstance)
            {
                int maxHeal = Mathf.Max(GameManager.Instance.MaxHealth - GameManager.Instance.Health, 0);
                int actualHeal = Mathf.Min(maxHeal, healthRestore);
                GameManager.Instance.Health += actualHeal;
                CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
                AudioManager.Instance.PlaySE(AUDIO.SE_02_HEAL);
            }
        }
    }
}
