using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManPlayer : MonoBehaviour {

	public float speed = 100.0f;

	private Vector2Int direction = Vector2Int.zero;
	private Vector2Int oldDirection = Vector2Int.zero;
	private Vector2 blockDirection = Vector2.zero;
	private Vector3 distanceMoved = Vector3.zero;
	private Vector2Int targetIndicies;
	private Vector3 targetPosition;

	private static readonly Vector2Int LEFT = new Vector2Int(0, -1);
	private static readonly Vector2Int UP = new Vector2Int(-1, 0);
	private static readonly Vector2Int RIGHT = new Vector2Int(0, 1);
	private static readonly Vector2Int DOWN = new Vector2Int(1, 0);

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		CheckInput ();

		Move ();

		UpdateOrientation ();
	}

	void CheckInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			direction = LEFT;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			direction = RIGHT;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			direction = UP;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			direction = DOWN;
		}

		if (direction == oldDirection)
			return;

		if (direction == blockDirection)
		{
			oldDirection = direction = Vector2Int.zero;
			return;
		}

		blockDirection = Vector2.zero;
		SetTarget();
		oldDirection = direction;
	}

	void Move () {
		if (direction == Vector2Int.zero)
			return;

		Vector2 moveTo = new Vector2(direction.y  , direction.x * -1);
		distanceMoved = (Vector3)(moveTo * speed) * Time.deltaTime;

		Vector3 targetDistance = targetPosition - transform.position;

		Vector3 targetPositionDistAbs = Utils.Abs(targetDistance);
		Vector3 distancedMovedAbs = Utils.Abs(distanceMoved);
		if(targetPositionDistAbs.x < distancedMovedAbs.x &&
		   targetPositionDistAbs.y < distancedMovedAbs.y )
		{
			transform.position += targetDistance;
			return;
		}

		transform.position += distanceMoved;
		//transform.localPosition += distanceMoved;

		SetTarget();
	}

	void UpdateOrientation()
	{
		if (direction == LEFT)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 180);
		}
		else if (direction == RIGHT)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		else if (direction == UP)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 90);
		}
		else if (direction == DOWN)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 270);
		}
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		//if (direction == Vector2.zero)
		//	return;
		//
		//if (collision.gameObject.CompareTag("Wall"))
		//{
		//	//distanceMoved = Mathf.Max(distanceMoved, (Vector3)(direction * speed) * Time.deltaTime);
		//	transform.localPosition -= (Vector3)(direction * speed) * Time.deltaTime;
		//	//transform.localPosition -= distanceMoved;
		//	blockDirection = direction;
		//	direction = Vector2.zero;
		//}
		//else if(collision.gameObject.CompareTag("Point"))
		//{
		//	collision.gameObject.SetActive(false);
		//}
		//else if (collision.gameObject.CompareTag("Power"))
		//{
		//	collision.gameObject.SetActive(false);
		//}
	}

	private void SetTarget()
	{
		if (direction == Vector2Int.zero)
			return;

		targetIndicies = CreatePacManMaze.GetIndices(gameObject.transform.position);
		Transform targetTransform = CreatePacManMaze.GetNode(targetIndicies + direction);
		if (targetTransform == null)
		{
			oldDirection = direction = Vector2Int.zero;
			return;
		}

		targetPosition = targetTransform.position;

	}
}
