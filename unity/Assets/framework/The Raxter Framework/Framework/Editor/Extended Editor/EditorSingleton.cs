using UnityEngine;
using UnityEditor;
using System.Collections;


public abstract class EditorScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
	
	protected static T _instance = null;
	
	public static T instance
	{
		get
		{
			if(_instance != null)
			{
				return _instance;
			}
			
			string editorFolder = "Assets/Editor/Editor Resources";
			string assetPath = editorFolder +"/"+ typeof(T).Name + ".editor.asset";
			Debug.Log("Loading " + typeof(T).Name + " from "+editorFolder+" folder");
			_instance = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
			
			if(_instance == null)
			{
				Debug.LogWarning(typeof(T).Name + " resource does not exist. Creating in "+editorFolder);
				_instance = ScriptableObject.CreateInstance<T>();
				
				System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(editorFolder);
				if(!directory.Exists)
				{
					directory.Create();
				}
				 
				AssetDatabase.CreateAsset(_instance, assetPath);
				AssetDatabase.SaveAssets();
			}
			
			return _instance;
		}
	}
}