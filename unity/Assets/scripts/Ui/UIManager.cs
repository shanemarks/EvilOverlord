using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;

	public Camera UICamera;

	public UILabel TextInfo;

	public UISprite [] PlayerIcons;

	public string ALIVE_ICON = "Alive",
				  DEAD_ICON = "Dead",
					KNIFE_ICON ="HoldingKnife",
	MASK_ICON ="HoldingMask";
	public UIPanel  Transient; // holds aniamtion effects;

	public UISprite[] PlayerIconBorders;

	public GameObject ObjectPickupPrefab;
	public GameObject Blood;
	public GameObject GibAnim;
	void Start ()
	{

	}

	public void PutBlood (Vector3 v)
	{
		GameObject go = NGUITools.AddChild(Transient.gameObject,Blood);
		go.transform.position = v;
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
		if (VoiceSpeaker.GetVoiceState() ==0)	GameController.instance.NextInstructions();
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
				p.PlayerIcon.spriteName = ALIVE_ICON;
			}
			 if (!p.IsAlive)
			{
				p.PlayerIcon.spriteName= DEAD_ICON;
			}
			
			 if (p.ItemsOwned == PickupType.FakeKnife || p.ItemsOwned == PickupType.RealKnife2 ||p.ItemsOwned == PickupType.RealKnife1)
			{
				p.PlayerIcon.spriteName = KNIFE_ICON;
				return;
			}

		 if (p.ItemsOwned == PickupType.GasMask1 || p.ItemsOwned == PickupType.GasMask2)
			{
				p.PlayerIcon.spriteName = MASK_ICON;
		
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
	
 void CreateObjectPickupAnimation (Vector3 v, string s, Color c)
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
