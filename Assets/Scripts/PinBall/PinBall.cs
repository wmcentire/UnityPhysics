using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] Canvas UI;

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
        state = game_state.START;
    }

    private void Update()
    {
        switch(state)
        {
            case game_state.START:

                break;
            case game_state.TITLE:
                UI.enabled= true;
                
                break;
            case game_state.LOSE:

                break;
            case game_state.LAUNCH:

                break;
        }
    }
}
