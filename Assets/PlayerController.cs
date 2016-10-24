using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float force;
	public float jumpHeight;
    private float lockPos;
    private float initSpeed;
	public bool finishedJump;
	public bool canMove;
    private float scalD;
    private float currD;

	private Rigidbody2D rb;

	void Start ()
	{
		canMove = true;
		finishedJump = true;
		rb = GetComponent<Rigidbody2D>();
        lockPos = 0f;
        initSpeed = 0.6f;
        scalD = -0.01f;
        currD = 0f;
	}

	void FixedUpdate ()
	{
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);
        if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2(1.8f*force, 0));
                else rb.AddForce(new Vector2(force, 0));
                //This line checks to see if the speed in the x direction is below a certain value. If it is, it sets the velocity.
                //This helps to make movement a bit more responsive but still smooth.
                if (checkV()) rb.velocity = new Vector2(initSpeed, rb.velocity.y);
            }

			if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (1.8f * -force, 0));
                else rb.AddForce(new Vector2(-force, 0));
                if (checkV()) rb.velocity = new Vector2(-initSpeed, rb.velocity.y);
            }

			if (Input.GetKey (KeyCode.Space) && finishedJump) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}
		else {
			if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (force, 0));
                else rb.AddForce(new Vector2(0.6f * force, 0));
                if (checkV()) rb.velocity = new Vector2(initSpeed, rb.velocity.y);
            }

			if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (-force, 0));
                else rb.AddForce(new Vector2(0.6f * -force, 0));
                if (checkV()) rb.velocity = new Vector2(-initSpeed, rb.velocity.y);
            }

			if (Input.GetKey (KeyCode.Space) && finishedJump) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}

        if (!finishedJump)
        {
            currD += scalD;
            rb.velocity += new Vector2(0, currD);
        }
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.CompareTag("Floor") && finishedJump == false)
		{
			finishedJump = true;
			canMove = true;
            currD = 0;
		}
		if(coll.gameObject.CompareTag("Wall"))
		{
			if (!finishedJump) {
				canMove = false;
			} 
//			else {
//				;
//			}
		}
	}
    private bool checkV()
    {
        if (System.Math.Abs(rb.velocity.x) < 0.4f) return true;
        else return false;
    }
}
