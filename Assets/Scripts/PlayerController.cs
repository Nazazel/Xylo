using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //Stuff from the Unity environment
    public float force;
    public float jumpHeight;
    private Rigidbody2D rb;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
	public Image controlsImage; 

    //Movement
    public bool finishedJump;
    public bool canMove;
    public bool moving;
    private bool crouch;
    private float scalD;
    private float currD;
    private bool sprint;
    private float airMult;
    private readonly float WALK_STARTSPEED = 0.6f;
    private readonly float WALK_SPEEDCAP = 4.5f;
    private readonly float RUN_SPEEDCAP = 9.0f;
    private readonly float SPR_MULT = 1.8f;

    //Ladder movement
    private bool hasLadder = false;
    private bool onLadder = false;
    private bool hasUpL = false;
    private bool hasDownL = false;
    private float[] ladderBounds;
    private readonly float ON_OFF_VARIANCE_TOP = 0.16f;
    private readonly float ON_OFF_VARIANCE_BOT = 0.6f;
    private float climbSpeed;
    private bool lCoolReady = true;
    private readonly float COOL_TIME = 0.4f;
    private readonly float MAX_SNAP_DISTANCE = 0.36f; //So that the player snaps to the center of the ladder, but a jump that is too large seems unnatural

    //Appearance, death, respawning, other miscellaneous player variables
    public bool hasSuit;
    public static bool isTouchingSuit = false;
    public bool suffocated;
    public bool isAlive;
    private float xSpawn;
    private float ySpawn;
    public bool isAwake;
    public bool awakeSequenceStarted;
	public bool loading;
	public bool gameEnd;
	public bool endSequenceStarted;

    //Items and inventory
    private bool groundItem;
    private int gItemID;
    private GameObject currItem;
    private GameObject inv;

    //Hints
    public bool activeHint;
    public GameObject hintBox;
    public Text hintText;
    public int currentObjective;
    
    //Death timer
    private float tTime;

    //
	private string inventoryString;
    public string[] obtainedObj;
	public float fadeSpeed = 1.5f;

    //Objectives
	public bool deadCrew;
	public int numDead;
	public bool commsCenterInit;
	public bool keyCards;
	public int numKeys;
	public bool accessCommsCenter;
	public bool commandCenter;
	public bool manual;
	public bool engineeringItems;
	public bool medicItems;
	public bool engineItems;
	public bool commsCenterFinal;
	public bool repairComms;

    //Fading
	public Image FadeImg;
	public bool introDone;
	public SpriteRenderer glow;
	public SpriteRenderer darkness;
	public Image AlarmUI;
	public bool bounce;
	public bool alarmIsStarted = false;


    //Spawning
    private Vector3[] lvCoords;
    private int nextScene;
    public bool firstSpawnInLV1 = true;

    void Start()
    {
		introDone = false;
		gameEnd = false;
		endSequenceStarted = false;
		GameObject.DontDestroyOnLoad (GameObject.FindWithTag("Full Player"));
		loading = false;
        isAlive = true;
		isAwake = false;
		deadCrew = false;
		numDead = 0;
		keyCards = false;
		numKeys = 0;
		awakeSequenceStarted = false;
		moving = false;
		hasSuit = false;
        canMove = true;
        crouch = false;
        finishedJump = true;
        rb = GetComponent<Rigidbody2D>();
        scalD = -0.011f;
        currD = 0f;
        lvCoords = new Vector3[6];
        groundItem = false;
        gItemID = 0;
        currItem = null;
        inv = GameObject.Find("Inventory Slots");
		glow = GameObject.Find ("Glowstick-Glow3").GetComponent<SpriteRenderer> ();
		glow.color = new Color(0,0,0,0);
		darkness = GameObject.Find("BlackBG").GetComponent<SpriteRenderer> ();
		playerAnimator.Play ("StellaStand");
		currentObjective = 0;
		hintBox = GameObject.FindWithTag ("HintBox");
		hintText = hintBox.GetComponentInChildren<Text> ();
		hintBox.SetActive (false);
		activeHint = true;
		obtainedObj = new string[8];
		obtainedObj [0] = "Power Drill";
		obtainedObj [1] = "Wrench";
		obtainedObj [2] = "Switchblade";
		obtainedObj [3] = "Hammer";
		obtainedObj [4] = "Saw";
		obtainedObj [5] = "Blow Torch";
		obtainedObj [6] = "Screw Driver";
		obtainedObj [7] = "Wire Cutters";
		bounce = false;

        climbSpeed = 0.06f;

		FadeImg = GameObject.Find ("Fade").GetComponent<Image>();
		controlsImage = GameObject.Find ("ControlsImage").GetComponent<Image>();
		controlsImage.color = Color.clear;
		InvokeRepeating ("FadeToClear", 0.0f, 0.1f);

        tTime = 0;

	}

	void Update()
	{
		if (isAwake && !gameEnd) {
			if (Input.GetKeyUp (KeyCode.Tab) && !activeHint && !loading && introDone) {
				canMove = false;
				activeHint = true;
				FadeImg.color = new Color (1, 1, 1, 0.5f);
				controlsImage.color = Color.white;
				rb.velocity = new Vector2 (0, rb.velocity.y);
				if (!hasSuit) {
					playerAnimator.Play ("StellaStand");
				} else {
					playerAnimator.Play ("SpaceStand");
				}
			} else if (Input.GetKeyUp (KeyCode.Tab) && activeHint && !loading && introDone) {
				canMove = true;
				activeHint = false;
				FadeImg.color = Color.clear;
				controlsImage.color = Color.clear;
			}
		}
	}

	void FixedUpdate ()
	{
		if (SceneManager.GetActiveScene ().name == "Credits") {
			DestroyImmediate (GameObject.FindWithTag ("Full Player"));
		}

		if (hintBox == null && !gameEnd) {
			loading = false;
			activeHint = false;
			if (hasSuit) {
				playerAnimator.Play ("SpaceStand");
			} else {
				playerAnimator.Play ("StellaStand");
			}
			FadeImg = GameObject.Find ("Fade").GetComponent<Image>();
			FadeImg.color = Color.black;
			InvokeRepeating ("FadeToClear", 0.0f, 0.1f);
			hintBox = GameObject.FindWithTag ("HintBox");
			hintText = hintBox.GetComponentInChildren<Text> ();
			hintBox.SetActive (false);
			controlsImage = GameObject.Find ("ControlsImage").GetComponent<Image>();
			controlsImage.color = Color.clear;
		}

		if (gameEnd && !endSequenceStarted) {
			endSequenceStarted = true;
			canMove = false;
			activeHint = true;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			StartCoroutine ("endGame");
		}

		if (isAwake && !gameEnd)
        {

			if (Input.GetKeyDown(KeyCode.H) && !activeHint && !onLadder)
            {
				if (playerAnimator.speed == 0) {
					playerAnimator.speed = 1;
				}

                rb.velocity = new Vector2(0, rb.velocity.y);
                if (!hasSuit)
                {
                    playerAnimator.Play("StellaStand");
                }
                else
                {
                    playerAnimator.Play("SpaceStand");
                }
                StartCoroutine("displayHint");
                activeHint = true;
            }

            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && activeHint == false && !onLadder)
            {
				if (playerAnimator.speed == 0) {
					playerAnimator.speed = 1;
				}

                if (isAlive)
                {
                    canMove = false;
                    crouch = true;
                    if (!hasSuit)
                    {
                        playerAnimator.Play("StellaCrouching");
                    }
                    else
                    {
                        playerAnimator.Play("SpaceCrouch");
                    }
                }
            }
            else
            {
                if (isAlive && activeHint == false)
                {
                    canMove = true;
                    crouch = false;
                }
            }

            //Ladder management code
            if (onLadder)
            {
                if (gameObject.transform.position.y > ladderBounds[0])
                    hasDownL = true;
                else hasDownL = false;
                if (gameObject.transform.position.y < ladderBounds[1])
                    hasUpL = true;
                else hasUpL = false;
            }

            //Movement
            if (!onLadder)
            {
				playerAnimator.speed = 1;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    sprint = true;
                }
                else
                {
                    sprint = false;
                }

                if (finishedJump)
                {
                    airMult = 1.0f;
                }
                else
                {
                    airMult = 0.5f;
                }

				if (canMove && !activeHint)
                {
                    //Move right
                    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    {
                        //Causes instant stop if character is moving left
                        if (rb.velocity.x < 0)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                        playerRenderer.flipX = true;
						glow.flipX = true;
                        moving = true;
                        if (sprint)
                        {
                            if (rb.velocity.x < RUN_SPEEDCAP)
                            {
                                rb.AddForce(new Vector2(SPR_MULT * airMult * force, 0));
                            }
                            else
                            {
                                rb.velocity = new Vector2(RUN_SPEEDCAP, rb.velocity.y);
                            }

                            //Animation
                            if (finishedJump)
                            {
                                if (!hasSuit)
                                {
                                    playerAnimator.Play("StellaRunning");
                                }
                                else
                                {
                                    playerAnimator.Play("SpaceRun");
                                }
                            }
                        }
                        else
                        {
                            if (rb.velocity.x < WALK_SPEEDCAP)
                            {
                                rb.AddForce(new Vector2(airMult * force, 0));
                            }
                            else
                            {
                                rb.velocity = new Vector2(WALK_SPEEDCAP, rb.velocity.y);
                            }

                            //Animation
                            if (finishedJump)
                            {
                                if (!hasSuit)
                                {
                                    playerAnimator.Play("StellaWalking");
                                }
                                else
                                {
                                    playerAnimator.Play("SpaceWalk");
                                }
                            }
                        }

                        //This line checks to see if the speed in the x direction is below a certain value. If it is, it sets the velocity.
                        //This helps to make movement a bit more responsive but still smooth.
                        if (checkV())
                            rb.velocity = new Vector2(WALK_STARTSPEED, rb.velocity.y);
                    }

                    //Move left
                    else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    {
                        //Causes instant stop if character is moving right
                        if (rb.velocity.x > 0)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                        playerRenderer.flipX = false;
						glow.flipX = false;
                        moving = true;
                        if (sprint)
                        {
                            if (rb.velocity.x > -RUN_SPEEDCAP)
                            {
                                rb.AddForce(new Vector2(-SPR_MULT * airMult * force, 0));
                            }
                            else
                            {
                                rb.velocity = new Vector2(-RUN_SPEEDCAP, rb.velocity.y);
                            }

                            //Animation
                            if (finishedJump)
                            {
                                if (!hasSuit)
                                {
                                    playerAnimator.Play("StellaRunning");
                                }
                                else
                                {
                                    playerAnimator.Play("SpaceRun");
                                }
                            }
                        }
                        else
                        {
                            if (rb.velocity.x > -WALK_SPEEDCAP)
                            {
                                rb.AddForce(new Vector2(-airMult * force, 0));
                            }
                            else
                            {
                                rb.velocity = new Vector2(-WALK_SPEEDCAP, rb.velocity.y);
                            }

                            //Animation
                            if (finishedJump)
                            {
                                if (!hasSuit)
                                {
                                    playerAnimator.Play("StellaWalking");
                                }
                                else
                                {
                                    playerAnimator.Play("SpaceWalk");
                                }
                            }
                        }

                        if (checkV())
                            rb.velocity = new Vector2(-WALK_STARTSPEED, rb.velocity.y);

                    }
                    else
                    {
                        moving = false;
                    }

                    //Jump code
                    if (Input.GetKey(KeyCode.Space) && finishedJump)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                        if (!hasSuit)
                        {
                            playerAnimator.Play("StellaJumping");
                        }
                        else
                        {
                            playerAnimator.Play("SpaceJump");
                        }
                        finishedJump = false;
                    }
                }
            }

            //Ladder key input
            else
            {
				rb.velocity = new Vector2 (0, 0);
				if (!hasSuit) {
					playerAnimator.Play ("StellaClimbing");
				} else {

					playerAnimator.Play ("SpaceClimb");
				}

				if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) && hasUpL) {
					rb.position += new Vector2 (0, climbSpeed);
					if (!hasSuit) {
						playerAnimator.speed = 2;
						playerAnimator.Play ("StellaClimbing");
					} else {
						playerAnimator.speed = 2;
						playerAnimator.Play ("SpaceClimb");
					}

				} else if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && hasDownL) {
					rb.position += new Vector2 (0, -climbSpeed);
					if (!hasSuit) {
						playerAnimator.speed = 2;
						playerAnimator.Play ("StellaClimbing");
					} else {
						playerAnimator.speed = 2;
						playerAnimator.Play ("SpaceClimb");
					}
				} else {
					if (!hasSuit) {
						playerAnimator.speed = 0;
					} else {
						playerAnimator.speed = 0;
					}
				}

            }

            //Increases falling rate
            if (!finishedJump && !onLadder)
            {
                currD += scalD;
                if (currD < -2)
                    currD = -5;
                rb.velocity += new Vector2(0, currD);
            }
            if (onLadder) currD = 0f;

			if (finishedJump && !moving && canMove && !onLadder)
            {
				if (playerAnimator.speed == 0) {
					playerAnimator.speed = 1;
				}

                if (!hasSuit)
                {
                    playerAnimator.Play("StellaStand");
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else
                {
                    playerAnimator.Play("SpaceStand");
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }

            //If player is dead, countdown to respawn
            if (!isAlive)
                tdTimer();
            
            //Picks up item if there is one [yay for items and inventory systems :'( ]
            if (Input.GetKey(KeyCode.E))
            {
                if (groundItem)
                {
                    if (currItem.CompareTag("Spacesuit"))
                        pickUpSuit();
                    else
                        pickUpItem();
                }

                //Ladder code for getting on ladder
                else if (hasLadder && !onLadder && lCoolReady && System.Math.Abs(gameObject.transform.position.x-ladderBounds[2])<MAX_SNAP_DISTANCE)
                {
                    onLadder = true;
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    gameObject.transform.position = new Vector2(ladderBounds[2],gameObject.transform.position.y);
                    rb.velocity = new Vector2(0, 0);
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    lCoolReady = false;
                    Invoke("lCoolDone", COOL_TIME);
                }
                else if (onLadder && lCoolReady && lCoolDiff())
                {
                    onLadder = false;
                    gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    rb.velocity = new Vector2(0, 0);
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    lCoolReady = false;
                    Invoke("lCoolDone", COOL_TIME);
                }

            }

            updateObjective();
			if (currentObjective > 6) {
				inventoryCheck ();
			}
        }
        else
        {
            if (awakeSequenceStarted == false)
            {
                awakeSequenceStarted = true;
                StartCoroutine("WakePlayer");
            }
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.CompareTag("Floor") && finishedJump == false && isAlive)
		{
			finishedJump = true;
            if (!crouch)
            {
                if (!hasSuit)
                {
                    playerAnimator.Play("StellaStand");
                }
                else
                {
                    playerAnimator.Play("SpaceStand");
                }
            }
			if (activeHint == false) {
				canMove = true;
			}
            currD = 0;
		}
		if(coll.gameObject.CompareTag("Wall") && isAlive)
		{
			if (!finishedJump) {
				canMove = false;
			} 
//			else {
//				;
//			}
		}
        if (coll.gameObject.CompareTag("Hazard")) die();

	}
    
    private bool checkV()
    {
        if (System.Math.Abs(rb.velocity.x) < 0.4f) return true;
        else return false;
    }
    private void die()
    {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        if(onLadder)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            onLadder = false;
            hasLadder = false;
        }
        isAlive = false;
        canMove = false;
        if (!hasSuit) {
			if (suffocated) {
				playerAnimator.Play ("StellaSuffocation");
			} 
			else {
				playerAnimator.Play ("StellaDeath");
			}
		} 
		else {
			playerAnimator.Play ("SpaceDie");
		}
    }

    private void respawn()
    {
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.transform.position = new Vector2(xSpawn,ySpawn);
		suffocated = false;
        isAlive = true;
        canMove = true;
    }

    private void tdTimer()
    {
        if (tTime < 3)
            tTime += Time.fixedDeltaTime;
        else
        {
            tTime = 0;
            respawn();
        }
    }

    public void canPickUp(bool a)
    {
        groundItem = a;
    }

    public void setItemID(int num)
    {
        gItemID = num;
    }

    public void setRef(GameObject o)
    {
        currItem = o;
    }

    private void pickUpItem()
    {
        currItem.SendMessage("claim");
        inv.SendMessage("claim", gItemID);
        groundItem = false;
        gItemID = 0;
        currItem = null;
    }

    private void pickUpSuit()
    {
        hasSuit = true;
        currItem.SendMessage("claim");
        groundItem = false;
        currItem = null;
    }

    public void tp(Vector3 a)
    {
        gameObject.transform.parent.position = new Vector3(a.x, a.y, 0);
        gameObject.transform.position = new Vector3(a.x, a.y, 0);
        rb.velocity = new Vector2(0f,0f);
    }


    public void canClimb(bool c)
    {
        hasLadder = c;

    }

    private bool lCoolDiff()
    {
        if (System.Math.Abs(gameObject.transform.position.y - ladderBounds[1]) < ON_OFF_VARIANCE_TOP || System.Math.Abs(gameObject.transform.position.y - ladderBounds[0]) < ON_OFF_VARIANCE_BOT || gameObject.transform.position.y < ladderBounds[0])
            return true;
        else return false;
    }

    public void passLadderBounds(float[] a)
    {
        ladderBounds = a;
    }

    private void lCoolDone()
    {
        lCoolReady = true;
    }

    public IEnumerator displayHint()
	{
		if (isAlive) {
			canMove = false;
			hintBox.SetActive (true);
			if (currentObjective == 0) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to find my crew members and check if they're alright...)";
			}
			else if (currentObjective == 1) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to get into the communications room to send out an SOS signal...)";
			}
			else if (currentObjective == 2) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to search the rooms for two omnicards in order to get into the communications room...)";
			}
			else if (currentObjective == 3) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I should go into the communications room and send out an SOS signal before I do anything else...)";
			}
			else if (currentObjective == 4) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I should try to send my location out using the GPS Tracker in the command center...)";
			}
			else if (currentObjective == 5) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I need the repair manual located in the engineering wing in order to find the tools I need...)";
			}
			else if (currentObjective == 6) {
				hintText.text = "<color=fuchsia>Stella</color>: (...The ship's oxygen system is failing! I need to find a spacesuit before oxygen levels drop to zero...)";
			}
			else if (currentObjective == 7) {
				inventoryString = "";
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to find the following item(s) in the Engineering Wing...) \n";
				yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
				yield return new WaitForSeconds (0.2f);
				for (int i = 0; i < 2; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
						Debug.Log (inventoryString);
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 8) {
				inventoryString = "";
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to find the following item(s) in the Medical Ward...) \n";
				yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
				yield return new WaitForSeconds (0.2f);
				for (int i = 2; i < 5; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 9) {
				inventoryString = "";
				hintText.text = "<color=fuchsia>Stella</color>: (...I need to find the following item(s) in the Engine Room...) \n";
				yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
				yield return new WaitForSeconds (0.2f);
				for (int i = 5; i < 8; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 10) {
				hintText.text = "<color=fuchsia>Stella</color>: (...Now that I have all the repair tools, I can head back to the communications room back on the first floor...)";
			}
			else if (currentObjective == 11) {
				hintText.text = "<color=fuchsia>Stella</color>: (...I can finally repair the communications terminal and send out an SOS signal...)";
			}

			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			hintBox.SetActive (false);
			canMove = true;
			activeHint = false;
		}
		StopCoroutine ("displayHint");
	}

	public void updateObjective ()
	{
		if (deadCrew && currentObjective == 0) {
			currentObjective = 1;
		}
		else if (commsCenterInit && currentObjective == 1) {
			currentObjective = 2;
		}
		else if (keyCards && currentObjective == 2) {
			currentObjective = 3;
		}
		else if (accessCommsCenter && currentObjective == 3) {
			currentObjective = 4;
		}
		else if (commandCenter && currentObjective == 4) {
			currentObjective = 5;
		}
		else if (manual && currentObjective == 5 && alarmIsStarted) {
			currentObjective = 6;
			InvokeRepeating("alarmOn", 0.0f, 0.05f);
		}
		else if (hasSuit && currentObjective == 6) {
			StartCoroutine("AlarmOffSequence");
			currentObjective = 7;
		}
		else if (engineeringItems && currentObjective == 7) {
			currentObjective = 8;
		}
		else if (medicItems && currentObjective == 8) {
			currentObjective = 9;
		}
		else if (engineItems && currentObjective == 9) {
			StartCoroutine ("allItems");
			currentObjective = 10;
		}
		else if (commsCenterFinal && currentObjective == 10) {
			currentObjective = 11;
		}
		else if (repairComms && currentObjective == 11) {

		}

	}

	public void inventoryCheck()
	{
		if (currentObjective == 7) {
			if (obtainedObj [0] == "" && obtainedObj [1] == "") {
				engineeringItems = true;
			}
		}
		else if (currentObjective == 8) {
			if (obtainedObj [2] == "" && obtainedObj [3] == "" && obtainedObj[4] == "") {
				medicItems = true;
			}
		}
		else if (currentObjective == 9) {
			if (obtainedObj [5] == "" && obtainedObj [6] == "" && obtainedObj[7] == "") {
				engineItems = true;
			}
		}

	}

	public IEnumerator WakePlayer()
	{
        playerAnimator.Play ("StellaWakingUp");
		yield return new WaitForSeconds (4.75f);
		darkness.color = new Color(0,0,0,0);
		glow.color = new Color(1,1,1,1);
		hintBox.SetActive (true);
		setHintText ("<color=fuchsia>Stella</color>: Ugh...what happened? Did...we crash? ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: We were on our way to that planetary system, Xylo. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Yeah, yeah that's right...but what the hell happened?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: I think I'm alright, no bruises or broken bones. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: I need to see if the others are okay before I do anything else.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		hintBox.SetActive (false);
		isAwake = true;
		StopCoroutine ("WakePlayer");
	}

	public void setHintText(string hint)
	{
		hintText.text = hint;
	}

	public void alarmOn()
	{
		AlarmUI = GameObject.Find ("Alarm").GetComponent<Image>();
		if (AlarmUI == null) {
			CancelInvoke("alarmOn");
			return;
		}
		if (!bounce && AlarmUI.color.a < 0.5f) {
			AlarmUI.color = Color.Lerp (AlarmUI.color, Color.red, fadeSpeed * Time.deltaTime);
		} 
		else if (!bounce && AlarmUI.color.a >= 0.5f) {
			bounce = true;
		}
		else if (bounce && AlarmUI.color.a > 0.15f) {
			AlarmUI.color = Color.Lerp (AlarmUI.color, Color.clear, fadeSpeed * Time.deltaTime);
		}
		else if (bounce && AlarmUI.color.a <= 0.15f) {
			bounce = false;
		}
	}

	public void FadeToBlack()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a == 1.0f) {
			CancelInvoke ("FadeToBlack");
		}
	}

	public void FadeToClear()
	{
        //Bug: this gets called again whenever Level One is entered
		FadeImg.color = Color.Lerp (FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a < 0.2f) {
			CancelInvoke ("FadeToClear");
			introDone = true;
			FadeImg.color = Color.clear;
			Debug.Log (FadeImg.color.a);
			activeHint = false;
		}
	}

    public void setLv1Coords()
    {
        lvCoords[0] = gameObject.transform.position;
    }

    public void setLv1Coords(Vector3 a)
    {
        lvCoords[0] = a;
    }

    public void updateSpawn(Vector3 cp)
    {
        xSpawn = cp.x;
        ySpawn = cp.y;
    }

    public void tpL1()
    {
        Debug.Log(lvCoords[0]);
        tp(lvCoords[0]);
        Debug.Log(gameObject.transform.position);

    }

	public IEnumerator endGame()
	{
		hintBox.SetActive (true);
		playerAnimator.Play ("StellaLook");
		setHintText ("<color=fuchsia>Stella</color>: There! It should be fully operational now!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.5f);
		hintBox.SetActive (false);
		playerAnimator.Play ("SpaceType");
		yield return new WaitForSeconds (3.0f);
		hintBox.SetActive (true);
		playerAnimator.Play ("StellaLook");
		setHintText ("<color=fuchsia>Stella</color>: Okay, I just sent out a signal.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: ...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Come on, come on...someone please pick it up...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: ...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Oh my God! Someone actually did! Hello? Hello?!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: This is Stella Kern reporting from the USS Ancora...Can anyone hear me? Hello?! Please someone...please answer...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Unknown Voice</color>: usnf...hsgt nhane?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: H-huh? What...I...don't understand? Why the hell isn't the built-in interpreter working?!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: P-please, I'm just trying to get off this ship...I need help, please!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Unknown Voice</color>: Jeohns, vaben dm doweuem and fear suits your kind well...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Wait...the interpreter is working...wh-what did you just say? Who is this?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: I am Mustafal Zabra, Ruler of the ProtoXylons. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: I want to thank you for reaching out to my people. I've been eagerly awaiting your signal...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Musta..fal? Please, I need your help! I-");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Did you like it? Xylo, I mean.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: W-what?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: My planet. Did you like it, human?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: I don't understand...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: It's everything you humans need, yes? For your new planet?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Y-yes...how-");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: But how unfortunate...you never made it to Xylo");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: ...never had the chance to selfishly exploit its resources");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: ...never had the chance to take advantage of those who reside there.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Wait, what? We'd never do that! We're compassionate by nature, we'd-");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Compassionate? You dare call yourself compassionate?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Where was that compassion when you humans waged war against my people, enslaved our women, murdered our children");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: ...all to look for new land to colonize and populate with your kind. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: You dare call your people compassionate when it was them who gutted our men and burned our villages?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: We welcomed your kind to our planets and cities...offered hospitality, food, drink and places to rest. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Then one night, I was awakened by the screams of my people and gazed in utter horror as I saw my towns in flames. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Your...compassionate people...destroyed so many lives. It took us months to drive them out of our planetary system.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: T-that can't be true...we'd never do anything like that!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: ...since then, we've isolated ourselves from others. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: We've learned from our mistakes and I am now ready to execute my plan...and it's all thanks to you, Stella Kern.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: H-huh? W-what are you talking about?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Xylo's sole purpose was to lure in naive humans like you. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: I created Xylo to be the perfect paradise for your people");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: ...and I only needed one human who was ignorant enough to take the bait. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: Now that we know exactly where you are, my people will arrive shortly to pay you a visit. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: We will use your ship's information to track down other humans and begin our universe-wide genocide of your species. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: None of this could have been done without you, my dear and thus, I sincerely thank you with every fiber in my body. ");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=red>Mustafal Zabra</color>: I promise...your death won't be as painful as the others.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		playerAnimator.Play ("StellaKneel");
		yield return new WaitForSeconds (0.5f);
		playerAnimator.Play ("StellaCry");
		yield return new WaitForSeconds (0.5f);
		hintBox.SetActive (false);
		InvokeRepeating ("FadeToBlack", 0.0f, 0.1f);
		yield return new WaitForSeconds (8.0f);
		SceneManager.LoadSceneAsync ("Credits");
	}

	public IEnumerator AlarmOffSequence()
	{
		yield return new WaitForSeconds (6.0f);
		CancelInvoke ("alarmOn");
		if(AlarmUI != null)
		{
			AlarmUI.color = new Color (0, 0, 0, 0);
		}
		activeHint = true;
		canMove = false;
		playerAnimator.Play("SpaceStand");
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		hintBox.SetActive (true);
		setHintText ("<color=yellow>Loudspeaker</color>: SYSTEM MALFUNCTION. CRITICAL POWER FAILU-...");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: That doesn't sound good. I should get moving.");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		hintBox.SetActive (false);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		canMove = true;
		activeHint = false;

	}

	public IEnumerator allItems()
	{
		activeHint = true;
		canMove = false;
		playerAnimator.Play("SpaceStand");
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		hintBox.SetActive (true);
		setHintText ("<color=fuchsia>Stella</color>: That's it. That's all the tools I need. I actually have them all!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText ("<color=fuchsia>Stella</color>: Now I can finally go back to the Communications room on the first floor and fix that broken terminal.\n I can actually signal for help!");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		hintBox.SetActive (false);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		canMove = true;
		activeHint = false;
	}

}
