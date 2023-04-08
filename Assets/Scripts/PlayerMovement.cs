using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider2D;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    
    private float dirX;
    private bool canDoubleJump;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

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
        
    }

    private void AirAttack()
    {
        if (!IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetBool("AirAttack", true);
            }
            else animator.SetBool("AirAttack", false);
        }
    }

    private void UpdateAnimations()
    {
        
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        rigidBody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
    }

    private void Restart()
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        animator.Rebind();
    }
}