using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] string tag;
    [SerializeField] PinBall manager;
    

    /// <summary>
    /// if object collision has the stored tag, destroy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == tag)
        {
            Destroy(collision.gameObject);
            manager.ballLost();
        }
    }
}
