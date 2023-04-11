using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class AIController2D : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float turnRate;
    [SerializeField] float jumpHeight;
    [SerializeField] float doubleJumpHeight;
    [SerializeField] float hitForce;
    [SerializeField, Range(1, 5)] float fallRateMultiplier;
    [Header("Ground")]
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundRadius;
    Vector2 velocity = Vector2.zero;
    Rigidbody2D rb;
    [SerializeField]SpriteRenderer spriteRenderer;
    [SerializeField] Transform[] waypoints;
    [Header("AI")]
    [SerializeField] float rayDistance = 1;
    bool faceRight = true;
    float groundAngle = 0;
    Transform targetWaypoint = null;

    enum State
    {
        IDLE,
        PATROL,
        ATTACK,
        CHASE,
        DEAD
    }

    State currentState = State.IDLE;
    float stateTimer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer= GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector2 direction = Vector2.zero;
        targetWaypoint = waypoints[Random.Range(0, waypoints.Length)];

        // update ai
        switch (currentState)
        {
            case State.IDLE:
                if (canSeePlayer()) currentState = State.CHASE;

                stateTimer += Time.deltaTime;
                if (stateTimer > 0.5f)
                {
                    setNewWaypointTarget();
                    currentState = State.PATROL;
                }
                break;
            case State.PATROL:
                direction.x = Mathf.Sign(targetWaypoint.position.x - transform.position.x);
                if (canSeePlayer()) currentState = State.CHASE;
                
                float dx = Mathf.Abs(transform.position.x - targetWaypoint.position.x);
                if(dx <= 0.25f)
                {
                    currentState = State.IDLE;
                    //direction.x = 0;
                    //targetWaypoint = waypoints[Random.Range(0, waypoints.Length)];
                }
                break;
            case State.ATTACK:

                break;
            case State.CHASE:

                break;
            case State.DEAD:

                break;
        }


        // check if the character is on the ground
        bool onGround = UpdateGroundCheck();
        // get direction input
        
        // set velocity

        velocity.x = direction.x * speed;

        if (onGround)
        {
            if (velocity.y < 0) velocity.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
                StartCoroutine(DoubleJump());
                animator.SetTrigger("Jump");
            }
        }
        // adjust gravity for jump
        float gravityMultiplier = 1;
        if (!onGround && velocity.y < 0) gravityMultiplier = fallRateMultiplier;

        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        // move character

        rb.velocity = velocity;

        // face character direction of movement (velocity)
        if(velocity.x > 0 && !faceRight)
        {
            flip();
        }else if(velocity.x < 0 && faceRight)
        {
            flip();
        }

        // animator
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        animator.SetBool("Fall", !onGround && velocity.y < -0.1f);
    }

    private bool UpdateGroundCheck()
    {        // check if the character is on the ground        
             Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayerMask); 
        if (collider != null) 
        { 
            RaycastHit2D raycastHit = Physics2D.Raycast(groundTransform.position, Vector2.down, groundRadius, groundLayerMask); 
            if (raycastHit.collider != null) 
            { 
                // get the angle of the ground (angle between up vector and ground normal)
                groundAngle = Vector2.SignedAngle(Vector2.up, raycastHit.normal); 
                Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red); 
            } 
        }
        return (collider != null); 
    }
        //corouting
        IEnumerator DoubleJump()
    {
        // wait a little after the jump to allow a double jump
        yield return new WaitForSeconds(0.01f);
        // allow a double jump while moving up
        while (velocity.y > 0)
        {
            // if "jump" pressed add jump velocity
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y += Mathf.Sqrt(doubleJumpHeight * -2 * Physics.gravity.y);
                break;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundTransform.position, groundRadius);
    }

    private void flip()
    {
        faceRight = !faceRight;
        spriteRenderer.flipX = !faceRight;
    }

    private void setNewWaypointTarget()
    {
        Transform waypoint = targetWaypoint;
        while(waypoint == targetWaypoint)
        {
            waypoint = waypoints[Random.Range(0, waypoints.Length)];
        }
        targetWaypoint = waypoint;
    }

    private bool canSeePlayer()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance);
        Debug.DrawRay(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance);

        return raycastHit.collider != null && raycastHit.collider.gameObject.CompareTag("Player");
    }
}