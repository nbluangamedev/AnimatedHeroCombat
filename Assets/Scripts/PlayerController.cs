using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 15f;
    [SerializeField]
    private float jumpImpulse = 10f;
    [SerializeField]
    private float airWalkSpeed = 5f;

    private Vector2 moveInput;

    TouchingDirections touchingDirections;

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

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);

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
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CanAttack)
                animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
