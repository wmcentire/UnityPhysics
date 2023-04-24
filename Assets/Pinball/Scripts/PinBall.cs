using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] Transform ballStart;
    [SerializeField] GameObject prefab;

    float timer = 0;
    game_state state;
    enum game_state
    {
        START,
        TITLE,
        LOSE,
        LAUNCH
    }
    
    public void setToStart()
    {
        timer = 2;
        state = game_state.START;
        SpawnBall();
    }

    public void SpawnBall()
    {
        Instantiate(prefab, ballStart);
    }

    private void Update()
    {
        switch(state)
        {
            case game_state.START:
                
                break;
            case game_state.TITLE:
                UI.enabled = true;
                break;
            case game_state.LOSE:

                break;
            case game_state.LAUNCH:

                break;
        }
    }
}
