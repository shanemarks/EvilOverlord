using UnityEngine;
using System.Collections.Generic;
using RaxterWorks.GamepadInputManager;
public class Movement : MonoBehaviour 
{
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

//	Transform _trans;
	
	public float MovementSpeedBase = 1;

	public int ControllerNumber;

	Collider _playerCollider;

	[SerializeField] 
	Player _thePlayer;

	
	public enum Direction 
	{
		Up,
		Down,
		Left, 
		Right
	}

	void Start () 
	{
		_playerCollider = GetComponent<Collider> ();
		Debug.Log ("Starting Movement");
//		_trans = gameObject.transform;
	
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

				if (Input.GetKey (Up))
				{
					Move (Direction.Up);
				}
				if (Input.GetKey(Down))
				{
					Move (Direction.Down);
				}
				if (Input.GetKey(Left))
				{
					Move (Direction.Left);
				}

				if (Input.GetKey(Right))
				{
					Move (Direction.Right);
				}

				if (Input.GetKeyUp(Action))
				{
					FireAction ();
				}
			}

			else if (_controller == ControllerType.XboxRight)
			{


//				if ((GamepadInput.GetButton(XBOXButton.Y,ControllerNumber) && GamepadInput.GetButton(XBOXButton.X,ControllerNumber)) || (GamepadInput.GetButton(XBOXButton.A,ControllerNumber) && GamepadInput.GetButton(XBOXButton.B,ControllerNumber)))
//					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
//				else
//					MovementSpeed = MovementSpeedBase;


				if (GamepadInput.GetButton(XBOXButton.Y,ControllerNumber))
				{
					Move (Direction.Up);
				}
				if (GamepadInput.GetButton(XBOXButton.A,ControllerNumber))
				{
					Move (Direction.Down);
				}
				
				if (GamepadInput.GetButton(XBOXButton.X,ControllerNumber))
				{
					Move (Direction.Left);
				}
				
				if (GamepadInput.GetButton(XBOXButton.B,ControllerNumber))
				{
					Move (Direction.Right);
				}
				
				if (GamepadInput.GetButton(Button.RightTrigger,ControllerNumber))
				{
					FireAction ();
				}
				
			}


			
			else if (_controller == ControllerType.XboxLeft)
			{

				
//				if ((GamepadInput.GetButton(Button.DPadUp,ControllerNumber) && GamepadInput.GetButton(Button.DPadLeft,ControllerNumber)) || (GamepadInput.GetButton(Button.DPadDown,ControllerNumber) && GamepadInput.GetButton(Button.DPadRight,ControllerNumber)))
//					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
//				else
//					MovementSpeed = MovementSpeedBase;
				
				
				if (GamepadInput.GetButton(Button.DPadUp,ControllerNumber))
				{
					Move (Direction.Up);
				}
				if (GamepadInput.GetButton(Button.DPadDown,ControllerNumber))
				{
					Move (Direction.Down);
				}
				
				if (GamepadInput.GetButton(Button.DPadLeft,ControllerNumber))
				{
					Move (Direction.Left);
				}
				
				if (GamepadInput.GetButton(Button.DPadRight,ControllerNumber))
				{
					Move (Direction.Right);
				}
				
				if (GamepadInput.GetButton(Button.LeftTrigger,ControllerNumber))
				{
					FireAction ();
				}
				
			}
		}

		
	

	}


	 /*bool CheckHit (Vector2 dir)
	{

		/*Ray r  = new Ray(transform.position,dir);
		RaycastHit hit;
		Physics.Raycast(r, out hit,0.01f);

		if (hit.collider != null)
		{
			if (hit.collider.tag =="boundary")
				return true;

		}
		return false;
		Ray2D r;
		Vector2 startCast = new Vector2 (transform.position.x, transform.position.y - 100);
		RaycastHit2D hit = Physics2D.Linecast (startCast, startCast + dir);
		
		if (hit.collider != null) {
			if(hit.collider.tag == "boundary")
				return true;
			
		}

	}*/


