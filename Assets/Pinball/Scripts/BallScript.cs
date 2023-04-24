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
    }

    public void Launch(float LP) // Launch Power
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * LP);
    }

}
