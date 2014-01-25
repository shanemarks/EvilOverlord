using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using RaxterWorks.GamepadInputManager;

public class GamepadSetup : EditorWindow
{
	
	[MenuItem ("Window/Gamepad Setup")]
	static void Init () 
	{
		EditorWindow.GetWindow (typeof (GamepadSetup));
	}
	
	string serialisedGamepadName = "";
	string serialisedGamepad = "";
	
	string GetInputManagerSerializedAxis(int player, int axis)
	{
		return  "  - m_Name: "+GamepadInput.GetInputAxisName(player, axis+1)+"\n" +
				"    descriptiveName: \n"+
				"    descriptiveNegativeName: \n"+
				"    negativeButton: \n"+
				"    positiveButton: \n"+
				"    altNegativeButton: \n"+
				"    altPositiveButton: \n"+
				"    gravity: 0\n"+
				"    dead: 0\n"+
    			"    sensitivity: 1\n"+
		    	"    type: 2\n" +
		    	"    axis: "+axis+"\n" +
		    	"    joyNum: "+(player+1)+"\n";
	}
	
	
	const string inputFilePath = "ProjectSettings/InputManager.asset";
	const string inputFileBackupPath = "ProjectSettings/InputManager.asset.gpcbackup";
	
	bool IsInputManagerInBinary()
	{
		byte [] inputManagerBytes = System.IO.File.ReadAllBytes(inputFilePath);
		return inputManagerBytes[0] != '%';
	}
	

	bool showSupported = false;
	bool showSupportedFull = false;
	
	int numberOfJoysticksToGen = 4;
	
	void BackUpInputFile()
	{
		FileUtil.ReplaceFile(inputFilePath, inputFileBackupPath);
	}
	
	void RestoreFromBackup()
	{
		FileUtil.ReplaceFile(inputFileBackupPath, inputFilePath);
	}
	
	const string inputManagerYAMLHeader = 	"%YAML 1.1\n"+
											"%TAG !u! tag:unity3d.com,2011:\n"+
											"--- !u!13 &1\n"+
											"InputManager:\n"+
											"  m_ObjectHideFlags: 0\n"+
											"  m_Axes:\n";
	
	string GetAppendableAxes(int playersToGenerate)
	{
		string toAppend = "\n";
		for (int p = 0 ; p < playersToGenerate ; p++)
		{
			for (int a = 0 ; a < GamepadInput.MaxAxes ; a++)
			{
				toAppend += GetInputManagerSerializedAxis(p, a);
			}
		}
		
		return toAppend;
	}
	
	static void GUILine()
	{
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
	}
	
