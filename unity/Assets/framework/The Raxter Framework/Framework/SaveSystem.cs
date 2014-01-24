using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SaveSystem
{
	
	
	public static void WriteSaveData(string fileName, string data)
	{
		if (fileName == playerPrefsFilesKey)
		{
			Debug.LogError ("Attempting to write to the player prefs file list");
			return;
		}
		if (fileName.Contains(""+playerPrefsDeliminator))
		{
			Debug.LogError ("Attempting to write a file with delimeter in the name");
			return;
		}
		PlayerPrefs.SetString(fileName, data);
		
		AddToPlayerPrefFileList(fileName);
	}
	
	public static string ReadSaveData(string fileName)
	{
		return PlayerPrefs.GetString(fileName);
	}
	
	public static void DeleteSaveData(string fileName)
	{
		PlayerPrefs.DeleteKey(fileName);
		RemoveFromPlayerPrefFileList(fileName);
	}
	
	
	public static string playerPrefsFilesKey = "__Files";
	public static char playerPrefsDeliminator = ',';
	
	public static string [] GetPlayerPrefFiles()
	{
		if (PlayerPrefs.HasKey(playerPrefsFilesKey))
		{
			return PlayerPrefs.GetString(playerPrefsFilesKey).Split(playerPrefsDeliminator);
		}
		else
		{
			PlayerPrefs.SetString(playerPrefsFilesKey, "");
			return new string [] {};
		}
	}
	
	static void AddToPlayerPrefFileList(string file)
	{
		List<string> fileList = new List<string>(GetPlayerPrefFiles());
		if (!fileList.Contains(file))
		{
			fileList.Add(file);
		}
		PlayerPrefs.SetString(playerPrefsFilesKey, string.Join(""+playerPrefsDeliminator, fileList.ToArray()));	
	}
	
	static void RemoveFromPlayerPrefFileList(string file)
	{
		List<string> fileList = new List<string>(GetPlayerPrefFiles());
		if (fileList.Contains(file))
		{
			fileList.Remove(file);
		}
		PlayerPrefs.SetString(playerPrefsFilesKey, string.Join(""+playerPrefsDeliminator, fileList.ToArray()));	
	}

	public static bool ContainsFile (string file)
	{
		return PlayerPrefs.HasKey(file);
	}
	
	
#if UNITY_EDITOR
	
	public static void DeleteAll()
	{
		foreach (string key in GetPlayerPrefFiles())
		{
			PlayerPrefs.DeleteKey(key);
		}
		PlayerPrefs.SetString(playerPrefsFilesKey, "");
	
	}
	
#endif
	
}
