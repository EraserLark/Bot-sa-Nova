using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour
{
    int NextSceneNum;

    public InputController INPUT;
    public LevelTransition TRANSITION;

    public int NumberOfRobots;
    int startingAmount;
    public GameObject WinSplashScreen;

    public Vector3 OffScreen;
    public Vector3 VictoryScreenSize;

    public AudioClip sweetVictory;
    AudioSource source;

    private void Start()
    {
        WinSplashScreen.SetActive(false);
        startingAmount = NumberOfRobots;
        source = GetComponent<AudioSource>();
        VictoryScreenSize = WinSplashScreen.GetComponent<RectTransform>().sizeDelta;
    }

    public void RobotEnter(GameObject robot)
    {
        INPUT.RemoveRobot(robot);
        NumberOfRobots--;
        robot.GetComponent<RobotController>().TeleportRobot(OffScreen);
        if (NumberOfRobots <= 0)
            WinScreen();
    }

    public void ResetNumberOfRobots()
    {
        NumberOfRobots = startingAmount;
        WinSplashScreen.SetActive(false);
    }

    private void WinScreen()
    {
        StartCoroutine(nameof(VictoryScreen));
    }

    IEnumerator VictoryScreen()
    {
        float TimeSinceStarted = 0f;

        // Splash screen
        WinSplashScreen.SetActive(true);
        WinSplashScreen.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        // Children
        foreach (Transform child in WinSplashScreen.transform)
            child.gameObject.SetActive(false);
        
        while (true)
        {
            TimeSinceStarted += Time.deltaTime / 4;

            // Spash screen
            WinSplashScreen.GetComponent<RectTransform>().sizeDelta =
                Vector2.Lerp(WinSplashScreen.GetComponent<RectTransform>().sizeDelta, VictoryScreenSize, TimeSinceStarted);

            if (WinSplashScreen.GetComponent<RectTransform>().sizeDelta.x >= VictoryScreenSize.x - 1)
            {
                WinSplashScreen.GetComponent<RectTransform>().sizeDelta = VictoryScreenSize;
                source.PlayOneShot(sweetVictory);

                foreach (Transform child in WinSplashScreen.transform)
                    child.gameObject.SetActive(true);
                
                yield break;
            }
            yield return null;
        }
    }

    public void NextLevel()
    {
        NextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        if (NextSceneNum > 10)
            Debug.Log("Next scene does not exist");
        TRANSITION.LoadNextScene();
    }

    public void TitleScreen()
    {
        NextSceneNum = 0;
        TRANSITION.LoadNextScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(NextSceneNum);
    }
}
