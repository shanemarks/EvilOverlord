using UnityEngine;
using System.Collections;
using Holoville.HOTween;
public class GibAnimation : MonoBehaviour {

	[SerializeField] UISprite[] Gibs;
	float rot =10;
	Transform pivot;
	void Start () {
	
			 foreach (UISprite s in Gibs)
				{
			HOTween.To (s.gameObject.transform, 1.3f, new TweenParms().Prop("localPosition",new Vector3 (Random.Range(-360,360),Random.Range(-360,360),0),true).OnComplete(Kill).Ease(EaseType.EaseOutSine));
			HOTween.To (s,  1.3f,"color", new Color (1,1,1,0));

				}

	}

	void Kill ()
	{
		Destroy(gameObject);
	}
}