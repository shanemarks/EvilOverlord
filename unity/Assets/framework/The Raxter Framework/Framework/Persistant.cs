using UnityEngine;
using System.Collections;

public class Persistant : MonoBehaviour 
{

	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		gameObject.name += "(Persistant)";
	}
}
