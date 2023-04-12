using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class AIController2D : MonoBehaviour, IDamagable
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
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] float groundRadius;
    [SerializeField] string enemyTag;
    Vector2 velocity = Vector2.zero;
    Rigidbody2D rb;
    [SerializeField]SpriteRenderer spriteRenderer;
    [SerializeField] Transform[] waypoints;
    [Header("AI")]
    [SerializeField] float rayDistance = 1;
    bool faceRight = true;
    float groundAngle = 0;
    Transform targetWaypoint = null;
    GameObject enemy = null;
    public int health = 100;
    enum State
    {
        IDLE,
        PATROL,
        ATTACK,
        CHASE,
        DEAD
    }

    State currentState = State.IDLE;
    float stateTimer = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer= GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector2 direction = Vector2.zero;
        targetWaypoint = waypoints[Random.Range(0, waypoints.Length)];

        CheckEnemySeen();
        // update ai
        switch (currentState)
        {
            case State.IDLE:
                if (enemy != null) currentState = State.CHASE;

                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    setNewWaypointTarget();
                    currentState = State.PATROL;
                }
                break;
            case State.PATROL:
                {
                    if (enemy != null) currentState = State.CHASE;
                    direction.x = Mathf.Sign(targetWaypoint.position.x - transform.position.x);

                    float dx = Mathf.Abs(targetWaypoint.position.x - transform.position.x );
                    if (dx <= 0.25f)
                    {
                        currentState = State.IDLE;
                        stateTimer = 1;
                    }
                }
                break;
            case State.ATTACK:
                // wait until animation is over, then set state
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    //check to see if the player is visible, then change states accordingly.
                    if (enemy != null)
                    {
                        currentState = State.CHASE;
                    }
                    else
                    {
                        currentState = State.IDLE;
                    }
                }
                break;
            case State.CHASE:
                {
                    // check if the player is still seen
                    if (enemy == null)
                    {
                        currentState = State.IDLE;
                        stateTimer = 1;
                        break;
                    }

                    // move in direction of player
                    float dx = Mathf.Abs(enemy.transform.position.x - transform.position.x);
                    if (dx <= 1f)
                    {
                        // initiates attack animation
                        currentState = State.ATTACK;
                        animator.SetTrigger("Attack");
                    }
                    else
                    {
                        // if not in range moves closer
                        direction.x = Mathf.Sign(enemy.transform.position.x - transform.position.x);
                    }
                }
                break;
            case State.DEAD:

                break;
        }


        // check if the character is on the ground
        bool onGround = UpdateGroundCheck();
        // get direction input
        
        // set velocity

        velocity.x = direction.x * speed;

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
        animator.SetBool("Falling", !onGround && velocity.y < -0.1f);
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

    private void CheckEnemySeen()
    {
        enemy = null;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.right : Vector2.left), rayDistance, raycastLayerMask);
        if (raycastHit.collider != null && raycastHit.collider.gameObject.CompareTag(enemyTag))
        {
            enemy = raycastHit.collider.gameObject;
            Debug.DrawRay(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance, Color.red);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        print(health);
    }
}