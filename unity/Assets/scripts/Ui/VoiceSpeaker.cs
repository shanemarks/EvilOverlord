﻿// Voice Speaker  (c) ZJP

//

// Windows 32B >> Copy 'Voice_speaker.dll' in windows\system32 folder

// Windows 64B >> Copy 'Voice_speaker.dll' in windows\SysWOW64 folder

// Remember to release "Voice_speaker.dll" with your final project. It will be placed in the same folder as the EXE

//

// Voice Speaker  (c) ZJP //

using UnityEngine;

using System;

using System.Collections.Generic;

using System.Collections;

using System.Runtime.InteropServices;


public class VoiceSpeaker : SingletonBehaviour<VoiceSpeaker>
	
{
	public AudioSource _bgmusic;

	private AudioSource _vcSrc;

	private static bool voiceBusy = false;
	
	[DllImport ("Voice_speaker.dll", EntryPoint="VoiceAvailable")] private static extern int    VoiceAvailable();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="InitVoice")]      private static extern void   InitVoice();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="WaitUntilDone")]  private static extern int    WaitUntilDone(int millisec);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="FreeVoice")]      private static extern void   FreeVoice();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceCount")]  private static extern int    GetVoiceCount();
	
	
	
	// Unity V4.x.x
	
	[DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceName")]   private static extern IntPtr GetVoiceName(int index);
	
	//  other Unity version
	
	// [DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceName")]   private static extern string GetVoiceName(int index);
	
	
	
	[DllImport ("Voice_speaker.dll", EntryPoint="SetVoice")]       private static extern void   SetVoice(int index);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="Say")]            public static extern void   Say(string ttospeak);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="SayAndWait")]     private static extern void   SayAndWait(string ttospeak);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="SpeakToFile")]    private static extern int    SpeakToFile(string filename, string ttospeak);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceState")]  public static extern int    GetVoiceState_();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceVolume")] private static extern int    GetVoiceVolume();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="SetVoiceVolume")] private static extern void   SetVoiceVolume(int volume);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceRate")]   private static extern int    GetVoiceRate();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="SetVoiceRate")]   private static extern void   SetVoiceRate(int rate);
	
	[DllImport ("Voice_speaker.dll", EntryPoint="PauseVoice")]     private static extern void   PauseVoice();
	
	[DllImport ("Voice_speaker.dll", EntryPoint="ResumeVoice")]    private static extern void   ResumeVoice();
	
	
	
	public int voice_nb = 0;

	public int voiceRate = 1;
	
	public float noVoiceVolume = 0.6f;
	public float voiceVolume = 0.2f;
	
	void Start ()
		
	{
		
		if( VoiceAvailable()>0 )
			
		{
			
			InitVoice(); // init the engine
			
			SetVoiceVolume (100);
			
			if (voice_nb > GetVoiceCount()) voice_nb = 0;
			
			if (voice_nb < 0) voice_nb = 0;
			
			
			
			// Unity V4.x.x *******************************************
			
			IntPtr pStr = GetVoiceName(voice_nb);
			
			string str = Marshal.PtrToStringAnsi(pStr);
			
			Debug.Log ("Voice name : "+str); // Voice Name
			
			// Unity V4.x.x *******************************************
			
			
			
			//Debug.Log ("Voice name : "+GetVoiceName(voice_nb)); // Voice Name other Unity version
			
			
			
			Debug.Log ("Number of voice : "+GetVoiceCount()); // Number of voice
			
			
			
			SetVoice(voice_nb); // 0 to voiceCount - 1
			
			Debug.Log ("Voice Rate : "+GetVoiceRate());
			
			SetVoiceRate(voiceRate);
			
			
			
			//Debug.Log ("Voice name : "+GetVoiceName(voice_nb));
			
		
			
			// Say("Tout les systèmes sont opérationnels. Moteurs, en ligne. Armement, en ligne. Nous sommes prêt. 9.,.8.,.7.,.6.,.5.,.4.,.3.,.2.,.1.,.0.,. .Décollage" );

			/*InstructionInfo blah = new InstructionInfo();

			foreach (LocationType cur in System.Enum.GetValues (typeof(LocationType))) {
				SayToFile(blah.GetRoomLocationName(cur));
				SayToFile (blah.GetPrepositionedForLocation(cur));
			}

			foreach (PickupType cur in System.Enum.GetValues (typeof(PickupType)))
				SayToFile(blah.GetItemName (cur));

			SayToFile("There is an item ");
			SayToFile ("The next listener will be given the name of that item.");
			SayToFile("There is a ");
			SayToFile(" in this room. The previous listener was given its location");
			SayToFile ("The ");
			SayToFile (" is not ");
			SayToFile (" The next listener will be given the same information.");
			SayToFile (" The previous listener was given this same information.");
			SayToFile ("In");
			SayToFile (" turns, I will reveal the identity and location of the ");
			SayToFile (" to whoever has the phone.");
			SayToFile (" is ");
			SayToFile (" Beware, someone else knows that you have important information");
			SayToFile ("I have no information to give you at this time, but avoid ending your turn to early");*/

			SayToFile ("Pass the headphones to another player when you are ready");


			
		}
		
	
	}
	
	
	
	void OnDisable()
		
	{
		
		if( VoiceAvailable()>0 )
			
		{
			
			FreeVoice(); 
			
		}
		
	}

	public static int GetVoiceState()
	{
		if (GetVoiceState_() != 0)
			return GetVoiceState_();

		if(voiceBusy)
			return 1;

		return 0;

	}

	void Update ()
	{
		if (GetVoiceState() == 0)
		{
			_bgmusic.volume = noVoiceVolume;
		}
		
		else if (GetVoiceState() == 1)
		{
			_bgmusic.volume = voiceVolume;
		}
		


	}

	public void Talk (List<AudioClip> clipList) 
	{
		StartCoroutine(ManageTalk(clipList));
	}

	IEnumerator ManageTalk(List<AudioClip> clipList)
	{
		Debug.Log("Playing call");
		voiceBusy = true;
		foreach(AudioClip cl in clipList)
		{
			if (cl == null) 
			{
				Debug.LogWarning("Missing an audio clip");
				continue;
			}

			Debug.Log("Playing clip " + cl.name);
			this.audio.clip = cl;
			this.audio.Play();
			while(this.audio.isPlaying) 
				yield return 0;
		}
		voiceBusy = false;
		Debug.Log("Finished call");
	}

	public void Talk (string s)
	{
		s =  s.Replace ("\n","\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
		Say (s);
	}

	public void SayToFile (string s)
	{
		string fileName = (s.Length > 20) ? s.Substring(0,20) : s;
		SpeakToFile ("D:/Audio/" + fileName + ".wav",s);

	}



}