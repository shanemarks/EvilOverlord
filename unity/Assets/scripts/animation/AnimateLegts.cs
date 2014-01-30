using UnityEngine;
using System.Collections;

public class AnimateLegts : MonoBehaviour {



	Player p; 
	Vector3 frontFootCache,BackFootCache;
	void Start ()
	{

		p = GetComponent <Player> ();
		frontFootCache = p.FrontFootSprite.transform.localPosition;
		BackFootCache = p.FrontFootSprite.transform.localPosition;
	}
	void Update () 
	{
	
		if (p._movement.IsMoving)
		{

		}
	
	}
}
