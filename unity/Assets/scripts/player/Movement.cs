using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RaxterWorks.GamepadInputManager;

public class Movement : MonoBehaviour {

	public enum  ControllerType 
	{
		Keyboard,
		Xbox
	}

	public ControllerType _controller = ControllerType.Keyboard;

	private bool movedThisUpdate; //check if we have moved this update;

	public event System.Action OnMoveEvent,OnStopEvent;
	
	KeyCode UpKey { get { return _thePlayer.Index == 3 ? Player4Up : Player3Up; } }
	KeyCode DownKey { get { return _thePlayer.Index == 3 ? Player4Down : Player3Down; } }
	KeyCode LeftKey { get { return _thePlayer.Index == 3 ? Player4Left : Player3Left; } }
	KeyCode RightKey { get { return _thePlayer.Index == 3 ? Player4Right : Player3Right; } }
	KeyCode ActionKey { get { return _thePlayer.Index == 3 ? Player4Action : Player3Action; } }
	KeyCode PlayKey { get { return _thePlayer.Index == 3 ? Player4Play : Player3Play; } }
	
	KeyCode Player3Up = KeyCode.W;
	KeyCode Player3Down = KeyCode.S;
	KeyCode Player3Left = KeyCode.A;
	KeyCode Player3Right = KeyCode.D;
	KeyCode Player3Action = KeyCode.Space;
	KeyCode Player3Play = KeyCode.Tab;
					 
	KeyCode Player4Up = KeyCode.UpArrow;
	KeyCode Player4Down = KeyCode.DownArrow;
	KeyCode Player4Left = KeyCode.LeftArrow;
	KeyCode Player4Right = KeyCode.RightArrow;
	KeyCode Player4Action = KeyCode.Return;
	KeyCode Player4Play = KeyCode.Backspace;
	
	KeyCode KeyboardPassPlayer0Yellow = KeyCode.U;
	KeyCode KeyboardPassPlayer1Blue = KeyCode.H;
	KeyCode KeyboardPassPlayer2Green = KeyCode.J;
	KeyCode KeyboardPassPlayer3Red = KeyCode.K;
	
	PadButton playButton = PadButton.LeftTrigger;
	PadButton playButtonAlt = PadButton.LeftBumper;

	PadButton actionButton = PadButton.RightTrigger;
	PadButton actionButtonAlt = PadButton.RightBumper;

	
	PadButton PassPlayer0Yellow = XBOXButton.Y;
	PadButton PassPlayer1Blue = XBOXButton.X;
	PadButton PassPlayer2Green = XBOXButton.A;
	PadButton PassPlayer3Red = XBOXButton.B;

	Transform _trans;
	
	public float MovementSpeedBase = 1;
	private float MovementSpeed;

	public int ControllerNumber;

