using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;
    [SerializeField] float waitTime = 2;
    [SerializeField] string tag;
    [SerializeField] PinBall manager;
    [SerializeField] Collider2D hitbox;
    [SerializeField] bool doActive = true;
    bool active = true;
    float timer = 0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if(manager != null && collision.gameObject.tag == tag && active)
        {
            Debug.Log("points" + hitPoints);
            manager.setScore(hitPoints);
            if (doActive)
            {
                active= false;
                timer = waitTime;
            }
        }
    }

    private void Update()
    {
        if(doActive)
        {
            if(!active)
            {
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    active = true;
                }
            }
        }
        else
        {
            active = true;
        }
    }
}
