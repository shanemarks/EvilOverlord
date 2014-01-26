using UnityEngine;
using System.Collections;
using RaxterWorks.GamepadInputManager;
using System.Collections.Generic;

public class PlayerController : SingletonBehaviour<PlayerController>
{

	public GameObject PlayerPrefab;
	private GameObject[] gos;


	//hardcoded controls:



     private KeyCode[] Up = {KeyCode.W, KeyCode.I, 	KeyCode.Keypad8, KeyCode.UpArrow};
	 private KeyCode[] Down = {KeyCode.S, KeyCode.K, KeyCode.Keypad2, KeyCode.DownArrow};
	private KeyCode[] Left = {KeyCode.A, KeyCode.J,KeyCode.Keypad4, KeyCode.LeftArrow};
	private KeyCode[] Right = {KeyCode.D, KeyCode.L, KeyCode.Keypad6, KeyCode.RightArrow};
	 private KeyCode[] Action = {KeyCode.LeftControl,KeyCode.Space, KeyCode.Keypad5, KeyCode.RightControl};

	[SerializeField] private Color[] PlayerColors;


	public int PlayerCount = 4;
	public int MovementSpeed = 5;
	public Player[] Players;


	public bool RoundWon;
	public List<Player> _livingPlayers;
	public class yComparerClass : IComparer {
		int IComparer.Compare(object x, object y) {
			float depthx, depthy;

			if(((GameObject)x).GetComponent<UIPanel>() != null)
				depthx = ((GameObject)x).GetComponent<UIPanel>().transform.position.y;
			else if(((GameObject)x).GetComponent<UISprite>() != null)
				depthx = ((GameObject)x).GetComponent<UISprite>().transform.position.y;
			else
				return 1;
			

			if(((GameObject)y).GetComponent<UIPanel>() != null)
				depthy = ((GameObject)y).GetComponent<UIPanel>().transform.position.y;
			else if(((GameObject)y).GetComponent<UISprite>() != null)
				depthy = ((GameObject)y).GetComponent<UISprite>().transform.position.y;
			else
				return -1;

			if (depthx > depthy)
				return -1;
			else if (depthx < depthy)
				return 1;
			else
				return 0;

		}
	}

	void Start ()
	{


		Debug.Log ("Starting Player Controller");

		Players = new Player[PlayerCount];
		Debug.Log (Up.Length);
		for (int i = 0 ; i < PlayerCount; i++)
		{
			GameObject go = NGUITools.AddChild(UIManager.instance.PlayerPanel.gameObject, PlayerPrefab);

			Players[i] = go.GetComponent<Player>();
			Players[i].name = "player" + (i+1);
			Players[i]._movement.Up = Up[i];
			Players[i]._movement.Down = Down[i];
			Players[i]._movement.Left = Left[i];		
			Players[i]._movement.Right = Right[i];	
			Players[i]._movement.Action = Action[i];
			Players[i].Index = i;
			Players[i].StartPos = new Vector3 ((i+4)*125,-50,0);
			Players[i].transform.localPosition = Players[i].StartPos;
			Players[i].PlayerColor = PlayerColors[i];
			Players[i].PlayerSprite.color = PlayerColors[i];
			Players[i]._movement.MovementSpeedBase = MovementSpeed;
			Players[i].PlayerIcon = UIManager.instance.PlayerIcons[i];

			Players[i].IsAlive = true;
			Players[i].ItemsOwned = PickupType.None;
			Players[i].GetComponent<UIPanel>().depth = 100-i;
			UIManager.instance.PlayerIconBorders[i].color = PlayerColors[i];

//			if (Input.GetJoystickNames().Length > 1)
//			{
//				if (i == 0)
//				{
//					Players[i]._movement._controller = Movement.ControllerType.XboxRight;
//					Players[i]._movement.ControllerNumber = 0;
//				}
//				if (i == 1)
//				{
//					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
//					Players[i]._movement.ControllerNumber = 0;
//				}
//			}
//			if (Input.GetJoystickNames().Length >=2)
//			{
//				if (i == 2)
//				{
//					Players[i]._movement._controller = Movement.ControllerType.XboxRight;
//					Players[i]._movement.ControllerNumber = 1;
//				}
//				if (i == 3)
//				{
//					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
//					Players[i]._movement.ControllerNumber = 1;
//				}
//			}

			Debug.Log ("Setting up controllers ("+Input.GetJoystickNames().Length+")");
			if (Input.GetJoystickNames().Length == 0)
			{
				Players[i]._movement._controller = Movement.ControllerType.Keyboard;
			}
			if (Input.GetJoystickNames().Length >= 1)
			{
				if (i == 1)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
					Players[i]._movement.ControllerNumber = 0;
				}
			}
			if (Input.GetJoystickNames().Length >= 2)
			{
				if (i == 2)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
					Players[i]._movement.ControllerNumber = 1;
				}
			}
			if (Input.GetJoystickNames().Length >= 3)
			{
				if (i == 3)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
					Players[i]._movement.ControllerNumber = 2;
				}
			}
			if (Input.GetJoystickNames().Length >= 4)
			{
				if (i == 0)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
					Players[i]._movement.ControllerNumber = 3;
				}
			}
			
		}

		gos = FindObjectsOfType (typeof(GameObject)) as GameObject[];

 		UIManager.instance.UpdateCharacterIcons ();
	}

	void Update ()
	{
		Player lowest = Players[0];
		Player highest = Players[0];

		float baseWidth;
		float adjFactor = Mathf.Sqrt (3);
		float vertexRatio = 18 / 25;

		//lowest.GetComponent<UIPanel> ().depth = 100;
		baseWidth = 144;

		ArrayList SceneObjects = new ArrayList();
		IComparer yComparer = new yComparerClass();

		foreach(GameObject go in gos) {
			if ((go.tag == "Player" || go.tag == "Item") && (go.GetComponent<UIPanel>() != null || go.GetComponent<UISprite>() != null)) {
				//Debug.Log(go.name);
				SceneObjects.Add(go); }
		}

		/*for (int i = 0 ; i < PlayerCount; i++)
		{
			SceneObjects.Add(Players[i]);
		}*/

		SceneObjects.Sort(yComparer);

		for (int i = 0; i < SceneObjects.Count; i++) {
			if(((GameObject)(SceneObjects[i])).GetComponent<UIPanel>() != null)
				((GameObject)(SceneObjects[i])).GetComponent<UIPanel>().depth = 100 + i;
			else
				((GameObject)(SceneObjects[i])).GetComponent<UISprite>().depth = 100 + i;
		}
	}

}
