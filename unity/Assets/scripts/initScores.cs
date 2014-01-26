using UnityEngine;
using System.Collections;

public class initScores : MonoBehaviour {

void Start ()
	{
		InitScores();
	}


	public static void InitScores()
	{
		PlayerPrefs.SetInt ("Player1",0);
		PlayerPrefs.SetInt ("Player2",0);
		PlayerPrefs.SetInt ("Player3",0);
		PlayerPrefs.SetInt ("Player4",0);
		PlayerPrefs.Save();
	}
}
