using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
            if(collision.GetComponent<RobotController>().isMoving == false)
            {
                transform.parent.gameObject.GetComponent<EndGoal>().RobotEnter(collision.gameObject);
                collision.GetComponent<RobotController>().SeverConnection();
            }
        }
    }
}
