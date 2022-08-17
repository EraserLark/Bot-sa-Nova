using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryNav : MonoBehaviour
{
    public GameObject CircleTransition;
    public Vector2 BigSize;

    public void GoToMenu()
    {
        StartCoroutine(nameof(Transition));
    }

    IEnumerator Transition()
    {
        float TimeSinceStarted = 0f;
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 2;
            CircleTransition.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(CircleTransition.GetComponent<RectTransform>().sizeDelta, BigSize, TimeSinceStarted);
            if (CircleTransition.GetComponent<RectTransform>().sizeDelta.x >= BigSize.x - 1)
            {
                SceneManager.LoadScene("TitleScreen");
                yield break;
            }
            yield return null;
        }
    }
}
