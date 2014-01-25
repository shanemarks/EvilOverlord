using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;

	public Camera UICamera;

	public UILabel TextInfo;

	public UISprite [] PlayerIcons;
	public UISprite [] PlayerIconBorders;
	public string ALIVE_ICON = "Alive",
				  DEAD_ICON = "Dead",
				 HOLDING_KNIFE = "HoldingKnife",
				  HOLDING_MASK = "HoldingMask";  
	public UIPanel  Transient; // holds aniamtion effects;
	
	public GameObject Blood;

	public GameObject ObjectPickupPrefab, GibAnim;

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
		GameController.instance.SayInstruction();
	}

	void  StartGame ()
	{
		Debug.Log ("Start Game");

	}

	public void UpdateCharacterIcons ()
	{
		foreach (Player p in PlayerController.instance.Players)
		{
			if (!p.IsAlive)
			{
				p.PlayerIcon.spriteName= DEAD_ICON;
			}

			else if (p.IsAlive && p.ItemsOwned == ItemType.None)
			{
				p.PlayerIcon.spriteName= ALIVE_ICON;
			}
		

			else if (p.ItemsOwned != ItemType.None)
			{
				if (p.ItemsOwned == ItemType.FakeKnife )
				{
					p.PlayerIcon.spriteName = HOLDING_KNIFE;
				}
				else if (p.ItemsOwned == ItemType.RealKnife )
				{
					p.PlayerIcon.spriteName = HOLDING_KNIFE;
				}

				else if (p.ItemsOwned == ItemType.GasMask)
				{
					p.PlayerIcon.spriteName = HOLDING_MASK;
				}
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

	
	public void Gib (Vector3 v)
	{

			GameObject go = 	NGUITools.AddChild(Transient.gameObject,GibAnim);
			go.transform.position = v;
	}


	public void CreateObjectPickupAnimation (Vector3 v, string s, Color c)
	{
		GameObject go = 	NGUITools.AddChild(Transient.gameObject,ObjectPickupPrefab);
		go.transform.position = v;
		go.GetComponent<UILabel>().color = c;
		
		go.GetComponent<UILabel>().text = s;
		
	}
	public void PutBlood (Vector3 v)
	{
		GameObject go = 	NGUITools.AddChild(Transient.gameObject,Blood);
		go.transform.position = v;
	}

	void ResetGame ()
	{
		Debug.Log ("Reset Game");
		Application.LoadLevel(Application.loadedLevel);
	}
}
