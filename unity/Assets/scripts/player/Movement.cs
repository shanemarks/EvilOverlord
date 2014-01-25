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


	Boundary CheckHitGetBoundary (Vector2 dir)
	{
		/*Ray r  = new Ray(transform.position,dir);
		RaycastHit hit;
		Physics.Raycast(r, out hit,0.01f);
		
		if (hit.collider != null)
		{
			if (hit.collider.tag =="boundary")
			{

				return hit.collider.gameObject.GetComponent<Boundary> ();
			}
		}
		return null;*/
		Ray2D r;
		Vector2 startCast = new Vector2 (transform.position.x, transform.position.y-0.06f);
		startCast  = startCast + (dir.normalized * 0.05f);
		Vector2 dest = startCast + (dir.normalized * 0.05f);

		Debug.DrawLine (new Vector3(startCast.x, startCast.y, 0), new Vector3(dest.x, dest.y, 0), Color.red, 2);
		RaycastHit2D hit = Physics2D.Linecast (startCast, dest);

		if (hit.collider != null) {
			//if(hit.collider.tag == "boundary")
			//{
				return hit.collider.gameObject.GetComponent<Boundary> ();
			//}

		}
		return null;

	}

	void MoveUp()
	{

		Boundary b = CheckHitGetBoundary(Vector2.up - 2*Vector2.right); 

		FaceDirection (Boundary.Direction.Up);
	
		if (b == null) {
						_trans.localPosition += new Vector3 (-2, 1, 0).normalized * MovementSpeed;
				}

		/*else
		{
			if (b != null)
			{

				CheckSlide(b);
			}
 
			else
			{
				MoveLeft ();
			}
		}*/

	
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

		Boundary b = CheckHitGetBoundary(-Vector2.up + 2*Vector2.right);

		FaceDirection (Boundary.Direction.Down);
		if (b==null) _trans.localPosition -=  new Vector3 (-2,1, 0).normalized * MovementSpeed ;
		/*else 
		{
			if (b != null)
			{
				Debug.Log ("checking down slide");
				CheckSlide(b);
			}

			else  if (CheckHitGetBoundary(Vector2.right)!=null)   _trans.localPosition += new Vector3 (MovementSpeed,0, 0);
		}*/
	}

	void MoveLeft ()
	{
		FaceDirection (Boundary.Direction.Left);
		if (CheckHitGetBoundary(-Vector2.up - 2*Vector2.right)==null)	_trans.localPosition +=  new Vector3 (-2,-1, 0).normalized * MovementSpeed;
		//else if (CheckHitGetBoundary(-Vector2.up)!=null) _trans.localPosition += new Vector3 (0,-MovementSpeed, 0);
	}

	void MoveRight ()
	{
		FaceDirection (Boundary.Direction.Right);
		if (CheckHitGetBoundary(Vector2.up + 2*Vector2.right)==null)_trans.localPosition -= new Vector3 (-2,-1, 0).normalized * MovementSpeed;
		//else if (CheckHitGetBoundary(Vector2.up)!=null) _trans.localPosition += new Vector3 (0,MovementSpeed, 0);
	}

	void FireAction ()
	{

		if (_thePlayer.OnRoomLocation != null)
		{


			GameController.instance.PlayerActivatedLocation (_thePlayer, _thePlayer.OnRoomLocation.roomObjectType);


		}

		else
		{

			if (_thePlayer.ItemsOwned == ItemType.RealKnife || _thePlayer.ItemsOwned == ItemType.FakeKnife)
			{

				// find nearest player
				Debug.Log ("Looking for closest player to "+_thePlayer.name);
				
				float minDist = float.PositiveInfinity;
				Player closestPlayer = null;
				foreach (Player testPlayer in PlayerController.instance.Players)
				{
					if (testPlayer == _thePlayer)
						continue;

					float dist = Vector2.Distance(testPlayer.transform.position, _thePlayer.transform.position);
					Debug.Log ("Distance "+ dist);
					if (dist < minDist)
					{
						Debug.Log ("Setting closest player to "+ testPlayer.name+" ("+ dist+")");
						closestPlayer = testPlayer;
						minDist = dist;
					}
				}

				if (minDist <= GameController.instance.knifeRange)
				{
					if (_thePlayer.ItemsOwned == ItemType.RealKnife)
					{
						UIManager.instance.CreateObjectPickupAnimation (closestPlayer.transform.position,"Stabbed to death!");
						closestPlayer.KillPlayer();

						_thePlayer.DropItem();

					}
					if (_thePlayer.ItemsOwned == ItemType.FakeKnife)
					{
						UIManager.instance.CreateObjectPickupAnimation (_thePlayer.transform.position,"Fake knife breaks!");
						_thePlayer.DropItem();
					}
				}

			}
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
			_thePlayer.PlayerHeadSprite.spriteName = "NWHead";
		else if (dir == Boundary.Direction.Right )
			_thePlayer.PlayerHeadSprite.spriteName = "NEHead";
		else if (dir == Boundary.Direction.Down )
			_thePlayer.PlayerHeadSprite.spriteName = "SEHead";
		else if (dir == Boundary.Direction.Left )
			_thePlayer.PlayerHeadSprite.spriteName = "SWHead";

	}

	
	void OnCollisionEnter (Collision c)
	{
		Debug.Log ("Registering collision with " + c.collider.name);
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
