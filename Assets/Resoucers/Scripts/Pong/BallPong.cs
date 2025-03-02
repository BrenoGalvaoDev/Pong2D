using UnityEngine;

public class BallPong : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TrailRenderer trail;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float speedMov;
    [SerializeField] Sprite[] imagens;


    bool isPaused = false;
    Vector2 savedVelocity;

    void Start()
    {
        LaunchBall();
    }

    //before releasing the ball, the position and velocity are reset to zero, your direction, color and velocity are randomized 
    public void LaunchBall()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;

        float movX = Random.Range(0, 2) == 0 ? -1 : 1;
        float movY = Random.Range(-1, 1);

        Color color = new Color(Random.value, Random.value, Random.value, 1f);
        sprite.color = color;
        sprite.sprite = imagens[(int)Random.value];
        trail.startColor = color;
        trail.startWidth = transform.localScale.x;
        trail.time = transform.localScale.x / 3;

        gameObject.SetActive(true);
        Vector2 direction = new Vector2(movX, movY).normalized;

        rb.linearVelocity = direction * speedMov;
    }

    // the rb.linearVelocity is saved when paused. and restored when return
    public void OnPauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            savedVelocity = rb.linearVelocity;

            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = savedVelocity;
        }
    }
}
