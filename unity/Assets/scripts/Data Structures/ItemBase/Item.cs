using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public void PickUp ()
	{
		Debug.Log ("Processing item");
		Destroy (gameObject);
	}
}
