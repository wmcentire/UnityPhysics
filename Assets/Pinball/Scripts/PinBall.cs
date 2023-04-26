using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PinBall : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] Canvas StartScreenUI;
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI scoreboard;
    [SerializeField] Transform ballStart;
    [SerializeField] GameObject prefab;
    [SerializeField] bool devMode = false;

    private int score = 0;

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

    public void setScore(int score)
    {
        this.score += score;
    }

    public void resetScore()
    {
        this.score = 0;
    }

    public void setToStart()
    {
        if(state == game_state.START)
        {
            Debug.Log("no");
        }
        else
        {
            Debug.Log("yes");
            state = game_state.START;
            StartScreenUI.gameObject.SetActive(false);
            SpawnBall();
        }
    }

    public void setToTitle()
    {
        state = game_state.TITLE;
    }

    public void SpawnBall()
    {
        Instantiate(prefab, ballStart.position, ballStart.rotation);
    }

    private void Update()
    {
        scoreboard.text = "Score " + score.ToString();
        switch(state)
        {
            case game_state.START:
                if (devMode && Input.GetKeyDown(KeyCode.S))
                {
                    SpawnBall();
                }
                break;
            case game_state.TITLE:
                StartScreenUI.gameObject.SetActive(true);
                startButton.onClick.AddListener(setToStart);
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
