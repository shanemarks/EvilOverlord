using UnityEngine;
using System.Collections;
using Holoville.HOTween;
public class AnimateLegts : MonoBehaviour {



	Player p; 
	
	Tweener _frontTweener, _backTweener;
	void Start ()
	{

		p = GetComponent <Player> ();

		p._movement.OnMoveEvent += StartLegAnimation;
		p._movement.OnStopEvent += StopLegAnimation;

	}

	void StartLegAnimation ()
	{
		p.FrontFootSprite.gameObject.transform.parent.localPosition = new Vector3 (0,20,0);
		p.BackFootSprite.gameObject.transform.parent.localPosition = new Vector3 (0,-15,0);

		_frontTweener = HOTween.To (p.FrontFootSprite.gameObject.transform.parent,0.1f,"localPosition",new Vector3(0,-25,0),true);
		_frontTweener.autoKillOnComplete = false;
		_frontTweener.loops = -1;
		_frontTweener.loopType = LoopType.Yoyo;
		
		
		_backTweener = HOTween.To (p.BackFootSprite.gameObject.transform.parent,0.1f,"localPosition",new Vector3(0,20,0),true);
		_backTweener.autoKillOnComplete = false;
		_backTweener.loops = -1;
		_backTweener.loopType = LoopType.Yoyo;


		_frontTweener.Play ();
		_backTweener.Play ();
	}

		
	void StopLegAnimation ()
	{
		_frontTweener.Kill();
		_backTweener.Kill();
		p.FrontFootSprite.gameObject.transform.parent.localPosition = Vector3.zero;
		p.BackFootSprite.gameObject.transform.parent.localPosition = Vector3.zero;
	}
	

}
