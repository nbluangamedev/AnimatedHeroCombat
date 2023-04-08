using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
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

            if (health < 0)
                IsAlive = false;
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                //remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        Hit(10);
    }
    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }
}