	void OnGUI()
	{
		bool inputManagerIsBinary = IsInputManagerInBinary();
		GUILayoutOption buttonWidth = GUILayout.MaxWidth(300);
		GUILayoutOption buttonHalfWidth = GUILayout.MaxWidth(150);
		
		
		EditorGUILayout.LabelField("Gamepad/Controller setup");
		
		GUILine();
			
		EditorGUILayout.LabelField("Input Settings/Axes Functions:");
		
		EditorGUILayout.LabelField("Go to \"Edit -> Project Settings -> Input\" to see input axes");
		
		EditorGUILayout.Separator();
		
		if (inputManagerIsBinary)
		{
			EditorGUILayout.BeginHorizontal();
			GUI.enabled = false;
		}
		if (GUILayout.Button(inputManagerIsBinary ? "Cannot append input axes in binary mode" : "Append game controller input axes", buttonWidth))
		{
			string toAppend = GetAppendableAxes(numberOfJoysticksToGen);
			System.IO.File.AppendAllText(inputFilePath, toAppend);
			AssetDatabase.Refresh();
		}
		if (inputManagerIsBinary)
		{
			GUI.enabled = true;
			bool displayHelp = GUILayout.RepeatButton("?", GUILayout.MaxWidth(30));
			
			EditorGUILayout.EndHorizontal();
			if (displayHelp)
			{
				EditorUtility.DisplayDialog("Cannot edit input axes in Binary Mode", 
					"In order to make the input axes appendable, go to:\n\nEdit -> Project Settings -> Editor\n\nand set \"Asset Serialization\" to \"Force Text\"\n\n(Unity Pro only)", "OK");

			}
			
		}
		
		EditorGUILayout.BeginHorizontal();
		{
			bool hasBackup = System.IO.File.Exists(inputFileBackupPath);
			GUILayoutOption backupGUIOption = hasBackup ? buttonHalfWidth : buttonWidth;
			if (GUILayout.Button(hasBackup? "Overwrite Backup" : "Create Backup", backupGUIOption))
			{
				bool overwrite = true;
				if (System.IO.File.Exists(inputFileBackupPath))
				{
					overwrite = EditorUtility.DisplayDialog("Are you sure you want to backup?", "There is already a backup, clicking OK will overwrite this backup.", "OK!", "Back");
				}
				if (overwrite)
				{
					BackUpInputFile();
					AssetDatabase.Refresh();
				}
			}
			if (hasBackup && GUILayout.Button("Restore From Backup", backupGUIOption))
			{
				bool overwrite = true;
				if (System.IO.File.Exists(inputFileBackupPath))
				{
					overwrite = EditorUtility.DisplayDialog("Are you sure you want to restore?", "This will overwrite your current input settings", "Do it", "Wait, go back");
				}
				if (overwrite)
				{
					RestoreFromBackup();
					AssetDatabase.Refresh();
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if (GUILayout.Button("Clear all Input Axes (even mouse and keyboard)", buttonWidth))
		{
			if (EditorUtility.DisplayDialog("Are you sure you want to clear?", "This will delete *all* inputs. ", "Rid me of them!", "No, keep them"))
			{
				string toWrite = inputManagerYAMLHeader;
				System.IO.File.WriteAllText(inputFilePath, toWrite);
				AssetDatabase.Refresh();
			}
		}
		if (GUILayout.Button("Overwrite all Input Axes", buttonWidth))
		{
			if (EditorUtility.DisplayDialog("Are you sure you want to overwrite?", "This will delete *all* inputs (including all existing mouse and keyboard axes) and replace them with joystick axes.", "Let me have them!", "No, don't do it!"))
			{
				string toWrite = inputManagerYAMLHeader + GetAppendableAxes(numberOfJoysticksToGen);
				System.IO.File.WriteAllText(inputFilePath, toWrite);
				AssetDatabase.Refresh();
			}
		}
		
		numberOfJoysticksToGen = EditorGUILayout.IntSlider("# of joysticks to setup", numberOfJoysticksToGen, 1, 4, buttonWidth);
		GUILine();
		
		if (showSupported)
		{
			EditorGUILayout.BeginHorizontal();
		}
		showSupported = EditorGUILayout.Foldout(showSupported, "Supported Controllers ("+GamepadInputConfigurations.GetAllControllers().Length+")");
		
		if (showSupported)
		{
			EditorGUILayout.LabelField("Show detailed list", GUILayout.MaxWidth(120));
			showSupportedFull = EditorGUILayout.Toggle(showSupportedFull, GUILayout.MaxWidth(30));
			EditorGUILayout.EndHorizontal();
		}

		System.Action<GamepadAxes> displayAxesNames =
		(axes) => 
		{
			EditorGUI.indentLevel ++;
			foreach (string name in axes.names)
			{
				EditorGUILayout.LabelField(name);
			}
			EditorGUI.indentLevel --;
			EditorGUILayout.LabelField("---");
		};
		if (showSupported)
		{
			HashSet<GamepadAxes> defaultControllers = new HashSet<GamepadAxes>(GamepadInputConfigurations.GetDefaultGamepads());
			HashSet<GamepadAxes> customControllers = new HashSet<GamepadAxes>(GamepadInputConfigurations.GetCustomControllers());
			HashSet<GamepadAxes> playerPrefControllers = new HashSet<GamepadAxes>(GamepadInputConfigurations.GetPlayerPrefControllers());
			
			if (showSupportedFull)
			{
				EditorGUILayout.LabelField("PlayerPref Controllers:");
				foreach (GamepadAxes axes in playerPrefControllers) displayAxesNames(axes);
				EditorGUILayout.LabelField("Custom Controllers:");
				foreach (GamepadAxes axes in customControllers) displayAxesNames(axes);
				EditorGUILayout.LabelField("Default Controllers:");
				foreach (GamepadAxes axes in defaultControllers) displayAxesNames(axes);
			}
			else
			{
				List<GamepadAxes> fullList = new List<GamepadAxes>(GamepadInputConfigurations.GetAllControllers());
				
				foreach(GamepadAxes axes in fullList)
				{
					EditorGUILayout.LabelField("Group: "+
						(customControllers.Contains(axes) ? " [Custom]" : "") + 
						(playerPrefControllers.Contains(axes) ? " [PlayerPrefs]" : ""));
					displayAxesNames(axes);
				}
			}
		}
		
		GUILine();
		
		serialisedGamepadName = EditorGUILayout.TextField("Gamepad name", serialisedGamepadName);
		serialisedGamepad = EditorGUILayout.TextField("Serialized Gamepad Axes", serialisedGamepad);
		
		if (GUILayout.Button("Setup Gamepad from string", buttonWidth))
		{
			GamepadAxes ua = new GamepadAxes();
			ua.Deserialize(serialisedGamepad);
			GamepadInputConfigurations.AddControllerSupport(serialisedGamepadName, ua);
		}
			
		GUILine();
		
		
	}
}