//	Boundary CheckHitGetBoundary (Vector2 dir)
//	{
//		/*Ray r  = new Ray(transform.position,dir);
//		RaycastHit hit;
//		Physics.Raycast(r, out hit,0.01f);
//		
//		if (hit.collider != null)
//		{
//			if (hit.collider.tag =="boundary")
//			{
//
//				return hit.collider.gameObject.GetComponent<Boundary> ();
//			}
//		}
//		return null;*/
//		Ray2D r;
//		Vector2 startCast = new Vector2 (transform.position.x, transform.position.y-0.06f);
//		startCast  = startCast + (dir.normalized * 0.05f);
//		Vector2 dest = startCast + (dir.normalized * 0.05f);
//
//		Debug.DrawLine (new Vector3(startCast.x, startCast.y, 0), new Vector3(dest.x, dest.y, 0), Color.red, 2);
//		RaycastHit2D hit = Physics2D.Linecast (startCast, dest);
//
//		if (hit.collider != null) {
//			//if(hit.collider.tag == "boundary")
//			//{
//				return hit.collider.gameObject.GetComponent<Boundary> ();
//			//}
//
//		}
//		return null;
//
//	}


	void Move(Direction moveDirection)
	{
		FaceDirection (moveDirection);

		Vector2 moveVector = Vector2.zero;


		switch (moveDirection)
		{
		case Direction.Up:
			moveVector = Vector2.up-2*Vector2.right;
			break;
		case Direction.Down:
			moveVector = -Vector2.up+2*Vector2.right;
			break;
		case Direction.Right:
			moveVector = Vector2.up+2*Vector2.right;
			break;
		case Direction.Left:
			moveVector = -Vector2.up-2*Vector2.right;
			break;
		}
		moveVector.Normalize();
			
		float ActualMovementSpeed;

		if ((moveDirection == Direction.Up && moveDirection == Direction.Left) || (moveDirection == Direction.Down && moveDirection == Direction.Right))
			ActualMovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
		else
			ActualMovementSpeed = MovementSpeedBase;

		moveVector *= ActualMovementSpeed;

		// eyes white over...

		float _searchRadius = 0.08f;
		float _circlePoints = 6;
		float _boundrySearchSteps = 8;

		
		Vector3 positionAdjust = Vector3.zero;
		float maxMagnitude = 0f;

		for (int i = 0 ; i < _circlePoints ; i++)
		{
			
			Vector3 direction = Quaternion.AngleAxis(360 * i/_circlePoints, Vector3.forward)*Vector3.up;
//			
			direction.Normalize();

			direction.y *= 0.5f;

			direction *= _searchRadius;

			Debug.DrawRay(transform.position, direction, Color.red);

			for (int b = 0 ; b < _boundrySearchSteps ; b++)
			{
				float relativeRayDistance =  b/_boundrySearchSteps;
				
				Vector2 searchRay = (Vector2)direction*relativeRayDistance;

				Vector2 worldPoint = (Vector2)transform.position + searchRay;
				Color debugColor = Color.green;

				if (CheckForBoundry(worldPoint))
				{
					debugColor = Color.blue;

					float positionAdjustLength = 1 - relativeRayDistance;
					Vector3 additionalPositionAdjust = positionAdjustLength * (-direction);
					
					positionAdjust += additionalPositionAdjust;
					maxMagnitude = Mathf.Max(maxMagnitude, positionAdjustLength);
					
					break;
				}

				Debug.DrawRay((Vector3)worldPoint -Vector3.up*0.01f, Vector3.up*0.02f, debugColor);
				Debug.DrawRay((Vector3)worldPoint -Vector3.left*0.01f, Vector3.left*0.02f, debugColor);



//				if (RayCastEnvironmentVertically(worldPosition + searchRay))
//				{
//					float positionAdjustLength = _landscapeBoundryBuffer - rayDistance;
//					Vector3 additionalPositionAdjust = positionAdjustLength * (-direction);
//					
//					positionAdjust += additionalPositionAdjust;
//					maxMagnitude = Mathf.Max(maxMagnitude, positionAdjustLength);
//					
//					break;
//				}
				
				
			}
		}
		
		positionAdjust.Normalize();
		positionAdjust *= maxMagnitude;

		Debug.DrawRay(transform.position, positionAdjust, Color.blue);

		transform.localPosition += (Vector3)moveVector + positionAdjust/transform.parent.lossyScale.x/16;

		
		// ... I'm not sure what happened here
	}

	bool CheckForBoundry(Vector2 worldPosition)
	{
		Collider2D [] colliders = Physics2D.OverlapPointAll(worldPosition);

		foreach (Collider2D collider in colliders)
		{
			if (collider.gameObject.GetComponent<Boundary>() != null)
			{
				return true;
			}
		}

		return false;
	}

