using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;

	public Camera UICamera;

	public UILabel TextInfo;

	public UISprite [] PlayerIcons;

	public string ALIVE_ICON = "Alive",
				  DEAD_ICON = "Dead",
				  CRATE_ICON = "Holding";
	public UIPanel  Transient; // holds aniamtion effects;


	public GameObject ObjectPickupPrefab;

	void Start ()
	{

	}

	void Update ()
	{
		UpdateCharacterIcons();

		if (Input.GetKeyDown(KeyCode.F1))
		{
			AnswerPhone ();
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			ReplayInstruction();
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			GameController.instance.showDebugOutput = !GameController.instance.showDebugOutput;
		}
	}
	
	public void AnswerPhone ()
	{
		Debug.Log ("Answer Phone");
		if (VoiceSpeaker.GetVoiceState() ==0)	GameController.instance.PlayInstructions();
	}

	public void ReplayInstruction()
	{
		GameController.instance.SayCurrentInstruction();
	}

	void  StartGame ()
	{
		Debug.Log ("Start Game");

	}

	public void UpdateCharacterIcons ()
	{
		foreach (Player p in PlayerController.instance.Players)
		{

			if (p.IsAlive)
			{
				p.PlayerIcon.spriteName= ALIVE_ICON;
			}
			
			else
			{
				p.PlayerIcon.spriteName = DEAD_ICON;
				return;
			}

			if (p.ItemsOwned != PickupType.None)
			{
				p.PlayerIcon.spriteName = CRATE_ICON;
		
			}
	
			p.PlayerIcon.Update ();

		}

	}

	public void CreateObjectPickupAnimation (Vector3 v, string s)
	{
		GameObject go = 	NGUITools.AddChild(Transient.gameObject,ObjectPickupPrefab);
		go.transform.position = v;


		go.GetComponent<UILabel>().text = s;

	}
	public void CreateObjectPickupAnimation (Vector3 v, string s, Color c)
	{
		GameObject go = 	NGUITools.AddChild(Transient.gameObject,ObjectPickupPrefab);
		go.transform.position = v;
		go.GetComponent<UILabel>().color = c;
		
		go.GetComponent<UILabel>().text = s;
		
	}


	void ResetGame ()
	{
		Debug.Log ("Reset Game");
		Application.LoadLevel(Application.loadedLevel);
	}
}
