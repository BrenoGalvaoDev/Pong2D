using UnityEngine;
using UnityEngine.Events;

public class ColliderScorePong : MonoBehaviour
{
    public UnityEvent events;
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            events.Invoke(); // called a GameController method ( between PlayerScored and OpponentScored)
            collision.gameObject.SetActive(false);
            audioSource.Play();
        }
    }
}
