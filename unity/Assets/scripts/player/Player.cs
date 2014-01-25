using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;

public class Player: MonoBehaviour {
		
	public bool IsAlive;
	public string PlayerName;

	public Movement _movement;
	public Vector3 StartPos;
	public Transform _trans;

	public ItemType ItemsOwned;

	public Color  PlayerColor;

	public UISprite PlayerSprite, PlayerIcon, PlayerHeadSprite, FrontFootSprite, BackFootSprite;
	public RoomLocation OnRoomLocation;


	bool notifiedAboutGasMask = false;

	public void DropItem ()
	{
		ItemsOwned = ItemType.None;
	}

	void Start ()
	{
	
		_trans = gameObject.transform;
		_trans.localPosition = StartPos;
	}

	void Update ()
	{

		if (ItemsOwned == ItemType.BoobyTrap)
		{
			KillPlayer ();
		}


		if (GameController.instance.PoisonVentOpen)
		{
			if (ItemsOwned != ItemType.GasMask)
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

	}

	public void KillPlayer ()
	{
		if (IsAlive)
		{
			Debug.Log ("Kill player");
			IsAlive =  false;
			_movement.enabled = false;
			notifiedAboutGasMask = false;
			
			HOTween.To(GetComponent<UIPanel>(), 1f, "alpha", 0f);
		}
	}
	
	void ResetPlayer ()
	{
		_movement.enabled = true;
		GetComponent<UIPanel>().alpha = 1;
	}

	

}

