using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;

	public Camera UICamera;



	public string ALIVE_ICON = "Alive",
	DEAD_ICON = "Dead",
	KNIFE_ICON ="HoldingKnife",
	MASK_ICON ="HoldingMask",
	BOTH_ICON ="HoldingBoth";
	public UIPanel  Transient; // holds aniamtion effects;



	public GameObject ObjectPickupPrefab;
	public GameObject Blood;
	public GameObject GibAnim;
	public UILabel ScreenMessage;

	int bloodDepthCounter = 0;

	public UILabel resetGame;
	public UIPanel PassPhonePanel;
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
			PassPhone ();
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

		if (ScoreController.instance.HaveWinners)
		{
			resetGame.text = "New game";
		}
	}
	
	public void AnswerPhone ()
	{
		Debug.Log ("Answer Phone");
		if (VoiceSpeaker.GetVoiceState() == 0)
		{
			GameController.instance.NextInstructions();
		}
	}


	public void PassPhone ()
	{
		if (VoiceSpeaker.GetVoiceState () == 0)
		{
			foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
			{
				if ((!icon.ThePlayer.hasPhone) && (icon.ThePlayer.IsAlive))
				{
					icon.ButtonIcon.gameObject.SetActive (true);
				}
			}
		}
	}


	public void HidePassButtons ()
	{
		foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
		{

				icon.ButtonIcon.gameObject.SetActive (false);
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
		PlayerIcon playerIcon = (PlayerIcon) PlayerIcon.First(typeof(PlayerIcon));

		foreach (Player p in PlayerController.instance.Players)
		{
			if (p.IsAlive)
			{
			switch (p.HoldingState)
			{
				case Player.ItemHoldingState.Both:
					playerIcon.Icon.spriteName  = BOTH_ICON;
					break;
				case Player.ItemHoldingState.Knife:
					playerIcon.Icon.spriteName  = KNIFE_ICON;
					break;

				case Player.ItemHoldingState.Mask:
					playerIcon.Icon.spriteName  = MASK_ICON;
					break;

				case Player.ItemHoldingState.None:
					playerIcon.Icon.spriteName = ALIVE_ICON;
					break;
				}
			}
			else 
			{
				playerIcon.Icon.spriteName = DEAD_ICON;

			}
					

			playerIcon = (PlayerIcon) playerIcon.Next;

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

			GameObject go = NGUITools.AddChild(Transient.gameObject,GibAnim);
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
