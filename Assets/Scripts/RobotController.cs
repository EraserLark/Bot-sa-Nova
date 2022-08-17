using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Animator anim;
    public Camera mainCamera;
    private string currentState;

    //public string[] robotName = { "Blue", "Brown", "Green", "Orange", "Purple" };   //Set to enum instead for dropdown
    public string robotName;    //i.e. Blue, Brown, Orange, etc. (Change to int robotNum ?)
    public GameObject victoryAnim;
    const string idleAnim = "_Idle";
    const string moveAnim = "_Move";
    const string bumpAnim = "_Bump";
    //const string victoryAnim = "_Victory";
    IEnumerator currentBumpCoroutine;
    Vector3 rotRight;
    Vector3 rotLeft; 

    //Audio
    AudioSource source;
    public AudioClip stepNoise;
    public AudioClip bumpNoise;
    public AudioClip robotVictoryNoise;
    public AudioClip connectNoise;
    public AudioClip disconnectNoise;

    // Movement
    Vector3 startingPosition;
    bool conectedFirst = false;

    public bool Up;
    public bool Right;
    public bool Down;
    public bool Left;

    public bool isMoving;
    public float moveSpeed = 10f;   //How fast the player moves after the movepoint
    public Transform movePoint;     //Point that moves along the grid, Player character follows this
    float overlapRadius = 0.2f;
    //public Transform testPoint;

    // Conection
    public GameObject buddy;
    bool seekingConnection;

    // Later Implements
    public LayerMask stopMovement;  //Objs on this layer will stop the player's movement completely (Robots should be on diff layer)
    public LayerMask pushable;  //Put objs on this layer that can be pushed??

    void Start()
    {
        /// MOVEMENT ///
        startingPosition = transform.position;
        Physics2D.queriesStartInColliders = false;  //??
        movePoint.parent = null; //Do this to avoid issues with the movePoint being a child otherwise
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        /// ANIMATION ///
        ChangeAnimationState(idleAnim);

        rotRight = transform.position + new Vector3(1, 1, 0);
        rotLeft = transform.position + new Vector3(-1, -1, 0);

        /// AUDIO ///
        source = GetComponent<AudioSource>();
    }

    #region MovementInputs
    //Up
    public void GetUpInput(bool fromInput)
    {
        if ((Up || !fromInput) && Vector3.Distance(transform.position, movePoint.position) <= 0.25f)   //Prevents input while too far while moving
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), overlapRadius, stopMovement))
            {
                isMoving = true;
                movePoint.position += new Vector3(0f, 1f, 0f);

                /// ANIMATION ///
                ChangeAnimationState(moveAnim);
                PlayAudio(stepNoise);
            }
            else
            {
                PlayAudio(bumpNoise);

                //Plays little bump animation by rotating the sprite back and forth
                ChangeAnimationState(bumpAnim);
            }

            if (buddy != null && fromInput)
                buddy.GetComponent<RobotController>().GetUpInput(false);
        }
    }

    //Left
    public void GetLeftInput(bool fromInput)
    {
        if ((Left || !fromInput) && Vector3.Distance(transform.position, movePoint.position) <= 0.25f)   //Prevents input while too far while moving
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), overlapRadius, stopMovement))
            {
                isMoving = true;
                movePoint.position += new Vector3(-1f, 0f, 0f);

                /// ANIMATION ///
                ChangeAnimationState(moveAnim);
                PlayAudio(stepNoise);
            }
            else
            {
                PlayAudio(bumpNoise);

                //Plays little bump animation by rotating the sprite back and forth
                ChangeAnimationState(bumpAnim);
            }

            if (buddy != null && fromInput)
                buddy.GetComponent<RobotController>().GetLeftInput(false);
        }
    }

    //Down
    public void GetDownInput(bool fromInput)
    {
        if ((Down || !fromInput) && Vector3.Distance(transform.position, movePoint.position) <= 0.25f)   //Prevents input while too far while moving
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), overlapRadius, stopMovement))
            {
                isMoving = true;
                movePoint.position += new Vector3(0f, -1f, 0f);

                /// ANIMATION ///
                ChangeAnimationState(moveAnim);
                PlayAudio(stepNoise);
            }
            else
            {
                PlayAudio(bumpNoise);

                //Plays little bump animation by rotating the sprite back and forth
                ChangeAnimationState(bumpAnim);
            }

            if (buddy != null && fromInput)
                buddy.GetComponent<RobotController>().GetDownInput(false);
        }
    }

    //Right
    public void GetRightInput(bool fromInput)
    {
        if ((Right || !fromInput) && Vector3.Distance(transform.position, movePoint.position) <= 0.25f)   //Prevents input while too far while moving
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), overlapRadius, stopMovement))
            {
                isMoving = true;
                movePoint.position += new Vector3(1f, 0f, 0f);

                /// ANIMATION ///
                ChangeAnimationState(moveAnim);
                PlayAudio(stepNoise);
            }
            else
            {
                PlayAudio(bumpNoise);

                //Plays little bump animation by rotating the sprite back and forth
                ChangeAnimationState(bumpAnim);
            }

            if (buddy != null && fromInput)
                buddy.GetComponent<RobotController>().GetRightInput(false);
        }
    }

    //For restarting the level
    public void TeleportRobotToStart()
    {
        gameObject.transform.position = startingPosition;
        movePoint.position = startingPosition;
    }

    //For touching victory
    public void TeleportRobot(Vector3 position)
    {
        var vicAnim = Instantiate(victoryAnim);
        vicAnim.transform.position = gameObject.transform.position;

        PlayAudio(robotVictoryNoise);   //Audio

        gameObject.transform.position = position;
        movePoint.position = position;
    }
    #endregion

    #region Wires&Conecting
    public void SetBuddy(GameObject botBuddy)
    {
        buddy = botBuddy;
        seekingConnection = false;

        PlayAudio(connectNoise); //Audio
    }
    public void SeverConnection()
    {
        if (buddy != null)
        {
            gameObject.GetComponent<LineRenderer>().SetPosition(0, Vector2.zero);
            gameObject.GetComponent<LineRenderer>().SetPosition(1, Vector2.zero);

            buddy.GetComponent<LineRenderer>().SetPosition(0, Vector2.zero);
            buddy.GetComponent<LineRenderer>().SetPosition(1, Vector2.zero);

            buddy.GetComponent<RobotController>().conectedFirst = false;
            buddy.GetComponent<RobotController>().buddy = null;

            conectedFirst = false;
            buddy = null;
            
            PlayAudio(disconnectNoise); //Audio
        }
    }

    public void OnMouseDown()
    {
        seekingConnection = true;
        SeverConnection();
    }

    public void OnMouseUp()
    {
        seekingConnection = false;
        gameObject.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
    }

    private void OnMouseOver()
    {
        foreach (GameObject robot in GameObject.FindGameObjectsWithTag("Robot"))
        {
            if (robot.GetComponent<RobotController>().seekingConnection &&
                robot != gameObject)
            {
                SeverConnection();
                robot.GetComponent<RobotController>().SetBuddy(gameObject);
                SetBuddy(robot);

                buddy.GetComponent<RobotController>().conectedFirst = true;
            }
        }
    }
    #endregion

    void Update()
    {
        /// MOVEMENT ///
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);   //Moves player itself

        if (transform.position == movePoint.position)
        {
            isMoving = false;
        }

        if (buddy != null) 
        {
            if (buddy.transform.position != buddy.GetComponent<RobotController>().movePoint.position || transform.position != movePoint.position)
            { 
                if (conectedFirst)
                {
                    gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                    gameObject.GetComponent<LineRenderer>().SetPosition(1, buddy.transform.position);
                }
                else
                {
                    buddy.GetComponent<LineRenderer>().SetPosition(0, buddy.transform.position);
                    buddy.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                }
            }
        }

        /// WIRES & CONNECTING ///
        if (buddy != null)
        {
            if (true)//!isMoving)
            {
                RaycastHit2D hit = Physics2D.Raycast(movePoint.position, (buddy.GetComponent<RobotController>().movePoint.position - movePoint.position),
                    Vector3.Distance(buddy.transform.position, transform.position));
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
                {
                    SeverConnection();
                }
            }
        }

        if (seekingConnection)
        {
            gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            gameObject.GetComponent<LineRenderer>().SetPosition(1, mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void ChangeAnimationState(string newState)
    {
        //if(currentState == newState) return;

        anim.Play(robotName + newState);

        currentState = newState;
    }

    void PlayAudio(AudioClip audio)
    {
        source.PlayOneShot(audio);
    }
}
