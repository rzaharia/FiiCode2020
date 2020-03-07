using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManPlayer : MonoBehaviour {

	public float speed = 100.0f;

	private Vector2 direction = Vector2.zero;
	private Vector2 blockDirection = Vector2.zero;
	private Vector3 distanceMoved = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		CheckInput ();

		Move ();

		UpdateOrientation ();
	}

	void CheckInput () {

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {

			direction = Vector2.left;

		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {

			direction = Vector2.right;

		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {

			direction = Vector2.up;

		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {

			direction = Vector2.down;
		}

		if(direction == blockDirection)
		{
			direction = Vector2.zero;
		}
		else
		{
			blockDirection = Vector2.zero;
		}
	}

	void Move () {
		if (direction == Vector2.zero)
			return;

		distanceMoved = (Vector3)(direction * speed) * Time.deltaTime;
		transform.localPosition += distanceMoved;
	}

	void UpdateOrientation()
	{
		if (direction == Vector2.left)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 180);
		}
		else if (direction == Vector2.right)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		else if (direction == Vector2.up)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 90);
		}
		else if (direction == Vector2.down)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 270);
		}
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (direction == Vector2.zero)
			return;

		if (collision.gameObject.CompareTag("Wall"))
		{
			transform.localPosition -= distanceMoved;
			blockDirection = direction;
			direction = Vector2.zero;
		}
		else if(collision.gameObject.CompareTag("Point"))
		{
			collision.gameObject.SetActive(false);
		}
		else if (collision.gameObject.CompareTag("Power"))
		{
			collision.gameObject.SetActive(false);
		}
	}
}
