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
	
	public UILabel handcuffLabel;
	public UISprite handcuffSprite;

	public UILabel passPhoneLabel;
	void Start ()
	{

	}

	public void SetHandcuffTime(int turnsLeft)
	{
		if (turnsLeft > 0)
		{
			handcuffLabel.text = "Cannot interact while handcuffs are on.\n"+turnsLeft+" turns until hands are free.";
			handcuffSprite.enabled = true;
		}
		else
		{
			handcuffLabel.text = "Handcuffs are off, you can interact with objects in the room!\n(Right Trigger button)";
			handcuffSprite.enabled = false;
		}
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
		PassPhone ();
		UpdateCharacterIcons();


#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F3))
		{
			GameController.instance.showDebugOutput = !GameController.instance.showDebugOutput;
		}
#endif

//		if (ScoreController.instance.HaveOverallWinners)
//		{
//			resetGame.text = "New game";
//		}

//		if(PlayerController.instance.RoundWon)
//		{
//
//			ResetGame ();
//		}
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
		PlayerController.instance.TallyPlayerInformation();
		bool showPassPhoneUI = PlayerController.instance.PlayerWithPhone.canPassPhone;
		bool haveTarget = false;
		
		passPhoneLabel.enabled = showPassPhoneUI;
		if (showPassPhoneUI)
		{

			foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
			{
				if (icon.ThePlayer.CanRecievePhone())
				{
					icon.ButtonIcon.gameObject.SetActive (true);
					haveTarget  = true;
				}

			}
		/*	if (!haveTarget)
			{
				foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
				{
					if (icon.ThePlayer == PlayerController.instance.PlayerWithLeastInformation)
					{
						icon.ButtonIcon.gameObject.SetActive (true);
					}
					    
				}

			}
			 */
		}

		else
		{
			
			foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
			{
				icon.ButtonIcon.gameObject.SetActive (false);
			}
		}

	}


// public void HidePassButtons ()
//	{
//		foreach (PlayerIcon icon in PlayerIcon.First(typeof (PlayerIcon)))
//		{
//
//				icon.ButtonIcon.gameObject.SetActive (false);
//		}
//	}


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
			playerIcon.PhoneCount.text = playerIcon.ThePlayer.PhoneCount.ToString();
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


	public void ResetGame ()
	{
		while (VoiceSpeaker.GetVoiceState() != 1)
		{
		}
		Debug.Log ("Reset Game");
		if (ScoreController.instance.HaveOverallWinners)
		{
			Application.LoadLevel(0);
		}
		else
		{
			Application.LoadLevel(1);
		}
		
	}
}
