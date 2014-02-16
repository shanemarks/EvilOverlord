using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

	public UISprite _sprite;
	Color c = Color.white;
	
	float counter = 0;
	public float speed = 2 ;

	void Update ()
	{
		c = new Color (1,1,1, Mathf.Abs (Mathf.Sin (counter) ) );
		counter += Time.deltaTime*speed;
		_sprite.color = c;

	}


}
