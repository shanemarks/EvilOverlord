using UnityEngine;
using System.Collections;

public class initScores : MonoBehaviour {

void Start ()
	{
		PlayerPrefs.SetInt ("Red",0);
		PlayerPrefs.SetInt ("Green",0);
		PlayerPrefs.SetInt ("Blue",0);
		PlayerPrefs.SetInt ("Orange",0);
		PlayerPrefs.Save();
	}
}
