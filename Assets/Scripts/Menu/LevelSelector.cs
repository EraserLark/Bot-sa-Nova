using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject CircleTransition;
    public Vector2 BigSize;

    int sceneNum;

    private void Start()
    {
        StartCoroutine(nameof(StartTransition));
    }

    public void OpenLevel(int num)
    {
        sceneNum = num;
        StopAllCoroutines();
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
                SceneManager.LoadScene(sceneNum);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator StartTransition()
    {
        CircleTransition.GetComponent<RectTransform>().sizeDelta = BigSize;
        float TimeSinceStarted = 0f;
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 2;
            CircleTransition.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(CircleTransition.GetComponent<RectTransform>().sizeDelta, Vector2.zero, TimeSinceStarted);
            if (CircleTransition.GetComponent<RectTransform>().sizeDelta == Vector2.zero)
            {
                CircleTransition.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }
}
