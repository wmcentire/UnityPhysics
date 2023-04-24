using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] Transform ballStart;
    [SerializeField] GameObject prefab;
    [SerializeField] bool devMode = false;

    float timer = 0;
    game_state state;
    enum game_state
    {
        START,
        TITLE,
        LOSE,
        LAUNCH
    }
    private void Start()
    {
        state = game_state.TITLE;
    }

    public void setToStart()
    {
        if(state == game_state.START)
        {

        }
        else
        {
            timer = 2;
            state = game_state.START;
            UI.enabled = false;
            SpawnBall();
        }
    }

    public void SpawnBall()
    {
        Instantiate(prefab, ballStart.position, ballStart.rotation);
    }

    private void Update()
    {
        switch(state)
        {
            case game_state.START:
                
                break;
            case game_state.TITLE:
                UI.enabled = true;
                if (devMode && Input.GetKeyDown(KeyCode.S))
                {
                    SpawnBall();
                }
                break;
            case game_state.LOSE:

                break;
            case game_state.LAUNCH:

                break;
        }
    }
}
