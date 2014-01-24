using UnityEngine;
using System.Collections;

public class Player: MonoBehaviour {
		
	public bool IsAlive;
	public string PlayerName;

	public Movement _movement;
	public Vector3 StartPos;
	public Transform _trans;

	public ItemType[] ItemsOwned;

	public Color  PlayerColor;

	public UISprite PlayerSprite;


	void Start ()
	{
	
		_trans = gameObject.transform;
		_trans.localPosition = StartPos;
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

