using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;

    Animator animator;
    Rigidbody2D rb;
    EnemyDamageable enemyDamageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        private set
        {
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyDamageable = GetComponent<EnemyDamageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

    }

    private void FixedUpdate()
    {
        if (enemyDamageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            //Dead flyier falls to the ground
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Flight()
    {
        //Fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        //See if we need to switch waypoint
        if (distance <= waypointReachedDistance)
        {
            //Switch the next waypoint
            waypointNum++;

            if(waypointNum>=waypoints.Count)
            {
                //Loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 currentScale = transform.localScale;
        //Facing the right
        if(transform.localScale.x > 0)
        {
            if(rb.velocity.x < 0)
            //Flip
            transform.localScale = new Vector3(-1 * currentScale.x, currentScale.y, currentScale.z);
        }
        else
        {
            //Facing the left
            if (rb.velocity.x > 0)
                transform.localScale = new Vector3(-1 * currentScale.x, currentScale.y, currentScale.z);
        }
    }
}
