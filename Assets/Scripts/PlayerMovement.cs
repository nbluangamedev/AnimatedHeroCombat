using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask jumpableGround;
    //[SerializeField] private TrailRenderer trailRenderer;
    //[SerializeField] private ParticleSystem moveEffect;
    //[SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    //[Range(0, 10)]
    //[SerializeField] private int occurAfterVelocity;
    //[Range(0, 0.2f)]
    //[SerializeField] private float dustFormationPeriod;
    //private float counter;
    private float dirX;
    private bool canDoubleJump;

    //private bool canDash = true;
    //private bool isDashing;
    //private float dashingPower = 24f;
    //private float dashingTime = 0.2f;
    //private float dashingCooldown = 1f;

    private enum AttackState { Attack1, Attack2, Attack3 }
    private AttackState attackState;
    private enum MovementState { Idle, Run, JumpUp, JumpDown }
    private MovementState movementState;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //trailRenderer = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        //trailRenderer.emitting = false;
    }

    void Update()
    {
        //if (isDashing)
        //{
        //    return;
        //}

        //counter += Time.deltaTime;
        //if (IsGrounded() && Mathf.Abs(rigidBody.velocity.x) > occurAfterVelocity)
        //{
        //    if (counter > dustFormationPeriod)
        //    {
        //        moveEffect.Play();
        //        counter = 0;
        //    }
        //}

        dirX = Input.GetAxisRaw("Horizontal");

        Jumping();

        AirAttack();

        UpdateAnimations();

        //if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        //{
        //    StartCoroutine(Dash());
        //    if (AudioManager.HasInstance)
        //    {
        //        AudioManager.Instance.PlaySE(AUDIO.SE_DASH);
        //    }
        //}
    }

    private void FixedUpdate()
    {
        //if (isDashing)
        //{
        //    return;
        //}

        Moving();
        Attack();
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
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            canDoubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || canDoubleJump)
            {
                //if (AudioManager.HasInstance)
                //{
                //    AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
                //}
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
                canDoubleJump = !canDoubleJump;
                animator.SetBool("DoubleJump", !canDoubleJump);
                //if (!canDoubleJump)
                //{
                //    jumpEffect.Play();
                //}
            }
        }
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

    private void Attack()
    {

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            attackState = AttackState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            attackState = AttackState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            attackState = AttackState.Attack1;
        }
        animator.SetInteger("State", (int)movementState);
        animator.SetInteger("Attack", (int)attackState);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    //private IEnumerator Dash()
    //{
    //    canDash = false;
    //    isDashing = true;
    //    float originalGravity = rigidBody.gravityScale;
    //    rigidBody.gravityScale = 0f;
    //    rigidBody.velocity = new Vector2(dirX * dashingPower, 0f);
    //    trailRenderer.emitting = true;
    //    yield return new WaitForSeconds(dashingTime);
    //    trailRenderer.emitting = false;
    //    rigidBody.gravityScale = originalGravity;
    //    isDashing = false;
    //    yield return new WaitForSeconds(dashingCooldown);
    //    canDash = true;
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(AUDIO.SE_DEATH);
        //}
        rigidBody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
    }

    private void Restart()
    {
        this.transform.position = playerSpawnPoint.position;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        animator.Rebind();
    }

    //viet ham tao box collider2d toi cho chem collider toi nhan vat
}