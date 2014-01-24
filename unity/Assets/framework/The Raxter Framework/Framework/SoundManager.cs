using UnityEngine;
using System.Collections;

public class ExampleSoundLibrary : SoundManager.SoundLibrary<ExampleSoundLibrary>
{
	public AudioClip levelMusic;
	
	public AudioClip sound0;
	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;
}

public class SoundManager : SingletonBehaviour<SoundManager> 
{
	public abstract class SoundLibrary<T> : ScriptableObjectSingleton<T> where T : ScriptableObject
	{
		
	}
	
	
	public int soundChannels = 4;
	
	AudioSource [] soundChannel;
//	AudioSource [] musicChannel;
	
	void Start()
	{
		
		soundChannel = new AudioSource[soundChannels];
		for (int i = 0 ; i < soundChannels ; i++)
		{
			GameObject go = new GameObject("Channel "+i);
			soundChannel[i] = go.AddComponent<AudioSource>();
			soundChannel[i].transform.parent = transform;
		}
//		musicChannel = new AudioSource[musicChannels];
//		for (int i = 0 ; i < musicChannels ; i++)
//		{
//			GameObject go = new GameObject("Channel "+i);
//			musicChannel[i] = go.AddComponent<AudioSource>();
//		}
	}
	
	int FindFreeChannel()
	{
		for (int i = 0 ; i < soundChannel.Length ; i++)
		{
			if (!soundChannel[i].isPlaying)
			{
				return i;
			}
		}
		
		return -1;
	}
	
	public int PlayMusic(AudioClip clip)
	{
		int ch = FindFreeChannel();
		soundChannel[ch].loop = true;
		soundChannel[ch].clip = clip;
		soundChannel[ch].volume = 1f;
		soundChannel[ch].Play();
		
		return ch;
	}
	
	public int PlaySound(AudioClip clip, float volume)
	{
		int ch = FindFreeChannel();
		soundChannel[ch].loop = false;
		soundChannel[ch].clip = clip;
		soundChannel[ch].volume = volume;
		soundChannel[ch].Play();
		
		return ch;
	}
	
	public void StopAll(AudioClip clip)
	{
		for (int i = 0 ; i < soundChannel.Length ; i++)
		{
			if (soundChannel[i].clip == clip)
			{
				soundChannel[i].Stop();
			}
		}
	}
	public bool IsPlaying(AudioClip clip)
	{
		for (int i = 0 ; i < soundChannel.Length ; i++)
		{
			if (soundChannel[i].clip == clip && soundChannel[i].isPlaying)
			{
				return true;
			}
		}
		return false;
	}
	
}
public static class AudioClassExtentions
{
	public static void PlayAsMusic(this AudioClip audioClip)
	{
		SoundManager.instance.PlayMusic(audioClip);
	}
	
	public static void Play(this AudioClip audioClip)
	{
		Play(audioClip, 1f);
	}
	
	public static void Play(this AudioClip audioClip, float volume)
	{
		SoundManager.instance.PlaySound(audioClip, volume);
	}
	
	public static void StopAll(this AudioClip audioClip)
	{
		SoundManager.instance.StopAll(audioClip);
	}
	
	public static bool IsPlaying(this AudioClip audioClip)
	{
		return SoundManager.instance.IsPlaying(audioClip);
	}
}