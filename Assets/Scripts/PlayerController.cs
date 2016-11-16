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

    //Movement
    public bool finishedJump;
    public bool canMove;
    public bool moving;
    private float scalD;
    private float currD;
    private bool sprint;
    private float airMult;
    private readonly float WALK_STARTSPEED = 0.6f;
    private readonly float WALK_SPEEDCAP = 4.5f;
    private readonly float RUN_SPEEDCAP = 9.0f;
    private readonly float SPR_MULT = 1.8f;

    //Appearance, death, respawning, other miscellaneous player variables
    public bool hasSuit;
    public static bool isTouchingSuit = false;
    public bool suffocated;
    public bool isAlive;
    private float xSpawn;
    private float ySpawn;
    public bool isAwake;
    public bool awakeSequenceStarted;

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

	private string inventoryString;
    private string[] obtainedObj;
	public float fadeSpeed = .05f;

    //IDK what this is
	public bool deadCrew;
	public bool commsCenterInit;
	public bool keyCards;
	public bool accessCommsCenter;
	public bool commandCenter;
	public bool manual;
	public bool engineeringItems;
	public bool medicItems;
	public bool engineItems;
	public bool commsCenterFinal;
	public bool repairComms;

	public Image FadeImg;
	public Image AlarmUI;
	public bool bounce;
	public bool alarmIsStarted = false;

    void Start()
    {
		GameObject.DontDestroyOnLoad (GameObject.FindWithTag("Full Player"));
        isAlive = true;
		isAwake = false;
		deadCrew = false;
		awakeSequenceStarted = false;
		moving = false;
		hasSuit = false;
        canMove = true;
        finishedJump = true;
        rb = GetComponent<Rigidbody2D>();
        scalD = -0.011f;
        currD = 0f;
        xSpawn = gameObject.transform.position.x;
        ySpawn = gameObject.transform.position.y;
        groundItem = false;
        gItemID = 0;
        currItem = null;
        inv = GameObject.Find("Inventory Slots");
		playerAnimator.Play ("StellaStand");
		currentObjective = 0;
		hintBox = GameObject.FindWithTag ("HintBox");
		hintText = hintBox.GetComponentInChildren<Text> ();
		hintBox.SetActive (false);
		activeHint = false;
		obtainedObj = new string[8];
		obtainedObj [0] = "Power Drill";
		obtainedObj [1] = "Wrench";
		obtainedObj [2] = "Switchblade";
		obtainedObj [3] = "Hammer";
		obtainedObj [4] = "Saw";
		//obtainedObj [5] = "Blow Torch";
		obtainedObj [6] = "Screw Driver";
		obtainedObj [7] = "Wire Cutters";
		bounce = false;

		FadeImg = GameObject.Find ("Fade").GetComponent<Image>();
		InvokeRepeating ("FadeToClear", 0.0f, 0.1f);

        tTime = 0;
	}

	void FixedUpdate ()
	{
		if (hintBox == null) {
			FadeImg = GameObject.Find ("Fade").GetComponent<Image>();
			InvokeRepeating ("FadeToClear", 0.0f, 0.1f);
			hintBox = GameObject.FindWithTag ("HintBox");
			hintText = hintBox.GetComponentInChildren<Text> ();
			hintBox.SetActive (false);
			activeHint = false;
		}

		if (isAwake) {
			if (Input.GetKeyDown (KeyCode.H) && !activeHint) {
				rb.velocity = new Vector2 (0, rb.velocity.y);
				if (!hasSuit) {
					playerAnimator.Play ("StellaStand");
				} else {
					playerAnimator.Play ("SpaceStand");
				}
				StartCoroutine ("displayHint");
				activeHint = true;
			}

			if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && activeHint == false) {
				if (isAlive) {
					canMove = false;
					if (!hasSuit) {
						playerAnimator.Play ("StellaCrouching");
					} else {
						playerAnimator.Play ("SpaceCrouch");
					}
				}
			} else {
				if (isAlive && activeHint == false) {
					canMove = true;
				}
			}

            //Movement
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

            if (canMove)
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
                        if (!hasSuit)
                        {
                            playerAnimator.Play("StellaRunning");
                        }
                        else
                        {
                            playerAnimator.Play("SpaceRun");
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
                        if (!hasSuit)
                        {
                            playerAnimator.Play("StellaWalking");
                        }
                        else
                        {
                            playerAnimator.Play("SpaceWalk");
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
                        if (!hasSuit)
                        {
                            playerAnimator.Play("StellaRunning");
                        }
                        else
                        {
                            playerAnimator.Play("SpaceRun");
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
                        if (!hasSuit)
                        {
                            playerAnimator.Play("StellaWalking");
                        }
                        else
                        {
                            playerAnimator.Play("SpaceWalk");
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

            //Increases falling rate
            if (!finishedJump) {
				currD += scalD;
				if (currD < -2)
					currD = -5;
				rb.velocity += new Vector2 (0, currD);
			}

			if (finishedJump && !moving && canMove) {
				if (!hasSuit) {
					playerAnimator.Play ("StellaStand");
					rb.velocity = new Vector2 (0, rb.velocity.y);
				} else {
					playerAnimator.Play ("SpaceStand");
					rb.velocity = new Vector2 (0, rb.velocity.y);
				}
			}


			//If player is dead, countdown to respawn
			if (!isAlive)
				tdTimer ();

			//Picks up item if there is one [yay for items and inventory systems :'( ]
			if (Input.GetKey (KeyCode.E) && groundItem) {
				if (currItem.CompareTag ("Spacesuit"))
					pickUpSuit ();
				else
					pickUpItem ();
			}
		} 
		else {
			if (awakeSequenceStarted == false) {
				awakeSequenceStarted = true;
				StartCoroutine ("WakePlayer");
			}
		}

		updateObjective ();
	}

    void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.CompareTag("Floor") && finishedJump == false && isAlive)
		{
			finishedJump = true;
			if (!hasSuit) {
				playerAnimator.Play ("StellaStand");
			} 
			else {
				playerAnimator.Play ("SpaceStand");
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

    public void updateCheck(Vector3 cp)
    {
        xSpawn = cp.x;
        ySpawn = cp.y;
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
        gameObject.transform.position = new Vector3(a.x, a.y, 0);
    }

	public IEnumerator displayHint()
	{
		if (isAlive) {
			canMove = false;
			hintBox.SetActive (true);
			if (currentObjective == 0) {
				hintText.text = "Stella: (...I've got to find my crew members and see if they're alright...)";
			}
			else if (currentObjective == 1) {
				hintText.text = "Stella: (...I need to get into the communications room to send out an SOS signal...)";
			}
			else if (currentObjective == 2) {
				hintText.text = "Stella: (...I need to search the rooms for two omnicards in order to get into the communications room...)";
			}
			else if (currentObjective == 3) {
				hintText.text = "Stella: (...I should go into the communications room and send out an SOS signal before I do anything else...)";
			}
			else if (currentObjective == 4) {
				hintText.text = "Stella: (...I should try to send my location out using the GPS Tracker in the command center...)";
			}
			else if (currentObjective == 5) {
				hintText.text = "Stella: (...I need the repair manual located in the engineering wing in order to find the tools I need...)";
			}
			else if (currentObjective == 6) {
				hintText.text = "Stella: (...The ship's oxygen system is failing! I need to find a spacesuit before oxygen levels drop to zero...)";
			}
			else if (currentObjective == 7) {
				inventoryString = "Stella: (...I need to find the following item(s) in the Engineering Wing...) \n";
				for (int i = 0; i < inv.GetComponent<Inventory> ().items.Length; i++) {
					if (inv.GetComponent<Inventory> ().items [i].name == "Power Drill") {
						obtainedObj [0] = "";
					} 
					else if (inv.GetComponent<Inventory> ().items [i].name == "Wrench") {
						obtainedObj [1] = "";
					}
				}
				for (int i = 0; i < 2; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 8) {
				inventoryString = "Stella: (...I need to find the following item(s) in the Medical Ward...) \n";
				for (int i = 0; i < inv.GetComponent<Inventory> ().items.Length; i++) {
					if (inv.GetComponent<Inventory> ().items [i].name == "Switchblade") {
						obtainedObj [2] = "";
					} 
					else if (inv.GetComponent<Inventory> ().items [i].name == "Hammer") {
						obtainedObj [3] = "";
					} 
					else if (inv.GetComponent<Inventory> ().items [i].name == "Saw") {
						obtainedObj [4] = "";
					}
				}
				for (int i = 2; i < 5; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 9) {
				inventoryString = "Stella: (...I need to find the following item(s) in the Engine Room...) \n";
				for (int i = 0; i < inv.GetComponent<Inventory> ().items.Length; i++) {
					if (inv.GetComponent<Inventory> ().items [i].name == "Blow Torch") {
						obtainedObj [5] = "";
					} 
					else if (inv.GetComponent<Inventory> ().items [i].name == "Screwdriver") {
						obtainedObj [6] = "";
					} 
					else if (inv.GetComponent<Inventory> ().items [i].name == "Wire Cutters") {
						obtainedObj [7] = "";
					}
				}
				for (int i = 5; i < 8; i++) {
					if (obtainedObj [i] != "") {
						inventoryString += "\t(" + obtainedObj[i] + ")\n";
					}
				}
				hintText.text = inventoryString;
			}
			else if (currentObjective == 10) {
				hintText.text = "Stella: (...Now that I have all the repair tools, I can head back to the communications room back on the first floor...)";
			}
			else if (currentObjective == 11) {
				hintText.text = "Stella: (...I can finally repair the communications terminal and send out an SOS signal...)";
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
			currentObjective = 7;
		}
		else if (engineeringItems && currentObjective == 7) {
			currentObjective = 8;
		}
		else if (medicItems && currentObjective == 8) {
			currentObjective = 9;
		}
		else if (engineItems && currentObjective == 9) {
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
		yield return new WaitForSeconds (3.5f);
		hintBox.SetActive (true);
		setHintText ("Stella: Ugh... What's going on?");
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		setHintText("Stella: The ship! My crew members! I need to find them!");
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

	public void FadeToClear()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a < 0.05f) {
			CancelInvoke ("FadeToClear");
			FadeImg.color = Color.clear;
		}
	}
		
}
