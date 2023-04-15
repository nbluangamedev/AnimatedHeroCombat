using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 15f;
    [SerializeField]
    private float jumpImpulse = 10f;
    [SerializeField]
    private float airWalkSpeed = 5f;

    [SerializeField]
    private Vector2 moveInput;
    [SerializeField] private bool CanDoubleJump;
    [SerializeField] private TrailRenderer trailRenderer;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField]
    private float dashingPower = 25f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private ParticleSystem movementParticle;
    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;
    float counter;

    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsRunning && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                        return runSpeed;
                    //Air move
                    else return airWalkSpeed;
                }
                //Idle speed
                else return 0;
            }
            //Movement locked
            else return 0;
        }
    }

    private bool isRunning = false;
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        private set
        {
            isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private bool isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        private set
        {
            if (isFacingRight != value)
            {
                //Flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }
            isFacingRight = value;
        }
    }

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void SetFacingDirection(Vector2 runInput)
    {
        if (runInput.x > 0 && !IsFacingRight)
        {
            //Face the right
            IsFacingRight = true;
        }
        else if (runInput.x < 0 && IsFacingRight)
        {
            //Face the left
            IsFacingRight = false;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    //public bool CanAttack
    //{
    //    get
    //    {
    //        return animator.GetBool(AnimationStrings.canAttack);
    //    }
    //}

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        counter += Time.deltaTime;
        if (touchingDirections.IsGrounded && Mathf.Abs(rb.velocity.x) > occurAfterVelocity)
        {
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
        }
        
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsRunning = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else isRunning = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //if (context.started && touchingDirections.IsGrounded && CanMove)
        //{
        //    animator.SetTrigger(AnimationStrings.jumpTrigger);
        //    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        //    CanDoubleJump = true;
        //}

        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            CanDoubleJump = false;
        }

        if (context.started && CanMove)
        {
            if (touchingDirections.IsGrounded || CanDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                CanDoubleJump = !CanDoubleJump;
                animator.SetTrigger(AnimationStrings.rollingTrigger);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //if (CanAttack)
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnSpellAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //if (CanAttack)
            animator.SetTrigger(AnimationStrings.spellAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && isDashing)
        {
            return;
        }

        if (context.started && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(moveInput.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
