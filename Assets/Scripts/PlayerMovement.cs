using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    private float dirX;
    
    private enum MovementState { Idle, Run, JumpUp, JumpDown, Roll, AirAtk, Atk1, Atk2, Atk3 }
    private MovementState movementState;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        Jumping();

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        if (rigidBody.bodyType != RigidbodyType2D.Static)
        {
            rigidBody.velocity = new Vector2(dirX * playerSpeed, rigidBody.velocity.y);
        }
    }

    private void Jumping()
    {        
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            }
        }
    }

    private void UpdateAnimations()
    {
        if (dirX > 0f)
        {
            spriteRenderer.flipX = false;
            movementState = MovementState.Run;
        }
        else if (dirX < 0f)
        {
            spriteRenderer.flipX = true;
            movementState = MovementState.Run;
        }
        else
        {
            movementState = MovementState.Idle;
        }

        if (rigidBody.velocity.y > 0.1f)
        {
            movementState = MovementState.JumpUp;
        }
        else if (rigidBody.velocity.y < -0.1f)
        {
            movementState = MovementState.JumpDown;
        }

        animator.SetInteger("State", (int)movementState);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}