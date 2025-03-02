using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPong : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    Vector2 movement;

    [Header("Settings"), Space(10)]
    [SerializeField] float movSpeed;

    bool isPaused = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    //RigidBody2D.linearVelocity is the new form to RigidBody2D.velocity in Unity 6
    public void SetMovementInput(InputAction.CallbackContext value)
    {
        if (!isPaused)
        {
            movement = value.ReadValue<Vector2>();
            rb.linearVelocity = new Vector2(0, movement.y * movSpeed);
        }
    }
    // I took advantage the input system, and I used the Escape key to call this method
    public void SetPauseGameInput(InputAction.CallbackContext value)
    {
        GameController.instance.PausedGame();
        isPaused = !isPaused;
    }
}
