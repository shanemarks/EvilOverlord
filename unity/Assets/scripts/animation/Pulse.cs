using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

	public UISprite _sprite;
	Color c = Color.white;
	
	float counter = 0;
	public float speed = 2 ;

	public float minAlpha = 0;

	void Update ()
	{
		if (_sprite == null)
			return;
		counter += Time.deltaTime*speed;
		_sprite.alpha = minAlpha + (1-minAlpha)*Mathf.Abs (Mathf.Sin (counter) );

	}

	void OnDisable()
	{
		if (_sprite == null)
			return;
		_sprite.alpha = 1;
	}


}
