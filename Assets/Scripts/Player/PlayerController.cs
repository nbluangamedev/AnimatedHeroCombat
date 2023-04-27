using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float jumpImpulse = 10f;
    [SerializeField] float airWalkSpeed = 5f;

    [SerializeField] Vector2 moveInput;
    [SerializeField] bool CanDoubleJump;
    [SerializeField] TrailRenderer trailRenderer;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 25f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] ParticleSystem movementParticle;
    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;
    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem touchParticle;
    float counter;

    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;

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

    public bool CanAttack
    {
        get
        {
            return animator.GetBool(AnimationStrings.canAttack);
        }
    }

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

        if (moveInput.x != 0)
        {
            rb.sharedMaterial = noFriction;
        }
        else
        {
            rb.sharedMaterial = fullFriction;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            fallParticle.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            fallParticle.Clear();
        }
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
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            CanDoubleJump = false;
        }

        if (context.started && CanMove)
        {
            if (touchingDirections.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                CanDoubleJump = true;
                AudioManager.Instance.PlaySE(AUDIO.SE_30_JUMP);
            }
            else if (CanDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                fallParticle.Play();
                CanDoubleJump = !CanDoubleJump;
                animator.SetTrigger(AnimationStrings.rollingTrigger);
                AudioManager.Instance.PlaySE(AUDIO.SE_35_MISS_EVADE);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CanAttack)
            {
                animator.SetTrigger(AnimationStrings.attackTrigger);
                //AudioManager.Instance.PlaySE(AUDIO.SE_BATTLEAXE, 0.5f);
            }
        }
    }

    public void OnSpellAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CanAttack && touchingDirections.IsGrounded)
            {
                animator.SetTrigger(AnimationStrings.spellAttackTrigger);
                AudioManager.Instance.PlaySE(AUDIO.SE_SPELLATTACK, 0.4f);
            }
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
            AudioManager.Instance.PlaySE(AUDIO.SE_22_SLASH);
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

    //add animation event
    private void SoundOnRun()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_08_STEP_ROCK_02);
        }
    }

    private void SoundOnFire()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_18_THUNDER_02);
        }
    }
}
