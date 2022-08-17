using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public AudioSource BGMusic;

    public List<GameObject> Robots;
    List<GameObject> DeadRobots;
    public EndGoal END;

    public LevelTransition LevelTransition;
    
    void Start()
    {
        Robots = new List<GameObject>();
        DeadRobots = new List<GameObject>();
        foreach (GameObject robot in GameObject.FindGameObjectsWithTag("Robot"))
        {
            Robots.Add(robot);
        }
    }

    public void StartReset()
    {
        LevelTransition.ResetLevel();
        for (int a = 0; a < Robots.Count; a++)
        {
            Robots[a].GetComponent<RobotController>().SeverConnection();
        }
    }
    public void LevelReset()
    {
        foreach (GameObject robot in DeadRobots)
        {
            Robots.Add(robot);
        }
        DeadRobots.Clear();
        for (int a = 0; a < Robots.Count; a++)
        {
            Robots[a].GetComponent<RobotController>().TeleportRobotToStart();
        }
        END.ResetNumberOfRobots();
    }

    public void RemoveRobot(GameObject robot)
    {
        DeadRobots.Add(robot);
        Robots.Remove(robot);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            foreach(GameObject robot in Robots)
                robot.GetComponent<RobotController>().GetUpInput(true);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            foreach (GameObject robot in Robots)
                robot.GetComponent<RobotController>().GetLeftInput(true);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            foreach (GameObject robot in Robots)
                robot.GetComponent<RobotController>().GetDownInput(true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (GameObject robot in Robots)
                robot.GetComponent<RobotController>().GetRightInput(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReset();
        }
    }
}
