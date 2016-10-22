using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpHeight;
	public bool finishedJump;
	public bool canMove;

	private Rigidbody2D rb;

	void Start ()
	{
		canMove = true;
		finishedJump = true;
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate ()
	{

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			if (Input.GetKey (KeyCode.RightArrow) && canMove) {
				rb.velocity = new Vector2 (5.0f * speed, rb.velocity.y);
			}

			if (Input.GetKey (KeyCode.LeftArrow) && canMove) {
				rb.velocity = new Vector2 (5.0f * -speed, rb.velocity.y);
			}

			if (Input.GetKey (KeyCode.Space) && finishedJump == true) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}
		else {
			if (Input.GetKey (KeyCode.RightArrow) && canMove) {
				rb.velocity = new Vector2 (speed, rb.velocity.y);
			}

			if (Input.GetKey (KeyCode.LeftArrow) && canMove) {
				rb.velocity = new Vector2 (-speed, rb.velocity.y);
			}

			if (Input.GetKey (KeyCode.Space) && finishedJump == true) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}
			

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.CompareTag("Floor") && finishedJump == false)
		{
			finishedJump = true;
			canMove = true;
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
}
