using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    #region SerializeField Components
    [Header("UI Components")]
    [SerializeField] Text scoreText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject loadPanel;

    [Space(10), Header("Components")]
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform parentBall;
    [SerializeField] PongEventsManager eventsManager;

    [Space(10), Header("Settings")]
    [SerializeField] int maxScore;
    [SerializeField] int maxBallsToPooling;

    //influences the spawn, and is directly related to the difficulty
    [SerializeField, Tooltip("first = Easy, second/default = Normal, Third = Hard")] float[] timeToSpawn = new float[3]; 

    int indexSpawn = 0; //is used to choose the next index in the List of balls pooling
    float spawn; // is used to change the time to spawn a balls in game, is directly related to the difficulty

    //ball settings
    float currentTime;
    List<GameObject> balls = new List<GameObject>();

    public bool pause { get; set; }
    bool gameOver = false;

    #endregion

    //events
    public delegate void PauseGame();
    public PauseGame OnPausedGame;

    public delegate void ChangedPoints(int playerPoint, int enemyPoint);
    public ChangedPoints OnChangedPoints;

    public delegate void ChangedDifficulty(int diff);
    public ChangedDifficulty OnChangedDifficulty;

    public static GameController instance;
    private void Awake()
    {
        instance = this;
        ObjectsPolling();
    }

    //Update Score text and add the balls to the EventMAnager List
    // the pause is true to not instatiate a ball in game, it is changed with the scena start button
    void Start()
    {
        UpdateScoreText();
        pause = true;
        foreach (GameObject ball in balls)
        {
            eventsManager.balls.Add(ball.GetComponent<BallPong>());
        }
    }

    private void Update()
    {
        AddTime();
        if (gameOver)
        {
            foreach (GameObject ball in balls)
            {
                ball.SetActive(false);
            }
        }
    }

    #region Functions

    //Event called to pause game. player, opponent and balls depends on method
    public void PausedGame()
    {
        if (OnPausedGame != null)
        {
            OnPausedGame();
            pause = !pause;
        }
    }

    //Load the scene in async mode, and open loading panel before the scene is fully loaded
    public void LoadGame(string scene)
    {
        loadPanel.SetActive(true);
        SceneManager.LoadSceneAsync(scene);
    }

    IEnumerator ResetBall()
    {
        yield return new WaitForSeconds(2);
        SpawnBall();
    }

    //Instantiate other balls in given period, and is called in start button
    public void AddTime()
    {
        if (!pause && !gameOver) //are two bools to block a instance after game ends
        {
            currentTime += Time.deltaTime;

            if (currentTime > spawn)
            {
                currentTime = 0;

                SpawnBall();
            }
        }
    }

    //lock the transform, give it a random size and launch it into the game
    public void SpawnBall()
    {
        GameObject ball = balls[indexSpawn];
        indexSpawn = (indexSpawn + 1) % balls.Count;

        float rand = Random.Range(0.2f, 1);
        ball.transform.localScale = new Vector3(rand, rand, 0);
        ball.SetActive(true);
        ball.GetComponent<BallPong>().LaunchBall();
    }

    //Instantiate a ball prefab, add it to the list
    public void ObjectsPolling()
    {
        for (int i = 0; i < maxBallsToPooling; i++)
        {
            GameObject ball = Instantiate(ballPrefab, parentBall);
            ball.transform.SetParent(parentBall);
            balls.Add(ball);
            ball.SetActive(false);
        }
    }

    //Called by button in scene
    //Change the spawn time
    public void BallSpawnTime(int i)
    {
        OnChangedDifficulty(i);
        switch (i)
        {
            case 0:
                spawn = timeToSpawn[0];
                break;

            case 2:
                spawn = timeToSpawn[2];
                break;

            default:
                spawn = timeToSpawn[1];
                break;
        }
    }
    #endregion

    #region Score System

    //if any method are called, the respective method change the number of point. and update the text in scene

    int playerScore = 0;
    int opponentScore = 0;

    public void PlayerScored()
    {
        if (!gameOver)
        {
            playerScore++;

            if (playerScore < maxScore)
            {
                UpdateScoreText();
                StartCoroutine(ResetBall());
                OnChangedPoints(playerScore, opponentScore);
                if (playerScore == opponentScore && playerScore == (maxScore - 1))
                {
                    maxScore += 2;
                }
            }
            else
            {
                UpdateScoreText();
                winPanel.SetActive(true);
                gameOver = true;
            }
        }
    }

    public void OpponentScored()
    {
        if (!gameOver)
        {
            opponentScore++;

            if (opponentScore < maxScore)
            {
                UpdateScoreText();
                StartCoroutine(ResetBall());
                OnChangedPoints(playerScore, opponentScore);
                if (playerScore == opponentScore && playerScore == (maxScore - 1))
                {
                    maxScore += 2;
                }
            }
            else
            {
                UpdateScoreText();
                losePanel.SetActive(true);
                gameOver = true;
            }
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = playerScore.ToString() + " : " + opponentScore.ToString();
    }

    #endregion
}
