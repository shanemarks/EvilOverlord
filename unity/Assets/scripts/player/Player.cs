﻿using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;

public class Player: MonoBehaviour {
		
	public bool IsAlive;
	public string PlayerName;

	public Movement _movement;
	public Vector3 StartPos;
	public Transform _trans;

	public PickupType ItemsOwned;
	public bool hasGasMask = false;


	public Color PlayerColor;

	public int Index = -1;

	public UISprite PlayerSprite, PlayerIcon, PlayerHeadSprite, FrontFootSprite, BackFootSprite;
	public RoomLocation OnRoomLocation;

	public UISprite knifeIcon,maskIcon;

	bool notifiedAboutGasMask = false;

	public Vector3 MaskPositionCache,KnifePostionCache;

	string KNIFE_ICON ="Knife",
	MASK_ICONSE ="SEMask",
	MASK_ICONSW ="SWMask";

	public void DropItem ()
	{
		ItemsOwned = PickupType.None;
	}

	void Start ()
	{

		MaskPositionCache = maskIcon.transform.localPosition;
		KnifePostionCache = knifeIcon.transform.localPosition;
	
		_trans = gameObject.transform;
		_trans.localPosition = StartPos;
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

		if (knife)
		{
			if (PlayerSprite.spriteName == "NW&SEBody")
			{
				knifeIcon.spriteName = KNIFE_ICON;
			}
			else
			{
				knifeIcon.spriteName = "";
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
		knifeIcon.Update();
		maskIcon.Update();


	}

	public void KillPlayer ()
	{
		if (IsAlive)
		{
			Debug.Log ("Kill player");
			IsAlive =  false;
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
	}



	

}

