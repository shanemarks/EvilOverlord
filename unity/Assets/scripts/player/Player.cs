using UnityEngine;
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



	bool notifiedAboutGasMask = false;

	public void DropItem ()
	{
		ItemsOwned = PickupType.None;
	}

	void Start ()
	{
	
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


		}
	}
	
	void ResetPlayer ()
	{
		hasGasMask = false;
		_movement.enabled = true;
		GetComponent<UIPanel>().alpha = 1;
	}



	

}

