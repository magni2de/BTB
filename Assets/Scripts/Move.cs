using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float lowSpeed = 3f;
	public float fastSpeed = 10f;

	public KeyCode enableFastSpeedWithKey = KeyCode.LeftShift;

	private Rigidbody2D playerRB2D;
	private Animator playerAnim;

	private const int orientation_RIGHT = 0;
	private const int orientation_DOWN = 1;
	private const int orientation_LEFT = 2;
	private const int orientation_UP = 3;

	// Use this for initialization
	void Start () {
		
		playerRB2D = gameObject.GetComponent<Rigidbody2D> ();
		playerAnim = gameObject.GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

		float maxSpeed = lowSpeed;

		//float horiz = Input.GetAxis ("Horizontal");
		//float vert = Input.GetAxis ("Vertical");

		var keyRight = Input.GetKey (KeyCode.RightArrow);
		var keyDown = Input.GetKey (KeyCode.DownArrow);
		var keyLeft = Input.GetKey (KeyCode.LeftArrow);
		var keyUp = Input.GetKey (KeyCode.UpArrow);

		if (Input.GetKey(enableFastSpeedWithKey))
			maxSpeed = fastSpeed;

		playerAnim.SetFloat ("Speed", Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")), Mathf.Abs(Input.GetAxis("Vertical"))));

		if (keyRight && !keyLeft) {
			
			playerAnim.SetInteger("Orientation", orientation_RIGHT);
			playerRB2D.velocity = new Vector2 (maxSpeed, playerRB2D.velocity.y);

		}

		if (!keyRight && !keyLeft) playerRB2D.velocity = new Vector2(0f, playerRB2D.velocity.y);

		if (keyLeft && !keyRight) {

			playerAnim.SetInteger ("Orientation", orientation_LEFT);
			playerRB2D.velocity = new Vector2 (-maxSpeed, playerRB2D.velocity.y);

		}

		if (keyUp && !keyDown) {

			playerAnim.SetInteger ("Orientation", orientation_UP);
			playerRB2D.velocity = new Vector2 (playerRB2D.velocity.x, maxSpeed);

		}

		if (!keyUp && !keyDown) playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, 0f);

		if (keyDown && !keyUp) {

			playerAnim.SetInteger ("Orientation", orientation_DOWN);
			playerRB2D.velocity = new Vector2 (playerRB2D.velocity.x, -maxSpeed);

		}

		//Vector3 movement = new Vector3 	(horiz, vert, 0f);
		//transform.Translate (movement * maxSpeed * Time.deltaTime);

		//playerRB2D.AddForce (Vector2.right * horiz * 50f);
		//playerRB2D.AddForce (Vector2.up * vert * 50f);

	}
}
