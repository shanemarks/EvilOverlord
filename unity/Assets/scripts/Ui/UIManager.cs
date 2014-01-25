using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;

	public Camera UICamera;

	public UILabel TextInfo;

	public UISprite [] PlayerIcons;

	public string ALIVE_ICON = "Alive",
				  DEAD_ICON = "Dead",
				  CRATE_ICON = "Holding";
	public UIPanel  Transient; // holds aniamtion effects;


	public GameObject ObjectPickupPrefab;

	void Start ()
	{

	}

	void AnswerPhone ()
	{
		Debug.Log ("Answer Phone");
		GameController.instance.PlayInstructions();
	}

	void  StartGame ()
	{
		Debug.Log ("Start Game");

	}

	public void UpdateCharacterIcons ()
	{
		foreach (Player p in PlayerController.instance.Players)
		{

			if (p.IsAlive)
			{
				p.PlayerIcon.spriteName= ALIVE_ICON;
			}
			
			else
			{
				p.PlayerIcon.spriteName = DEAD_ICON;
				return;
			}

			if (p.IsHoldingItem)
			{
				p.PlayerIcon.spriteName = CRATE_ICON;
		
			}
	
			p.PlayerIcon.Update ();

		}

	}

	public void CreateObjectPickupAnimation (Vector3 v, string s)
	{
		GameObject go = 		NGUITools.AddChild(Transient.gameObject,ObjectPickupPrefab);
		go.transform.position = v;
		go.GetComponent<UILabel>().text = s;
	}

	void Update ()
	{
		UpdateCharacterIcons();
	}
}
