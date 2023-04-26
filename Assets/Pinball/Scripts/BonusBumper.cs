using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBumper : MonoBehaviour
{
    [SerializeField] int bumpPoints = 10;
    [SerializeField] float waitTime = 2;
    [SerializeField] string tag;
    [SerializeField] PinBall manager;
    [SerializeField] bool lit = false;
    [SerializeField] Color litColor;
    [SerializeField] Color unlitColor;
    [SerializeField] GameObject circle1;
    [SerializeField] GameObject circle2;
    [SerializeField] Collider2D hitbox;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(lit && collision.gameObject.tag == tag)
        {
            manager.setScore(bumpPoints);
            setUnlit();
        }
    }
    float timer = 0;
    public void setLit()
    {
        lit = true;
    }

    public void setUnlit()
    {
        lit = false;
        timer = waitTime;
    }

    private void Update()
    {
        if (lit)
        {
            //circle1.GetComponent<SpriteRenderer>().material.color = litColor;
            //circle2.GetComponent<SpriteRenderer>().material.color = litColor;
            hitbox.enabled = true;
        }
        else
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                setLit();
            }
            hitbox.enabled = false;
            //circle1.GetComponent<SpriteRenderer>().material.color = unlitColor;
            //circle2.GetComponent<SpriteRenderer>().material.color = unlitColor;
        }
    }
}
