using Unity.VisualScripting;
using UnityEngine;

public class EnemyPong : MonoBehaviour
{
    private enum States {Normal, Winning, Losing }

    [Header("Settings")]
    [SerializeField] float velocity; // used to multiply movement speed
    [SerializeField] float radius;
    [SerializeField] LayerMask layer;
    [SerializeField, Tooltip("first = winning, second = Losing, Third = Default --- in normal game")] float[] speeds = new float[] { 0.6f, 0.9f, 0.7f };



    private States states;
    bool isPaused = false;

    // Moves the enemy paddle towards the target position if it's not paused.
    // Uses Lerp to create smooth movement based on speed and game state.

    void Update()
    {
        Transform target = TargetPosition();
        if (target != null && !isPaused)
        {
            float newY = Mathf.Lerp(transform.position.y, target.position.y * Speed(), Time.deltaTime * velocity);
            transform.position = new Vector3(transform.position.x, newY, 0);
        }
    }


    // Checks for a target (the ball) within a defined radius.
    // Returns the target's transform if found, otherwise returns null.

    public Transform TargetPosition()
    {
        Collider2D circle = Physics2D.OverlapCircle(transform.position, radius, layer);

        return circle != null ? circle.transform : null;
    }

    // Toggles the pause state of the enemy paddle.
    // When paused, the enemy will stop moving.
    public void OnPauseGame()
    {
        isPaused = !isPaused;
    }


    // Updates the enemy's state based on the current score.
    // Winning if the enemy has more points, Losing if it has fewer, and Normal if tied.
    public void OnChangedPoints(int playerPoints, int enemyPoints)
    {
        if (playerPoints > enemyPoints)
        {
            states = States.Losing;
        }
        else if (playerPoints == enemyPoints)
        {
            states = States.Normal;
        }
        else
        {
            states = States.Winning;
        }
    }


    // Adjusts enemy difficulty based on the selected difficulty level.
    // Reduces speed and detection radius for easier mode, increases them for harder mode.
    public void OnChangedDifficulty(int diff)
    {
        switch (diff)
        {
            case 0:
                for (int i = 0; i < speeds.Length - 1; i++)
                {
                    speeds[i] -= 0.1f;
                }
                velocity--;
                radius--;
                break;

            case 2:

                for (int i = 0; i < speeds.Length - 1; i++)
                {
                    speeds[i] += 0.1f;
                }
                velocity++;
                radius++;
                break;

            default:
                break;
        }
    }

    // Returns the movement speed multiplier based on the enemy's current state.
    // Uses predefined speed values for winning, losing, and normal states.
    private float Speed()
    {

        if (speeds.Length < 3)
        {
            Debug.LogWarning("Speed array is not properly set. Returning default speed.");
            return 0.7f;
        }

        switch (states)
        {
            case States.Winning:
                return speeds[0];

            case States.Losing:
                return speeds[1];

            default:
                return speeds[2];
        }
    }

}