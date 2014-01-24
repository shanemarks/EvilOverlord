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

	Item OnItem;

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


	void MoveUp()
	{
		Ray r  = new Ray(transform.position,new Vector3(0,1,0));
		RaycastHit hit;
		collider.Raycast(r,out hit,500);
		Debug.Log (hit.collider);
	
		_trans.localPosition += new Vector3 (0,MovementSpeed, 0);
	}

	void MoveDown ()
	{

		_trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
	}

	void MoveLeft ()
	{
		_trans.localPosition += new Vector3 (-MovementSpeed,0, 0);
	}

	void MoveRight ()
	{
		_trans.localPosition += new Vector3 (MovementSpeed,0, 0);
	}

	void FireAction ()
	{
		Debug.Log ("Action Fired");
		if (OnItem != null)
		{
			OnItem.PickUp ();
		}
	}
	
	void OnCollisionEnter (Collision c)
	{
		Debug.Log ("hit");
		if (c.collider.tag == "Item")
		{
			OnItem = c.collider.gameObject.GetComponent<Item>();
		}
	}

	void OnCollisionExit (Collision c)
	{
		OnItem = null;
	}

}
