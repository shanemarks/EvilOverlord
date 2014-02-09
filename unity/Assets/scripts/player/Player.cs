using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;

public class Player: MonoBehaviour {
		
	public enum ItemHoldingState
		{
		None,
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


	public Color PlayerColor;

	public int Index = -1;

	public UISprite PlayerSprite, PlayerHeadSprite, FrontFootSprite, BackFootSprite;

	public RoomLocation OnRoomLocation;

	public UISprite knifeIcon,maskIcon,PhoneIcon, RtIcon;

	public event System.Action OnDeath;

	bool notifiedAboutGasMask = false;

	public Vector3 MaskPositionCache,KnifePostionCache;

	string KNIFE_ICON ="Knife",
	MASK_ICONSE ="SEMask",
	MASK_ICONSW ="SWMask";


	public bool IsMoving;

	public void DropItem ()
	{
		ItemsOwned = PickupType.None;
	}

	void Start ()
	{

		MaskPositionCache = maskIcon.transform.localPosition;
		KnifePostionCache = knifeIcon.transform.localPosition;
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

		return true;
	}

	public void PassPhone (Player p)
	{
		hasPhone = false;
		p.hasPhone = true;
		p.canPassPhone = false;
		PlayerController.instance.PlayerWithPhone = p;
		UIManager.instance.HidePassButtons ();

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

		if (OnRoomLocation != null)
		{

			if (!RtIcon.gameObject.activeSelf)		// TODO: SWAP TO KNIFE HAND if player is in knife stabbing range
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
			if (OnDeath != null)
			{
				OnDeath ();
			}
		}
	}
	
	void ResetPlayer ()
	{
		hasGasMask = false;
		_movement.enabled = true;
		GetComponent<UIPanel>().alpha = 1;
	}



	

}

