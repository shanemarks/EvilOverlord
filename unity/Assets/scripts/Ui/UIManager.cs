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
	MASK_ICON ="HoldingMask",
	BOTH_ICON ="HoldingBoth";
	public UIPanel  Transient; // holds aniamtion effects;

	public UISprite[] PlayerIconBorders;
	public UILabel[] ScoreIcons;
	public GameObject ObjectPickupPrefab;
	public GameObject Blood;
	public GameObject GibAnim;
	public UILabel ScreenMessage;

	int bloodDepthCounter = 0;

	public UILabel resetGame;

	void Start ()
	{

	}

	public void PutBlood (Vector3 v)
	{
		GameObject go = NGUITools.AddChild(Transient.gameObject,Blood);
		go.transform.position = v;

		go.GetComponent<UISprite>().depth = bloodDepthCounter;

		bloodDepthCounter += 1;
	}
	void Update ()
	{
		UpdateCharacterIcons();

		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F1))
		{
			AnswerPhone ();
		}
		if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.F2))
		{
			ReplayInstruction();
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F3))
		{
			GameController.instance.showDebugOutput = !GameController.instance.showDebugOutput;
		}
#endif

		resetGame.text = "New game";
	}
	
	public void AnswerPhone ()
	{
		Debug.Log ("Answer Phone");
		if (VoiceSpeaker.GetVoiceState() == 0)
		{
			GameController.instance.NextInstructions();
		}
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
				p.PlayerIcon.spriteName = DEAD_ICON;
				continue;
			}

			bool knife = p.ItemsOwned == PickupType.FakeKnife || p.ItemsOwned == PickupType.RealKnife2 ||p.ItemsOwned == PickupType.RealKnife1;
			bool mask = p.hasGasMask;
			if (knife && mask)
			{
				p.PlayerIcon.spriteName = BOTH_ICON;
				continue;
			}
			if (knife)
			{
				p.PlayerIcon.spriteName = KNIFE_ICON;
				continue;
			}

			if (mask)
			{
				p.PlayerIcon.spriteName = MASK_ICON;
				continue;
		
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
		if (ScoreController.instance.HaveWinners)
		{
			Application.LoadLevel(0);
		}
		else
		{
			Application.LoadLevel(1);
		}
	}
}
