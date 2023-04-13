using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float turnRate;
    [SerializeField] float jumpHeight;
    [SerializeField] float doubleJumpHeight;
    [SerializeField] float hitForce;
    [SerializeField] float attackRadius;
    [SerializeField, Range(1, 5)] float fallRateMultiplier; 
    [SerializeField, Range(1, 5)] float lowJumpRateMultiplier; 
    [Header("Ground")]
    [SerializeField] Transform groundTransform;
    [Header("Attack")]
    [SerializeField] Transform attackTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundRadius;
    Vector2 velocity = Vector2.zero;
    Rigidbody2D rb;
    [SerializeField]SpriteRenderer spriteRenderer;
    bool faceRight = true;
    float groundAngle = 0;

    public float health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer= GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // check if the character is on the ground
        bool onGround = UpdateGroundCheck() && (velocity.y <= 0);
        // get direction input
        Vector2 direction = Vector2.zero;
        direction.x = Input.GetAxis("Horizontal");
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
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack");
                CheckAttack();
            }
        }
        // adjust gravity for jump
        float gravityMultiplier = 1; 
        if (!onGround && velocity.y < 0) gravityMultiplier = fallRateMultiplier; 
        if (!onGround && velocity.y > 0 && !Input.GetButton("Jump")) gravityMultiplier = lowJumpRateMultiplier;

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
    {
        // check if the character is on the ground
        Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayerMask);
        if (collider != null)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(groundTransform.position, Vector2.down, groundRadius, groundLayerMask);
            if (raycastHit.collider != null)
            {
                // get the angle of the ground (angle between up vector and ground normal)
                groundAngle = Vector2.SignedAngle(Vector2.up, raycastHit.normal);
                Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red);
            }
        }

        return (collider != null);
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
    private void CheckAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackTransform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            if (collider.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.Damage(10);
            }
        }
    }

    
}