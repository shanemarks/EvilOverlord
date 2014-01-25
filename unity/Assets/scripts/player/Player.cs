using UnityEngine;
using System.Collections.Generic;

public class Player: MonoBehaviour {
		
	public bool IsAlive;
	public string PlayerName;

	public Movement _movement;
	public Vector3 StartPos;
	public Transform _trans;

	public List<ItemType> ItemsOwned;

	public Color  PlayerColor;

	public UISprite PlayerSprite;
	public  RoomLocation OnRoomLocation;

	void Start ()
	{
	
		_trans = gameObject.transform;
		_trans.localPosition = StartPos;
	}

	void Update ()
	{
		// TODO it items contain boobytrap, asplode the person
	}

	public void KillPlayer ()
	{
		Debug.Log ("Kill player");
		IsAlive =  false;
		_movement.enabled = false;
	}
	
	void ResetPlayer ()
	{
		_movement.enabled = true;
	}
}

