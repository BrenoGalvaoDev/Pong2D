using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PongEventsManager : MonoBehaviour
{
    [SerializeField] GameController controller;
    [SerializeField] EnemyPong enemy;

    public List<BallPong> balls { get; set; } = new List<BallPong>();

    public static PongEventsManager instance;
    private void Start()
    {
        instance = this;

        controller.OnPausedGame += enemy.OnPauseGame;
        controller.OnChangedPoints += enemy.OnChangedPoints;
        controller.OnChangedDifficulty += enemy.OnChangedDifficulty;

        foreach (BallPong ball in balls)
        {
            controller.OnPausedGame += ball.OnPauseGame;
        }
    }
}
