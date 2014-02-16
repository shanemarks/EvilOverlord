using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

	public UISprite _sprite;
	Color c = Color.white;
	
	float counter = 0;
	public float speed = 2 ;

	void Update ()
	{
		counter += Time.deltaTime*speed;
		_sprite.alpha = Mathf.Abs (Mathf.Sin (counter) );

	}

	void OnDisable()
	{
		_sprite.alpha = 1;
	}


}
