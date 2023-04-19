using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool isInPlay = false;
    [SerializeField] bool devMode = false;
    [SerializeField] float launchMultiplier = 2;

    private void Update()
    {
        if(devMode)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Launch(500 * launchMultiplier);
            }
        }
        switch (isInPlay)
        {
            case true:

                break;
            case false:

                break;
        }
    }

    public void Launch(float LP) // Launch Power
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * LP);
    }

}