//	void MoveUp()
//	{
//
//		Boundary b = CheckHitGetBoundary(Vector2.up - 2*Vector2.right); 
//
//		FaceDirection (Boundary.Direction.Up);
//	
//		if (b == null) {
//						_trans.localPosition += new Vector3 (-2, 1, 0).normalized * MovementSpeed;
//				}
//
//		/*else
//		{
//			if (b != null)
//			{
//
//				CheckSlide(b);
//			}
// 
//			else
//			{
//				MoveLeft ();
//			}
//		}*/
//
//	
//	}
//
//
//	void CheckSlide (Boundary b)
//	{
//
//		
//		switch (b.PreferredSlideDirection)
//		{
//		case Boundary.Direction.Left:
//			MoveLeft ();
//			break;
//		case Boundary.Direction.Right:
//			MoveRight ();
//			break;
//		case Boundary.Direction.Up:
//			MoveUp ();
//			break;
//		case Boundary.Direction.Down:
//			MoveDown ();
//			break;
//		}
//	}
//	void MoveDown ()
//	{
//
//		Boundary b = CheckHitGetBoundary(-Vector2.up + 2*Vector2.right);
//
//		FaceDirection (Boundary.Direction.Down);
//		if (b==null) _trans.localPosition -=  new Vector3 (-2,1, 0).normalized * MovementSpeed ;
//		/*else 
//		{
//			if (b != null)
//			{
//				Debug.Log ("checking down slide");
//				CheckSlide(b);
//			}
//
//			else  if (CheckHitGetBoundary(Vector2.right)!=null)   _trans.localPosition += new Vector3 (MovementSpeed,0, 0);
//		}*/
//	}
//
//	void MoveLeft ()
//	{
//		FaceDirection (Boundary.Direction.Left);
//		if (CheckHitGetBoundary(-Vector2.up - 2*Vector2.right)==null)	_trans.localPosition +=  new Vector3 (-2,-1, 0).normalized * MovementSpeed;
//		//else if (CheckHitGetBoundary(-Vector2.up)!=null) _trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
//	}
//
//	void MoveRight ()
//	{
//		FaceDirection (Boundary.Direction.Right);
//		if (CheckHitGetBoundary(Vector2.up + 2*Vector2.right)==null)_trans.localPosition -= new Vector3 (-2,-1, 0).normalized * MovementSpeed;
//		//else if (CheckHitGetBoundary(Vector2.up)!=null) _trans.localPosition += new Vector3 (0,MovementSpeed, 0);
//	}

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

	void FaceDirection (Direction dir) 
	{
		if (dir == Direction.Left || dir == Direction.Right) 
		{
			_thePlayer.PlayerSprite.spriteName = "NE&SWBody";
			_thePlayer.FrontFootSprite.transform.localPosition = new Vector3 (22.2f, -59.32f, 0);
			_thePlayer.BackFootSprite.transform.localPosition = new Vector3 (-22.2f, -36.6f, 0);
				
		} 
		else 
		{
			_thePlayer.PlayerSprite.spriteName = "NW&SEBody";
			_thePlayer.FrontFootSprite.transform.localPosition = new Vector3 (-22.2f, -59.32f, 0);
			_thePlayer.BackFootSprite.transform.localPosition = new Vector3 (22.2f, -36.6f, 0);
		}

		if (dir == Direction.Up)
			_thePlayer.PlayerHeadSprite.spriteName = "NWHead";
		else if (dir == Direction.Right )
			_thePlayer.PlayerHeadSprite.spriteName = "NEHead";
		else if (dir == Direction.Down )
			_thePlayer.PlayerHeadSprite.spriteName = "SEHead";
		else if (dir == Direction.Left )
			_thePlayer.PlayerHeadSprite.spriteName = "SWHead";

	}

//	void OnTriggerEnter2D (Collider2D c)
//	{
//		Debug.Log ("Trigger fired");
//		if (c.tag == "Item") {
//			if (_thePlayer.OnRoomLocation != null)
//			{	
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
//			}
//			
//			RoomLocation r = c.gameObject.GetComponent<RoomLocation>();
//			bool occupied = false;
//			
//			foreach (Player p in PlayerController.instance.Players)
//			{
//				
//				
//				if (p.OnRoomLocation == r)
//				{
//					
//					occupied = true;
//				}
//			}
//			
//			if (!occupied)
//			{
//				
//				_thePlayer.OnRoomLocation = r;
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color = _thePlayer.PlayerColor;
//			}
//		}
//	}
//
//
//	void OnCollisionEnter2D (Collision2D c)
//	{
//		Debug.Log ("Registering collision with " + c.collider.name);
//		if (c.collider.tag == "Item")
//		{
//			if (_thePlayer.OnRoomLocation != null)
//			{	
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
//			}
//
//			RoomLocation r = c.collider.gameObject.GetComponent<RoomLocation>();
//			bool occupied = false;
//
//			foreach (Player p in PlayerController.instance.Players)
//			{
//
//	
//				if (p.OnRoomLocation == r)
//				{
//
//					occupied = true;
//				}
//			}
//
//			if (!occupied)
//			{
//
//				_thePlayer.OnRoomLocation = r;
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color = _thePlayer.PlayerColor;
//			}
//		}
//	}
//
//	void OnCollisionExit (Collision c)
//	{
//		if (c.collider.tag != "Player")
//		{
//			if (_thePlayer.OnRoomLocation != null)
//			{
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
//				_thePlayer.OnRoomLocation = null;
//			}
//		}
//	}
//
//	void OnTriggerExit2D (Collider2D c)
//	{
//		if (c.tag != "Player")
//		{
//			if (_thePlayer.OnRoomLocation != null)
//			{
//				_thePlayer.OnRoomLocation.GetComponent<UISprite>().color =  Color.white;
//				_thePlayer.OnRoomLocation = null;
//			}
//		}
//
//	}

}
