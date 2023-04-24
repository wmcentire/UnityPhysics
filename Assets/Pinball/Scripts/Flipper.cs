using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] float flipForce = 20;

    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody2D>(out body);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            body.AddTorque(10000, ForceMode2D.Force);
        }
    }
}
