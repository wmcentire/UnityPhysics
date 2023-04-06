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
    [SerializeField, Range(1, 5)] float fallRateMultiplier;
    [Header("Ground")]
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundLayerMask;
    Vector2 velocity = Vector2.zero;
    Rigidbody2D rb;
    [SerializeField]SpriteRenderer spriteRenderer;
    bool faceRight = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer= GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // check if the character is on the ground
        bool onGround = Physics2D.OverlapCircle(groundTransform.position, 0.2f, groundLayerMask) != null;
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
        }
        // adjust gravity for jump
        float gravityMultiplier = 1;
        if (!onGround && velocity.y < 0) gravityMultiplier = fallRateMultiplier;

        velocity.y += Physics.gravity.y * gravityMultiplier;

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

    private void flip()
    {
        faceRight = !faceRight;
        spriteRenderer.flipX = !faceRight;
    }
}