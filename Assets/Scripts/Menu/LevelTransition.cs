using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public Vector2 BigSize;
    public InputController INPUT;
    public EndGoal END;

    public void Start()
    {
        StartCoroutine(nameof(OpeningTransition));
    }
    IEnumerator OpeningTransition()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = BigSize;
        float TimeSinceStarted = 0f;
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 2;
            gameObject.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(gameObject.GetComponent<RectTransform>().sizeDelta, Vector2.zero, TimeSinceStarted);
            if (gameObject.GetComponent<RectTransform>().sizeDelta == Vector2.zero)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }

    public void ResetLevel()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(ResetTransiton));
    }
    IEnumerator ResetTransiton()
    {
        float TimeSinceStarted = 0f;
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 2;
            gameObject.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(gameObject.GetComponent<RectTransform>().sizeDelta, BigSize, TimeSinceStarted);
            if (gameObject.GetComponent<RectTransform>().sizeDelta.x >= BigSize.x - 1)
            {
                INPUT.LevelReset();
                StartCoroutine(nameof(OpeningTransition));
                yield break;
            }
            yield return null;
        }
    }

    public void LoadNextScene()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(NextSceneTransition));
    }
    IEnumerator NextSceneTransition()
    {
        float TimeSinceStarted = 0f;
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 2;
            gameObject.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(gameObject.GetComponent<RectTransform>().sizeDelta, BigSize, TimeSinceStarted);
            if (gameObject.GetComponent<RectTransform>().sizeDelta.x >= BigSize.x - 1)
            {
                END.LoadScene();
                yield break;
            }
            yield return null;
        }
    }
}
