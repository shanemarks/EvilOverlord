using UnityEngine;
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





	void Start () {
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
	}


}
