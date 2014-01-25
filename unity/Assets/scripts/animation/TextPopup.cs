using UnityEngine;
using System.Collections;
using Holoville.HOTween;
public class TextPopup : MonoBehaviour 
{
	Transform _trans;
	TweenParms  parms,parmsC;
	public float time = 0.5f;
	void  Start ()
	{
		parms = new TweenParms().Prop("localPosition",new Vector3 (0,200,0),true).Prop ("localScale", new Vector3 (2,2,2)).OnComplete(Kill).Ease(EaseType.EaseOutSine);

		_trans = gameObject.transform;
		HOTween.To (_trans, 2f, parms);
		HOTween.To (GetComponent<UILabel>(),  2,"color", new Color (1,1,1,0));

	}

	void Kill ()
	{
		Destroy(gameObject);
	}


}
