using UnityEngine;
using System.Collections.Generic;
using RaxterWorks.GamepadInputManager;
public class Movement : MonoBehaviour {
	public enum  ControllerType 
	{
		Keyboard,
		XboxLeft,
		XboxRight
	}

	public ControllerType _controller = ControllerType.Keyboard;
	public bool UseKeyboard;


	public KeyCode Up = KeyCode.W;
	public KeyCode Down = KeyCode.S;
	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Action = KeyCode.Space;

	Transform _trans;
	
	public float MovementSpeedBase = 1;
	private float MovementSpeed;

	public int ControllerNumber;

	Collider _playerCollider;
	[SerializeField] Player _thePlayer;


	void Start () 
	{
		_playerCollider = GetComponent<Collider> ();
		Debug.Log ("Starting Movement");
		_trans = gameObject.transform;
	
	}
	
	void Update () 
	{
		if (GamepadInput.GetButtonUp(Button.Select,ControllerNumber) || GamepadInput.GetButtonUp(Button.Start,ControllerNumber))
		{
			UIManager.instance.AnswerPhone ();
		}

		if (_thePlayer.IsAlive)
		{

			if (_controller == ControllerType.Keyboard)
			{
				if ((Input.GetKey(Up) && Input.GetKey(Left)) || (Input.GetKey(Down) && Input.GetKey(Right)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;

				if (Input.GetKey (Up))
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

			else if (_controller == ControllerType.XboxRight)
			{


				if ((GamepadInput.GetButton(XBOXButton.Y,ControllerNumber) && GamepadInput.GetButton(XBOXButton.X,ControllerNumber)) || (GamepadInput.GetButton(XBOXButton.A,ControllerNumber) && GamepadInput.GetButton(XBOXButton.B,ControllerNumber)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;


				if (GamepadInput.GetButton(XBOXButton.Y,ControllerNumber))
				{
					MoveUp ();
				}
				if (GamepadInput.GetButton(XBOXButton.A,ControllerNumber))
				{
					MoveDown ();
				}
				
				if (GamepadInput.GetButton(XBOXButton.X,ControllerNumber))
				{
					MoveLeft ();
				}
				
				if (GamepadInput.GetButton(XBOXButton.B,ControllerNumber))
				{
					MoveRight ();
				}
				
				if (GamepadInput.GetButton(Button.RightTrigger,ControllerNumber))
				{
					FireAction ();
				}
				
			}


			
			else if (_controller == ControllerType.XboxLeft)
			{

				
				if ((GamepadInput.GetButton(Button.DPadUp,ControllerNumber) && GamepadInput.GetButton(Button.DPadLeft,ControllerNumber)) || (GamepadInput.GetButton(Button.DPadDown,ControllerNumber) && GamepadInput.GetButton(Button.DPadRight,ControllerNumber)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;
				
				
				if (GamepadInput.GetButton(Button.DPadUp,ControllerNumber))
				{
					MoveUp ();
				}
				if (GamepadInput.GetButton(Button.DPadDown,ControllerNumber))
				{
					MoveDown ();
				}
				
				if (GamepadInput.GetButton(Button.DPadLeft,ControllerNumber))
				{
					MoveLeft ();
				}
				
				if (GamepadInput.GetButton(Button.DPadRight,ControllerNumber))
				{
					MoveRight ();
				}
				
				if (GamepadInput.GetButton(Button.LeftTrigger,ControllerNumber))
				{
					FireAction ();
				}
				
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
			if (hit.collider.tag =="boundary")
				return true;

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
			if (hit.collider.tag =="boundary")
			{

				return hit.collider.gameObject.GetComponent<Boundary> ();
			}
		}
		return null;

	}

	void MoveUp()
	{

		Boundary b = CheckHitGetBoundary(Vector3.up + 2*Vector3.left); 
	
		if (!CheckHit(Vector3.up + 2*Vector3.left )) _trans.localPosition += new Vector3 (-2,1, 0).normalized * MovementSpeed ;


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

		Boundary b = CheckHitGetBoundary(Vector3.down + 2*Vector3.right);
		
		if (!CheckHit(Vector3.down + 2*Vector3.right )) _trans.localPosition -=  new Vector3 (-2,1, 0).normalized * MovementSpeed ;
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
		if (!CheckHit(Vector3.down + 2*Vector3.left))	_trans.localPosition +=  new Vector3 (-2,-1, 0).normalized * MovementSpeed;
		else if (!CheckHit(Vector3.down)) _trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
	}

	void MoveRight ()
	{
		if (!CheckHit(Vector3.up + 2*Vector3.right))_trans.localPosition -= new Vector3 (-2,-1, 0).normalized * MovementSpeed;
		else if (!CheckHit(Vector3.up)) _trans.localPosition += new Vector3 (0,MovementSpeed, 0);
	}

	void FireAction ()
	{

		if (_thePlayer.OnRoomLocation != null)
		{

			GameController.instance.PlayerActivatedLocation (_thePlayer, _thePlayer.OnRoomLocation.roomObjectType);
		}
		else
		{

			GameController.instance.PlayerActivatedItem(_thePlayer);

		}

	}
	
	void OnCollisionEnter (Collision c)
	{


		if (c.collider.tag == "Item")
		{
			if (_thePlayer.OnRoomLocation != null)
			{	
				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
			}

			RoomLocation r = c.collider.gameObject.GetComponent<RoomLocation>();
			bool occupied = false;

			foreach (Player p in PlayerController.instance.Players)
			{

	
				if (p.OnRoomLocation == r)
				{

					occupied = true;
				}
			}

			if (!occupied)
			{

				_thePlayer.OnRoomLocation = r;
				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color = _thePlayer.PlayerColor;
			}
		}
	}

	void OnCollisionExit (Collision c)
	{
		if (c.collider.tag != "Player")
		{
			if (_thePlayer.OnRoomLocation != null)
			{
				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
				_thePlayer.OnRoomLocation = null;
			}
		}
	}

}
