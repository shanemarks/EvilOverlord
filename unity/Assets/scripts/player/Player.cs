using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;

public class Player: MonoBehaviour {
		
	public enum ItemHoldingState
		{
		None,
		Handcuffs,
		Knife,
		Mask,
		Both
		}

	public ItemHoldingState HoldingState;
	public bool IsAlive;
	public string PlayerName;

	public Movement _movement;

	public Vector3 StartPos;
	public Transform _trans;
	
	public bool hasPhone = false;
	public bool canPassPhone = false;

	public PickupType ItemsOwned;
	public bool hasGasMask = false;

	public bool handcuffsOn = false;

	public bool enableLeave;

	public Color PlayerColor;

	public int Index = -1;

	public UISprite PlayerSprite, PlayerHeadSprite, FrontFootSprite, BackFootSprite;

	public RoomLocation OnRoomLocation;

	public UISprite knifeIcon,maskIcon,PhoneIcon, RtIcon, handcuffSprite;

	bool notifiedAboutGasMask = false;

	public Vector3 MaskPositionCache,KnifePostionCache,HandPositionCache;

	string KNIFE_ICON ="Knife",
	MASK_ICONSE ="SEMask",
	MASK_ICONSW ="SWMask";


	public bool IsMoving;

	private int _phoneCount = 0;

	public int PhoneCount 
	{
		get 
		{
			return _phoneCount;
		}
	}

	public void DropItem ()
	{
		ItemsOwned = PickupType.None;
	}

	void Start ()
	{

		MaskPositionCache = maskIcon.transform.localPosition;
		KnifePostionCache = knifeIcon.transform.localPosition;
		HandPositionCache = handcuffSprite.transform.localPosition;
		PhoneIcon.color =  PlayerSprite.color;
		RtIcon.color = PlayerSprite.color;
		_trans = gameObject.transform;
		_trans.localPosition = StartPos;
	}

	public bool CanRecievePhone()
	{

		if (!IsAlive)
			return false;

		if (hasPhone)
			return false;

		if (Mathf.Abs (PlayerController.instance.PlayerWithLeastInformation.PhoneCount - PhoneCount) >= 3 )
			return false;

		return true;
	}

	public void PassPhone (Player p)
	{
		hasPhone = false;
		p.hasPhone = true;
		p.canPassPhone = false;
		PlayerController.instance.PlayerWithPhone = p;
		p._phoneCount++;
		GameController.instance.lastPlayerToPass = Index;
		GameController.instance.NextInstructions();

	}



