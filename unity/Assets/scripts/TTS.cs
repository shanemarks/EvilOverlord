using UnityEngine;
using System.Collections;

public class TTS : SingletonBehaviour <TTS>{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public IEnumerator GetSound (string s) 
	{
		WWW www =  new WWW("http://translate.google.com/translate_tts?tl=en&q=" + WWW.EscapeURL(s));
				
		yield return www;

		AudioClip audio = www.GetAudioClip(false,false,AudioType.MPEG);

		audio.Play();

		              
	}	
}
