using UnityEngine;
using System.Collections;

public class PlayerIcon : LinkBase {
	void Awake ()
	{
		CalculateLinkPosition ();
	}

	public UISprite Border;
	public UILabel  Points;
	public UISprite Icon;
	public UISprite ButtonIcon;

	public Player ThePlayer;

	void Update ()
	{
		if (ButtonIcon.gameObject.activeSelf)
		{
			if (!ThePlayer.IsAlive)
			{
				ButtonIcon.gameObject.SetActive(false);
			}
		}
	}
}
