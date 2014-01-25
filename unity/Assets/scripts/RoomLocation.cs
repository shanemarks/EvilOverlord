using UnityEngine;
using System.Collections;

public class RoomLocation : MonoBehaviour 
{

	public LocationType roomObjectType;


	public Player occupiedPlayer = null;


	void Update ()
	{
		GetComponent<UISprite>().color = occupiedPlayer == null ? Color.white : occupiedPlayer.PlayerColor;
	}
}
