using UnityEngine;
using UnityEngine.Experimental;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float force;
	public float jumpHeight;
	public Animator playerAnimator;
	public SpriteRenderer playerRenderer;
    private float lockPos;
    private float initSpeed;
	public bool finishedJump;
	public bool canMove;
	public bool hasSuit;
	public bool suffocated;
    private float scalD;
    private float currD;
    public bool isAlive;
    private float xSpawn;
    private float ySpawn;
    private bool groundItem;
	public bool moving;
    private int gItemID;
    private GameObject currItem;
    private GameObject inv;

    public static bool isTouchingSuit=false;

    private float tTime;

	private Rigidbody2D rb;

    void Start()
    {
		GameObject.DontDestroyOnLoad (GameObject.FindWithTag("Full Player"));
        isAlive = true;
		moving = false;
		hasSuit = false;
        canMove = true;
        finishedJump = true;
        rb = GetComponent<Rigidbody2D>();
        lockPos = 0f;
        initSpeed = 0.6f;
        scalD = -0.011f;
        currD = 0f;
        xSpawn = gameObject.transform.position.x;
        ySpawn = gameObject.transform.position.y;
        groundItem = false;
        gItemID = 0;
        currItem = null;
        inv = GameObject.Find("Inventory Slots");
		playerAnimator.Play ("StellaStand");

        tTime = 0;
	}

	void FixedUpdate ()
	{

		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			if (isAlive) {
				canMove = false;
				if (!hasSuit) {
					playerAnimator.Play ("StellaCrouching");
				} 
				else {
					playerAnimator.Play ("SpaceCrouch");
				}
			}
		} 
		else 
		{
			if (isAlive) {
				canMove = true;
			}
		}

        //Movement
        if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) || (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))) {
				if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && canMove) {
					if (rb.velocity.x < 0) {
						rb.velocity = new Vector2 (0, rb.velocity.y);
					}
					playerRenderer.flipX = true;
					moving = true;
					if (finishedJump) {
						if (rb.velocity.x < 9) {
							rb.AddForce (new Vector2 (1.8f * force, 0));
						} else {
							rb.velocity = new Vector2 (9, rb.velocity.y);
						}
					} else {
						if (rb.velocity.x < 9) {
							rb.AddForce (new Vector2 (force, 0));
						} else {
							rb.velocity = new Vector2 (9, rb.velocity.y);
						}
					}
					//This line checks to see if the speed in the x direction is below a certain value. If it is, it sets the velocity.
					//This helps to make movement a bit more responsive but still smooth.
					if (checkV ())
						rb.velocity = new Vector2 (initSpeed, rb.velocity.y);
					if (finishedJump) {
						if (!hasSuit) {
							playerAnimator.Play ("StellaRunning");
						} 
						else {
							playerAnimator.Play ("SpaceRun");
						}
					}
				}

				if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && canMove) {
					if (rb.velocity.x > 0) {
						rb.velocity = new Vector2 (0, rb.velocity.y);
					}
					playerRenderer.flipX = false;
					moving = true;
					if (finishedJump) {
						if (rb.velocity.x > -9) {
							rb.AddForce (new Vector2 (1.8f * -force, 0));
						} else {
							rb.velocity = new Vector2 (-9, rb.velocity.y);
						}
					} else {
						if (rb.velocity.x > -9) {
							rb.AddForce (new Vector2 (-force, 0));
						} else {
							rb.velocity = new Vector2 (-9, rb.velocity.y);
						}
					}
					if (checkV ())
						rb.velocity = new Vector2 (-initSpeed, rb.velocity.y);
					if (finishedJump) {
						if (!hasSuit) {
							playerAnimator.Play ("StellaRunning");
						} 
						else {
							playerAnimator.Play ("SpaceRun");
						}
					}
				}
			} 
			else {
				moving = false;
			}

			if (Input.GetKey (KeyCode.Space) && finishedJump && canMove) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				if (!hasSuit) {
					playerAnimator.Play ("StellaJumping");
				} 
				else {
					playerAnimator.Play ("SpaceJump");
				}
				finishedJump = false;
			}
		}
		else {
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) || (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))) {
				if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && canMove) {
					if (rb.velocity.x < 0) {
						rb.velocity = new Vector2 (0, rb.velocity.y);
					}
					playerRenderer.flipX = true;
					moving = true;
					if (finishedJump) {
						if (rb.velocity.x < 4.5f) {
							rb.AddForce (new Vector2 (force, 0));
						} else {
							rb.velocity = new Vector2 (4.5f, rb.velocity.y);
						}
					} 
					else {
						if (rb.velocity.x < 4.5f) {
							rb.AddForce (new Vector2 (0.6f * force, 0));
						} else {
							rb.velocity = new Vector2 (4.5f, rb.velocity.y);
						}

					}
					if (checkV ())
						rb.velocity = new Vector2 (initSpeed, rb.velocity.y);
					if (finishedJump) {
						if (!hasSuit) {
							playerAnimator.Play ("StellaWalking");
						} 
						else {
							playerAnimator.Play ("SpaceWalk");
						}
					}
				}

				if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && canMove) {
					if (rb.velocity.x > 0) {
						rb.velocity = new Vector2 (0, rb.velocity.y);
					}
					playerRenderer.flipX = false;
					moving = true;
					if (finishedJump) {
						if (rb.velocity.x > -4.5f) {
							rb.AddForce (new Vector2 (-force, 0));
						} else {
							rb.velocity = new Vector2 (-4.5f, rb.velocity.y);
						}
					} else {
						if (rb.velocity.x > -4.5f) {
							rb.AddForce (new Vector2 (0.6f * -force, 0));
						} else {
							rb.velocity = new Vector2 (-4.5f, rb.velocity.y);
						}
					}
					if (checkV ())
						rb.velocity = new Vector2 (-initSpeed, rb.velocity.y);
					if (finishedJump) {
						if (!hasSuit) {
							playerAnimator.Play ("StellaWalking");
						} 
						else {
							playerAnimator.Play ("SpaceWalk");
						}
					}
				}
			} 
			else {
				moving = false;
			}

			if (Input.GetKey (KeyCode.Space) && finishedJump && canMove) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				if (!hasSuit) {
					playerAnimator.Play ("StellaJumping");
				} 
				else {
					playerAnimator.Play ("SpaceJump");
				}
				finishedJump = false;
			}
		}

        //Increases falling rate
        if (!finishedJump)
        {
            currD += scalD;
            if (currD < -2) currD = -5;
            rb.velocity += new Vector2(0, currD);
        }

		if (finishedJump && !moving && canMove) {
			if (!hasSuit) {
				playerAnimator.Play ("StellaStand");
				rb.velocity = new Vector2 (0, rb.velocity.y);
			} 
			else {
				playerAnimator.Play ("SpaceStand");
				rb.velocity = new Vector2 (0, rb.velocity.y);
			}
		}


        //If player is dead, countdown to respawn
        if (!isAlive)
            tdTimer();

        //Picks up item if there is one [yay for items and inventory systems :'( ]
        if(Input.GetKey(KeyCode.E) && groundItem)
        {
            if (currItem.CompareTag("Spacesuit"))
                pickUpSuit();
            else
                pickUpItem();
        }

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
			canMove = true;
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
}
