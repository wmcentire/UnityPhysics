using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool isInPlay = false;
    [SerializeField] bool devMode = false;
    [SerializeField] float launchMultiplier = 2;
    [SerializeField] Transform startPosition;
    [SerializeField] float tiltMult = 1 ;

    private void Start()
    {
        Launch(700 * launchMultiplier);
    }
    private void Update()
    {
        if(devMode)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Launch(500 * launchMultiplier);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = startPosition.position;
                rb.velocity = Vector2.zero;
            }
        }

        //if (Input.GetKey(KeyCode.A))
        //{
        //    Vector2 addedForce = rb.velocity;
        //    addedForce.x = rb.velocity.x -tiltMult;
        //    rb.velocity = addedForce;
        //}
        //if(Input.GetKey(KeyCode.D))
        //{
        //    Vector2 addedForce = rb.velocity;
        //    addedForce.x = rb.velocity.x + tiltMult;
        //    rb.velocity = addedForce;
        //}
    }
    /// <summary>
    /// takes in a float and uses it to add force to the rigidbody2d
    /// </summary>
    /// <param name="LP"></param>
    public void Launch(float LP) // Launch Power
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * LP);
    }

}
