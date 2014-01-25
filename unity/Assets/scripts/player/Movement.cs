﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public bool UseKeyboard;

	public KeyCode Up = KeyCode.W;
	public KeyCode Down = KeyCode.S;
	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Action = KeyCode.Space;
	Transform _trans;

	public float MovementSpeed = 1;

	Collider _playerCollider;

	RoomLocation OnRoomLocation;



	void Start () 
	{
		_playerCollider = GetComponent<Collider> ();
		Debug.Log ("Starting Movement");
		_trans = gameObject.transform;
	}
	
	void Update () 
	{
		if (UseKeyboard)
		{
			if (Input.GetKey(Up))
			{
				MoveUp ();
			}
			if (Input.GetKey(Down))
			{
				MoveDown ();
			}
			if (Input.GetKey(Left))
			{
				MoveLeft ();
			}

			if (Input.GetKey(Right))
			{
				MoveRight ();
			}

			if (Input.GetKeyUp(Action))
			{
				FireAction ();
			}
		}
	}


	 bool CheckHit (Vector3 dir)
	{

		Ray r  = new Ray(transform.position,dir);
		RaycastHit hit;
		Physics.Raycast(r, out hit,0.01f);

		if (hit.collider != null)
		{
			if (hit.collider.tag == "boundary")
			{
				return true;
			}
		}
		return false;

	}


	Boundary CheckHitGetBoundary (Vector3 dir)
	{
		Ray r  = new Ray(transform.position,dir);
		RaycastHit hit;
		Physics.Raycast(r, out hit,0.01f);
		
		if (hit.collider != null)
		{

			return hit.collider.gameObject.GetComponent<Boundary> ();
			
		}
		return null;

	}

	void MoveUp()
	{

		Boundary b = CheckHitGetBoundary(Vector3.up);
	
		if (!CheckHit(Vector3.up)) _trans.localPosition += new Vector3 (0,MovementSpeed, 0);


		else
		{
			if (b != null)
			{

				CheckSlide(b);
			}

			else
			{
				MoveLeft ();
			}
		}

	
	}


	void CheckSlide (Boundary b)
	{

		
		switch (b.PreferredSlideDirection)
		{
		case Boundary.Direction.Left:
			MoveLeft ();
			break;
		case Boundary.Direction.Right:
			MoveRight ();
			break;
		case Boundary.Direction.Up:
			MoveUp ();
			break;
		case Boundary.Direction.Down:
			MoveDown ();
			break;
		}
	}
	void MoveDown ()
	{

		Boundary b = CheckHitGetBoundary(Vector3.down);

		
		if (!CheckHit(Vector3.down)) _trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
		else 
		{
			if (b != null)
			{
				Debug.Log ("checking down slide");
				CheckSlide(b);
			}

			else  if (!CheckHit(Vector3.right))   _trans.localPosition += new Vector3 (MovementSpeed,0, 0);
		}
	}

	void MoveLeft ()
	{
		if (!CheckHit(Vector3.left))	_trans.localPosition += new Vector3 (-MovementSpeed,0, 0);
		else if (!CheckHit(Vector3.down)) _trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
	}

	void MoveRight ()
	{
		if (!CheckHit(Vector3.right))_trans.localPosition += new Vector3 (MovementSpeed,0, 0);
		else if (!CheckHit(Vector3.up)) _trans.localPosition += new Vector3 (0,MovementSpeed, 0);
	}

	void FireAction ()
	{

		if (OnRoomLocation != null)
		{
			Debug.Log ("Action Fired");
			//TODO
			GameController.instance.PlayerActivatedLocation (GetComponent<Player>(), OnRoomLocation.roomObjectType);

		}
	}
	
	void OnCollisionEnter (Collision c)
	{

		if (c.collider.tag == "Item")
		{
			OnRoomLocation = c.collider.gameObject.GetComponent<RoomLocation>();
		}
	}

	void OnCollisionExit (Collision c)
	{
		OnRoomLocation = null;
	}

}
