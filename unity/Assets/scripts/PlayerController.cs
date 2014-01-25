using UnityEngine;
using System.Collections;
using RaxterWorks.GamepadInputManager;
public class PlayerController : SingletonBehaviour<PlayerController>
{

	public GameObject PlayerPrefab;


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
	void Start ()
	{


		Debug.Log ("Starting Player Controller");

		Players = new Player[PlayerCount];
		Debug.Log (Up.Length);
		for (int i = 0 ; i < PlayerCount; i++)
		{
			GameObject go = NGUITools.AddChild(UIManager.instance.PlayerPanel.gameObject, PlayerPrefab);

			Players[i] = go.GetComponent<Player>();
			Players[i].name = "player" + i;
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
			if (Input.GetJoystickNames().Length >1)
			{
				if (i == 0)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxRight;
					Players[i]._movement.ControllerNumber = 0;
				}
				if (i == 1)
				{
					Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
					Players[i]._movement.ControllerNumber = 0;
				}
			}
			if (Input.GetJoystickNames().Length >=2)
			{
				if (i == 2)
					{
						Players[i]._movement._controller = Movement.ControllerType.XboxRight;
						Players[i]._movement.ControllerNumber = 1;
					}
					if (i == 3)
					{
						Players[i]._movement._controller = Movement.ControllerType.XboxLeft;
						Players[i]._movement.ControllerNumber = 1;
					}
			}
			
		}

 		UIManager.instance.UpdateCharacterIcons ();
	}

	void Update ()
	{
		Player lowest = Players[0];
		Player highest = Players[0];

		float baseWidth;
		float adjFactor = Mathf.Sqrt (3);
		float vertexRatio = 18 / 25;

		lowest.GetComponent<UIPanel> ().depth = 100;
		baseWidth = 144;
	

		for (int i = 1 ; i < PlayerCount; i++)
		{
			if(Players[i].transform.position.y < lowest.transform.position.y) 
			{
				Players[i].GetComponent<UIPanel>().depth = lowest.GetComponent<UIPanel>().depth - 10;
				lowest = Players[i];
			}
			else if(Players[i].transform.position.y > highest.transform.position.y) 
			{
				Players[i].GetComponent<UIPanel>().depth = highest.GetComponent<UIPanel>().depth + 10;
				highest = Players[i];
			}
		}
	}

}
