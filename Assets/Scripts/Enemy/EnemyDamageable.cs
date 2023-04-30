using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyDamageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> enemyDamageableHit;
    //public UnityEvent<int> enemyKillPoint;

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

    public int enemyKillScore = 5;
    public int bossKillScore = 10;
    public int demonKillScore = 20;
    private int currentScore;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (GameManager.HasInstance)
        {
            currentScore = GameManager.Instance.Scores;
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
                if (GameManager.HasInstance)
                {
                    GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                    if(enemy != null) 
                    {
                        GameManager.Instance.UpdateScores(currentScore + enemyKillScore);
                    }
                }
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
            enemyDamageableHit?.Invoke(actualDamage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, actualDamage);
            AudioManager.Instance.PlaySE(AUDIO.SE_61_HIT_03);
            return true;
        }
        //Unable to be hit
        return false;
    }
}