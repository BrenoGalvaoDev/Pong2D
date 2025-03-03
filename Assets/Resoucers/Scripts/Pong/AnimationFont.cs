using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class AnimationFont : MonoBehaviour
{
    public UnityEvent Method;
    public Text text;

    void Update()
    {
        //Method.Invoke();
    }

    //Called by animation
    public void UpdateColorScore()
    {
        text.color = new Color(Random.value, Random.value, Random.value, 1);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}
