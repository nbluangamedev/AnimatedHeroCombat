using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Uses the collider to check directions to see if the object is currently on the ground, touching wall, touching ceiling
public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    private CapsuleCollider2D touchingCollider;
    public float groundDistance = 0.1f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.1f;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    private Animator animator;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        private set
        {
            isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private bool isOnWall;
    public bool IsOnWall
    {
        get
        {
            return isOnWall;
        }
        private set
        {
            isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    private bool isOnCeiling;

    public bool IsOnCeiling
    {
        get
        {
            return isOnCeiling;
        }
        private set
        {
            isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
