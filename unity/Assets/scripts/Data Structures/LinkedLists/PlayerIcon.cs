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
}
