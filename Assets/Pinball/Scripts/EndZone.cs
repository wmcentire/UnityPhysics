using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] string tag;
    [SerializeField] PinBall manager;
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == tag)
        {
            Destroy(collision.gameObject);
            manager.setToTitle();
            manager.resetScore();
        }
    }
}
