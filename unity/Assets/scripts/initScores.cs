using UnityEngine;
using System.Collections;

public class initScores : MonoBehaviour {

void Start ()
	{
		PlayerPrefs.SetInt ("Player1",0);
		PlayerPrefs.SetInt ("Player2",0);
		PlayerPrefs.SetInt ("Player3",0);
		PlayerPrefs.SetInt ("Player4",0);
		PlayerPrefs.Save();
	}
}
