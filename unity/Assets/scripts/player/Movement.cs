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

	private bool movedThisUpdate; //check if we have moved this update;

	public event System.Action OnMoveEvent,OnStopEvent;


	public KeyCode Up = KeyCode.W;
	public KeyCode Down = KeyCode.S;
	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Action = KeyCode.Space;

	Transform _trans;
	
	public float MovementSpeedBase = 1;
	private float MovementSpeed;

	public int ControllerNumber;

	private Collider2D lastCollider;
	private bool enableLeave;

	Collider _playerCollider;
	[SerializeField] Player _thePlayer;

	public bool IsMoving;

	void Start () 
	{
		_playerCollider = GetComponent<Collider> ();
		Debug.Log ("Starting Movement");
		_trans = gameObject.transform;

	
	}
	
	void Update () 
	{
	
		movedThisUpdate = false;
//		if ((_controller == ControllerType.XboxLeft && GamepadInput.GetButtonUp(Button.Select, ControllerNumber)) || 
//		    (_controller == ControllerType.XboxRight && GamepadInput.GetButtonUp(Button.Start, ControllerNumber)))
//		{
//			Debug.Log("AnswerPhone - frame "+Time.frameCount);
//			UIManager.instance.AnswerPhone ();
//		}
//		UIManager.instance.AnswerPhone ();
	
		if (_thePlayer.IsAlive)
		{

			if (_controller == ControllerType.Keyboard)
			{
				if ((Input.GetKey(Up) && Input.GetKey(Left)) || (Input.GetKey(Down) && Input.GetKey(Right)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;

				MovementSpeed *= (Time.deltaTime*100);
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

				MovementSpeed *= (Time.deltaTime*100);

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
				
				if (GamepadInput.GetButtonDown(Button.RightTrigger,ControllerNumber))
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
				MovementSpeed *= (Time.deltaTime*100);
				
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
				
				if (GamepadInput.GetButtonDown(Button.LeftTrigger,ControllerNumber))
				{
					FireAction ();
				}
				
			}
		}






		BoxCollider2D box = collider2D as BoxCollider2D;

//		Physics2D.OverlapAreaAll(box.center-box.size/2, box.center+box.size/2);
		
//		Debug.DrawLine(transform.position, transform.TransformPoint(transform.InverseTransformPoint(transform.position) + (Vector3)(box.center-box.size/2)));
//		Debug.DrawLine(GetRelativePositionInWorldSpace(box.center+box.size/2), GetRelativePositionInWorldSpace(box.center-box.size/2));

		
		Vector3 center = GetRelativePositionInWorldSpace (box.center);

		Collider2D [] colliders = Physics2D.OverlapAreaAll(
									GetRelativePositionInWorldSpace(box.center+box.size/2),
						        	GetRelativePositionInWorldSpace(box.center-box.size/2));

		
		Debug.DrawLine(GetRelativePositionInWorldSpace(box.center+box.size/2),
		               GetRelativePositionInWorldSpace(box.center-box.size/2));

		float minDist = float.PositiveInfinity;
		RoomLocation closestLocation = null;

		foreach (Collider2D coll in colliders)
		{
			RoomLocation roomLocation = coll.gameObject.GetComponent<RoomLocation>();

			if (roomLocation != null)
			{
				float dist = Vector2.Distance(transform.position, roomLocation.transform.position);
//				Debug.Log ("RoomLocation hit ("+_thePlayer.Index+")" + roomLocation.roomObjectType +" - "+dist);


				if (dist < minDist)
				{
					minDist = dist;
					closestLocation = roomLocation;
				}
			}
		}
//		Debug.Log ("RoomLocation hit ---");

		if (_thePlayer.IsAlive == false)
		{
			closestLocation = null;
		}

		if (closestLocation != null) // we are over a location
		{
			if (closestLocation.occupiedPlayer == null) // it is unoccupied
			{
				// occupy it

				// unoccupy the last room
				if (_thePlayer.OnRoomLocation != null)
					_thePlayer.OnRoomLocation.occupiedPlayer = null;

				_thePlayer.OnRoomLocation = closestLocation;
				closestLocation.occupiedPlayer = _thePlayer;

			}
			else
			{
				// it is occupied, do nothing
			}
		}
		else // we are not over a location
		{
			// unoccupy the last room
			if (_thePlayer.OnRoomLocation != null)
				_thePlayer.OnRoomLocation.occupiedPlayer = null;

			_thePlayer.OnRoomLocation = null;

		}


		if (!movedThisUpdate && IsMoving)
		{
			IsMoving = false;
			Debug.Log  ("Firing Movement Stop Event");

			if (OnStopEvent !=null)
			{
				OnStopEvent ();
			}
		}
	

	}


	Vector3 GetRelativePositionInWorldSpace (Vector3 relative)
	{
		return transform.TransformPoint(transform.InverseTransformPoint(transform.position) + relative);
	}




	Boundary CheckHitGetBoundary (Vector2 dir)
	{
	
				Ray2D r;
				Vector2 startCast = new Vector2 (transform.position.x, transform.position.y - 0.06f);
				startCast = startCast + (dir.normalized * 0.05f);
				Vector2 dest = startCast + (dir.normalized * 0.05f);

				Debug.DrawLine (new Vector3 (startCast.x, startCast.y, 0), new Vector3 (dest.x, dest.y, 0), Color.red, 2);
				RaycastHit2D[] hit = Physics2D.LinecastAll (startCast, dest);

		for (int i = 0; i < hit.Length; i++) {
			if (hit [i].collider != null && hit[i].collider.tag != "Player") {
								//if(hit.collider.tag == "boundary")
								//{
								return hit[i].collider.gameObject.GetComponent<Boundary> ();
								//}

			}
		}
		return null;

	}

	void MoveUp()
	{
		CheckMoveEvent ();	
		Boundary b = CheckHitGetBoundary(Vector2.up - 2*Vector2.right); 

		FaceDirection (Boundary.Direction.Up);
	
		if (b == null) {
						_trans.localPosition += new Vector3 (-2, 1, 0).normalized * MovementSpeed;
				}
	}

	void CheckMoveEvent ()
	{
		movedThisUpdate = true;
		if (!IsMoving)
		{
			Debug.Log ("Firing Move Event");
			IsMoving = true;
			if (OnMoveEvent != null)
			{
				OnMoveEvent ();
			}
		}
	}


	void MoveDown ()
	{
	 	CheckMoveEvent ();

		Boundary b = CheckHitGetBoundary(-Vector2.up + 2*Vector2.right);

		FaceDirection (Boundary.Direction.Down);
		if (b==null) _trans.localPosition -=  new Vector3 (-2,1, 0).normalized * MovementSpeed ;
		}

	void MoveLeft ()
	{
		CheckMoveEvent ();
		FaceDirection (Boundary.Direction.Left);
		if (CheckHitGetBoundary(-Vector2.up - 2*Vector2.right)==null)	_trans.localPosition +=  new Vector3 (-2,-1, 0).normalized * MovementSpeed;
	}

	void MoveRight ()
	{
		CheckMoveEvent ();
		FaceDirection (Boundary.Direction.Right);
		if (CheckHitGetBoundary(Vector2.up + 2*Vector2.right)==null)_trans.localPosition -= new Vector3 (-2,-1, 0).normalized * MovementSpeed;
	}

	void FireAction ()
	{
		
		//if (_thePlayer.ItemsOwned == PickupType.None || !GameController.instance.PlayerActivatedItem(_thePlayer))
		//{
			if (_thePlayer.OnRoomLocation != null)
			{

				GameController.instance.PlayerActivatedLocation (_thePlayer, _thePlayer.OnRoomLocation.roomObjectType);
			}
	//	}

	}

	void FaceDirection (Boundary.Direction dir) 
	{
		if (dir == Boundary.Direction.Left || dir == Boundary.Direction.Right) {
						_thePlayer.PlayerSprite.spriteName = "NE&SWBody";
			_thePlayer.FrontFootSprite.transform.localPosition = new Vector3 (22.2f, -59.32f, 0);
			_thePlayer.BackFootSprite.transform.localPosition = new Vector3 (-22.2f, -36.6f, 0);
	

				} else {
			_thePlayer.PlayerSprite.spriteName = "NW&SEBody";
			_thePlayer.FrontFootSprite.transform.localPosition = new Vector3 (-22.2f, -59.32f, 0);
			_thePlayer.BackFootSprite.transform.localPosition = new Vector3 (22.2f, -36.6f, 0);
				}

		if (dir == Boundary.Direction.Up)
		{
			_thePlayer.PlayerHeadSprite.spriteName = "NWHead";

		
		}
		else if (dir == Boundary.Direction.Right )
		{
			_thePlayer.PlayerHeadSprite.spriteName = "NEHead";

	
		

		}
		else if (dir == Boundary.Direction.Down )
		{
			_thePlayer.PlayerHeadSprite.spriteName = "SEHead";

			
		}
		else if (dir == Boundary.Direction.Left )
		{
			_thePlayer.PlayerHeadSprite.spriteName = "SWHead";
					
		}

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
//	void OnTriggerExit2D (Collider2D c)
//	{
//		if (c.tag != "Item")
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
