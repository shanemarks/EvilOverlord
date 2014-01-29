using UnityEngine;
using System.Collections;
using Holoville.HOTween;
public class AnswerCallButton : MonoBehaviour {

	[SerializeField] UISprite  _label;
	[SerializeField] UIButtonMessage _message;
	[SerializeField] UIImageButton _button;
	Tweener colortween;

	void Start () {
		colortween = HOTween.To (_label, 0.7f, "color", new Color (1,1,0.7f,1));
		colortween.loops = -1;
		colortween.loopType = LoopType.Yoyo;
	}
	
	// Update is called once per frame
	void Update () {
		if (VoiceSpeaker.GetVoiceState () == 0) 
		{
			_message.enabled = true;
			_button.hoverSprite = "ButtonActive";
			_label.color =  Color.white;
			colortween.enabled = true;
		}

		else
		{
			_message.enabled = false;
			_button.hoverSprite = "Button";
			colortween.enabled = false;
			_label.color =  Color.gray;
	

		}
	}
}
