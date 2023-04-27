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
    [SerializeField] TextMeshProUGUI ballcount;
    [SerializeField] TextMeshProUGUI startButtonTxT;
    [SerializeField] Transform ballStart;
    [SerializeField] GameObject prefab;
    [SerializeField] bool devMode = false;

    private int score = 0;
    private int ballCount = 3;

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
    /// <summary>
    /// add points to score
    /// </summary>
    /// <param name="score"></param>
    public void setScore(int score)
    {
        this.score += score;
    }
    /// <summary>
    /// set score to zero
    /// </summary>
    public void resetScore()
    {
        this.score = 0;
    }
    /// <summary>
    /// runs through the starting functions of the game
    /// </summary>
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
    /// <summary>
    /// spawns ball idk what else to say
    /// </summary>
    public void SpawnBall()
    {
        Instantiate(prefab, ballStart.position, ballStart.rotation);
    }

    public void ballLost()
    {
        ballCount--;
        if(ballCount < 0)
        {
            resetScore();
            setToTitle();
        }
        else
        {
            state = game_state.LAUNCH;
            startButtonTxT.text = "Launch Ball!";
        }
    }

    private void Update()
    {
        scoreboard.text = "Score " + score.ToString();
        ballcount.text = "Balls Left: " + ballCount.ToString();
        switch(state)
        {
            case game_state.START:
                if (devMode && Input.GetKeyDown(KeyCode.S))
                {
                    SpawnBall();
                }
                break;
            case game_state.TITLE:
                ballCount = 3;
                StartScreenUI.gameObject.SetActive(true);
                startButtonTxT.text = "Start Game!";
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    setToStart();
                }
                if (devMode && Input.GetKeyDown(KeyCode.S))
                {
                    SpawnBall();
                }
                break;
            case game_state.LOSE:

                break;
            case game_state.LAUNCH:
                Debug.Log("Launch state");
                StartScreenUI.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    setToStart();
                }
                break;
        }
    }
}