	private Collider2D lastCollider;

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
				if ((Input.GetKey(UpKey) && Input.GetKey(LeftKey)) || (Input.GetKey(DownKey) && Input.GetKey(RightKey)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;

				MovementSpeed *= (Time.deltaTime*100);
				if (Input.GetKey (UpKey))
				{
					MoveUp ();
				}
				if (Input.GetKey(DownKey))
				{
					MoveDown ();
				}
				if (Input.GetKey(LeftKey))
				{
					MoveLeft ();
				}

				if (Input.GetKey(RightKey))
				{
					MoveRight ();
				}

			}

			else if (_controller == ControllerType.Xbox)
			{


				if ((GamepadInput.GetButton(XBOXButton.Y,ControllerNumber) && GamepadInput.GetButton(XBOXButton.X,ControllerNumber)) || (GamepadInput.GetButton(XBOXButton.A,ControllerNumber) && GamepadInput.GetButton(XBOXButton.B,ControllerNumber)))
					MovementSpeed = MovementSpeedBase * Mathf.Sqrt (3) / 2;
				else
					MovementSpeed = MovementSpeedBase;

				MovementSpeed *= (Time.deltaTime*100);

				float deadZone = 0.1f;

				Vector2 leftAnalog = GamepadInput.GetXYAxis(XYAxis.LeftAnalog, ControllerNumber);
				leftAnalog.y *= -1;

				if (leftAnalog.magnitude > 0.15)
					MoveVector(leftAnalog);


			}

			bool actionUp = 
				_controller == ControllerType.Keyboard 
					? Input.GetKeyUp(ActionKey) 
					: GamepadInput.GetButtonDown(actionButton, ControllerNumber) || GamepadInput.GetButtonDown(actionButtonAlt, ControllerNumber);

			if (actionUp)
			{
				FireAction ();
			}


			if (_thePlayer.hasPhone)
			{
				bool playVoiceDown = 
					_controller == ControllerType.Keyboard  
						? Input.GetKeyDown(PlayKey)
						: GamepadInput.GetButtonDown(playButton, ControllerNumber) || GamepadInput.GetButtonDown(playButtonAlt, ControllerNumber);

				if (playVoiceDown && VoiceSpeaker.GetVoiceState() == 0)
				{
					UIManager.instance.ReplayInstruction();
					
					_thePlayer.canPassPhone = true;
				}
				
				if (_thePlayer.canPassPhone)
				{
					int playerIndexToPassTo = -1;
					bool [] passDown = new bool [4] {false,false,false,false};
					if (_controller == ControllerType.Keyboard)
					{
						passDown[0] = Input.GetKeyDown(KeyboardPassPlayer0Yellow);
						passDown[1] = Input.GetKeyDown(KeyboardPassPlayer1Blue);
						passDown[2] = Input.GetKeyDown(KeyboardPassPlayer2Green);
						passDown[3] = Input.GetKeyDown(KeyboardPassPlayer3Red);
					}
					else
					{
						passDown[0] = GamepadInput.GetButtonDown(PassPlayer0Yellow, ControllerNumber);
						passDown[1] = GamepadInput.GetButtonDown(PassPlayer1Blue, ControllerNumber);
						passDown[2] = GamepadInput.GetButtonDown(PassPlayer2Green, ControllerNumber);
						passDown[3] = GamepadInput.GetButtonDown(PassPlayer3Red, ControllerNumber);
					}

					for (int p = 0 ; p < 4 ; p++)
					{
						Debug.Log(p+" "+passDown[p] +" : "+PlayerController.instance.Players[p].CanRecievePhone());
						if (passDown[p] && PlayerController.instance.Players[p].CanRecievePhone())
						{
							_thePlayer.PassPhone(PlayerController.instance.Players[p]);
							break;
						}
					}
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


//	IEnumerator WaitForVoice ()
//	{
//		while (VoiceSpeaker.GetVoiceState() != 0) // then is talking
//		{
//			if (!_thePlayer.hasPhone) // somehow passed the phone
//			{
//				_thePlayer.canPassPhone = false;
//				yield break;
//			}
//			yield return null;
//		}
//		
//		_thePlayer.canPassPhone = true;
//	}


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

	
	
	void MoveVector(Vector2 move)
	{
		CheckMoveEvent ();	
		Boundary b = CheckHitGetBoundary(move); 

		PolarVector2 polar = new PolarVector2(move);
		
		polar.angle += 360;
		polar.angle %= 360;

		      if (polar.angle >= 0 && polar.angle < 90)
			FaceDirection (Boundary.Direction.Right);
		else if (polar.angle >= 90 && polar.angle < 180)
			FaceDirection (Boundary.Direction.Down);
		else if (polar.angle >= 180 && polar.angle < 270)
			FaceDirection (Boundary.Direction.Left);
		else if (polar.angle >= 270 && polar.angle < 360)
			FaceDirection (Boundary.Direction.Up);
		
		if (b == null) _trans.localPosition += (Vector3)move * MovementSpeed;
	}

	void MoveUp(float amount = 1)
	{
		CheckMoveEvent ();	
		Boundary b = CheckHitGetBoundary(Vector2.up - 2*Vector2.right); 

		FaceDirection (Boundary.Direction.Up);
	
		if (b == null) _trans.localPosition += amount*new Vector3 (-2, 1, 0).normalized * MovementSpeed;
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


	void MoveDown (float amount = 1)
	{
	 	CheckMoveEvent ();

		Boundary b = CheckHitGetBoundary(-Vector2.up + 2*Vector2.right);

		FaceDirection (Boundary.Direction.Down);
		if (b==null) _trans.localPosition -=  amount*new Vector3 (-2,1, 0).normalized * MovementSpeed ;
		}

	void MoveLeft (float amount = 1)
	{
		CheckMoveEvent ();
		FaceDirection (Boundary.Direction.Left);
		if (CheckHitGetBoundary(-Vector2.up - 2*Vector2.right) == null)	_trans.localPosition +=  amount*new Vector3 (-2,-1, 0).normalized * MovementSpeed;
	}

	void MoveRight (float amount = 1)
	{
		CheckMoveEvent ();
		FaceDirection (Boundary.Direction.Right);
		if (CheckHitGetBoundary(Vector2.up + 2*Vector2.right) == null)	_trans.localPosition -= amount*new Vector3 (-2,-1, 0).normalized * MovementSpeed;
	}

	void FireAction ()
	{
		if (ScoreController.instance.roundEnd) _thePlayer.enableLeave = true;
		
		if (_thePlayer.ItemsOwned == PickupType.None || !GameController.instance.PlayerActivatedItem(_thePlayer))
		{
			if (_thePlayer.OnRoomLocation != null)
			{

				GameController.instance.PlayerActivatedLocation (_thePlayer, _thePlayer.OnRoomLocation.roomObjectType);
				return;
			}
		}

		else if (_thePlayer.IsAlive)
		{
			GameController.instance.PlayerActivatedItem (_thePlayer);
		}
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