	void Update ()
	{
	

		if (GameController.instance.GasTrapTriggered)
		{
			if (!hasGasMask /* && ItemsOwned != PickupType.GasMask1 && ItemsOwned != PickupType.GasMask2*/)
			{
				KillPlayer();
			}
			else
			{
				if (!notifiedAboutGasMask)
				{
					UIManager.instance.CreateObjectPickupAnimation (transform.position, "Gas mask used");
					notifiedAboutGasMask = true;
				}
			}
		}

		bool knife = ItemsOwned == PickupType.FakeKnife || ItemsOwned == PickupType.RealKnife2 || ItemsOwned == PickupType.RealKnife1;
		bool mask = hasGasMask;

		
		if (handcuffsOn)
		{
			HoldingState = ItemHoldingState.Handcuffs;
			if (PlayerHeadSprite.spriteName == "SWHead")
			{
				handcuffSprite.transform.localPosition =  HandPositionCache - new Vector3 (75,0,0);

				handcuffSprite.spriteName = "SWHandcuff";
				handcuffSprite.width  = 83;
				handcuffSprite.height = 53;

				
			}

			else if (PlayerHeadSprite.spriteName == "SEHead")
			{
				
				handcuffSprite.transform.localPosition = HandPositionCache;
				handcuffSprite.spriteName = "SEHandcuff";
				handcuffSprite.width  = 83;
				handcuffSprite.height = 53;
				
			}

			else if (PlayerHeadSprite.spriteName == "NWHead")
			{
				
				handcuffSprite.transform.localPosition =  HandPositionCache - new Vector3 (128,-10,0);
				handcuffSprite.spriteName = "NWHandcuff";
				handcuffSprite.width  = 28;
				handcuffSprite.height = 28;
				
			}

			
			else if (PlayerHeadSprite.spriteName == "NEHead")
			{
				
				handcuffSprite.transform.localPosition =  HandPositionCache - new Vector3 (-50,-10,0);
				handcuffSprite.spriteName = "NEHandcuff";
				handcuffSprite.width = 28;
				handcuffSprite.height = 28;
				
			}
			
		}
		else
		{
			if (mask && knife)
			{
				HoldingState = ItemHoldingState.Both;
			}
			
			else if (!mask && !knife)
			{
				HoldingState = ItemHoldingState.None;
			}

			else if (mask)
			{
				HoldingState = ItemHoldingState.Mask;
			}

			else if (knife)
			{
				HoldingState = ItemHoldingState.Knife;
			}
		}

		if (knife)
		{
			knifeIcon.spriteName = KNIFE_ICON;

			if (PlayerSprite.spriteName == "NW&SEBody")
			{
				knifeIcon.transform.localPosition = KnifePostionCache;
			}
			else
			{
				knifeIcon.transform.localPosition = KnifePostionCache + new Vector3 (100,0,0);
		
			}
		
		}
		
		else
		{
			knifeIcon.spriteName = "";
		
		}
		if (mask)
		{
		
			if (PlayerHeadSprite.spriteName == "SWHead")
			{
				maskIcon.spriteName = MASK_ICONSW;
				maskIcon.transform.localPosition = MaskPositionCache + new Vector3(-15,0,0);

			}
			else if (PlayerHeadSprite.spriteName == "SEHead")
			{

				maskIcon.spriteName = MASK_ICONSE;
				maskIcon.transform.localPosition = MaskPositionCache;

			}

			else maskIcon.spriteName ="";
		}

		else
		{

			maskIcon.spriteName = "";
		}
		
		handcuffSprite.enabled = HoldingState == ItemHoldingState.Handcuffs;

		if (hasPhone)
		{
			if (!PhoneIcon.gameObject.activeSelf)
			{
				PhoneIcon.gameObject.SetActive(true);
			
				PhoneIcon.color = PlayerSprite.color;

			}

		}

		else
		{
			if (PhoneIcon.gameObject.activeSelf)
			{
				PhoneIcon.gameObject.SetActive(false);
			}
		}
	
		knifeIcon.Update();
		maskIcon.Update();
		bool canStab = (GameController.instance.FindStabablePlayer(this) != null && (HoldingState == ItemHoldingState.Both || HoldingState == ItemHoldingState.Knife));
		if (OnRoomLocation != null || canStab)
		{

			if (canStab)
			{
				RtIcon.spriteName = "rt_knife";
				RtIcon.gameObject.GetComponent<Pulse>().enabled = true;
			}
			else
			{
				RtIcon.spriteName = "rt_hand";
				RtIcon.gameObject.GetComponent<Pulse>().enabled = false;
			}
			


			if (!RtIcon.gameObject.activeSelf)	
			{ 

		
				RtIcon.gameObject.SetActive(true);
			}

		}

		else
		{
			if (RtIcon.gameObject.activeSelf)
			{

				RtIcon.gameObject.SetActive(false);
			}
					
		}




	}

	public void KillPlayer ()
	{
		if (IsAlive)
		{
			Debug.Log ("Kill player");
			IsAlive =  false;

			DropItem();
			_movement.enabled = false;
			notifiedAboutGasMask = false;
			if (OnRoomLocation != null)
			{
//				OnRoomLocation.gameObject.GetComponent<UISprite>().color = Color.white;

				OnRoomLocation.occupiedPlayer = null;
				OnRoomLocation = null;

			}
			UIManager.instance.Gib(transform.position);
			UIManager.instance.PutBlood(transform.position);
			HOTween.To(GetComponent<UIPanel>(), 1f, "alpha", 0f);

			ScoreController.instance.timer = 0;
		}
	}
	
	void ResetPlayer ()
	{
		hasGasMask = false;
		_movement.enabled = true;
		GetComponent<UIPanel>().alpha = 1;
		enableLeave = false;
	}



	

}

