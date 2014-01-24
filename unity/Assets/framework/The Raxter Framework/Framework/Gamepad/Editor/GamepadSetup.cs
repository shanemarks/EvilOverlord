using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GamepadSetup : EditorWindow
{
	
	[MenuItem ("Window/Gamepad Setup")]
	static void Init () 
	{
		EditorWindow.GetWindow (typeof (GamepadSetup));
	}
	
	string GetInputManagerSerializedAxis(int player, int axis)
	{
		return  "  - m_Name: "+GamepadController.GetInputAxisName(player, axis+1)+"\n" +
    			"    sensitivity: 1\n"+
		    	"    type: 2\n" +
		    	"    axis: "+axis+"\n" +
		    	"    joyNum: "+(player+1)+"\n";
	}
	string GetInputManagerSerializedButton(int player, int button)
	{
		return  "  - m_Name: "+GamepadController.GetInputButtonName(player, button)+"\n" +
    			"    sensitivity: 1000\n"+
    			"    positiveButton: joystick "+(player+1)+" button "+button+"\n"+
		    	"    type: 2\n" +
		    	"    joyNum: "+(player+1)+"\n";
	}
	
	void OnEditorUpdate()
	{
		this.Repaint();
	}
	
	const string inputFilePath = "ProjectSettings/InputManager.asset";
	const string inputFileBackupPath = "ProjectSettings/InputManager.asset.gpcbackup";
	
	bool showRaw = false;
	bool showAxes = false;
	
	int selectedPlayer = 0;
	int numberOfPlayersToGen = 4;
	
	void BackUpInputFile()
	{
		FileUtil.ReplaceFile(inputFilePath, inputFileBackupPath);
	}
	
	void RestoreFromBackup()
	{
		FileUtil.ReplaceFile(inputFileBackupPath, inputFilePath);
	}
	
	void OnGUI()
	{
		EditorApplication.update -= OnEditorUpdate;
		EditorApplication.update += OnEditorUpdate;
		
		EditorApplication.playmodeStateChanged -= OnEditorUpdate;
		EditorApplication.playmodeStateChanged += OnEditorUpdate;
		
		if (GUILayout.Button("Add Game Controller Input Axes"))
		{
			string toAppend = "\n";
			for (int p = 0 ; p < numberOfPlayersToGen ; p++)
			{
				for (int a = 0 ; a < GamepadController.MaxAxes ; a++)
				{
					toAppend += GetInputManagerSerializedAxis(p, a);
				}
				for (int b = 0 ; b < GamepadController.MaxButtons ; b++)
				{
					toAppend += GetInputManagerSerializedButton(p, b);
				}
			}
//			Debug.Log (toAppend);
			System.IO.File.AppendAllText(inputFilePath, toAppend);
			AssetDatabase.Refresh();
		}
		
		numberOfPlayersToGen = EditorGUILayout.IntSlider("Joysticks to setup", numberOfPlayersToGen, 1, 4);
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Create Backup"))
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
		if (System.IO.File.Exists(inputFileBackupPath) && GUILayout.Button("Restore From Backup"))
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
		EditorGUILayout.EndHorizontal();
		
		if (GUILayout.Button("Clear all Input Axes (even mouse and keyboard)"))
		{
			if (EditorUtility.DisplayDialog("Are you sure you want to clear?", "This will delete *all* inputs. A backup will be created however.", "Rid me of them!", "No, keep them"))
			{
				string toWrite = 	"%YAML 1.1\n"+
									"%TAG !u! tag:unity3d.com,2011:\n"+
									"--- !u!13 &1\n"+
									"InputManager:\n"+
									"  m_ObjectHideFlags: 0\n"+
									"  m_Axes:\n";
				System.IO.File.WriteAllText(inputFilePath, toWrite);
				AssetDatabase.Refresh();
			}
		}
		
		
		if (Application.isPlaying)
		{
//			EditorGUILayout.FloatField("Time", Time.realtimeSinceStartup);
			EditorGUILayout.LabelField("Click on the Game Window in order for joystick input to be readable");
			
			
			selectedPlayer = EditorGUILayout.IntSlider("Player", selectedPlayer, 0, 3);
			
			showRaw = EditorGUILayout.Foldout(showRaw, "Show Raw Inputs");
			
			if (showRaw)
			{
				try
				{
					EditorGUILayout.BeginHorizontal();
//					EditorGUILayout.BeginVertical();
//					for (int ab = 0 ; ab < Mathf.Max(GamepadController.MaxAxes, GamepadController.MaxButtons) ; ab++)
//					{
//						EditorGUILayout.PrefixLabel("Player "+selectedPlayer);
//					}
//					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					for (int a = 1 ; a <= GamepadController.MaxAxes ; a++)
					{
						EditorGUILayout.FloatField("Axis "+a, GamepadController.GetRawAxis(a, selectedPlayer));
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					for (int b = 0 ; b < GamepadController.MaxButtons ; b++)
					{
						EditorGUILayout.Toggle("Button "+b, GamepadController.GetRawButton(b, selectedPlayer));
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
				}
				catch (UnityException e)
				{
					EditorGUILayout.LabelField("Axes are not set up");
					EditorGUILayout.LabelField(e.Message);
				}
			}
			
			
			showAxes = EditorGUILayout.Foldout(showAxes, "Show Controller Inputs");
			
			if (showAxes)
			{
				try
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					foreach (XYAxis xyAxis in System.Enum.GetValues(typeof(XYAxis)))
					{
						EditorGUILayout.Vector2Field(""+xyAxis, GamepadController.GetXYAxis(xyAxis, selectedPlayer));
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					foreach (Axis axis in System.Enum.GetValues(typeof(Axis)))
					{
						EditorGUILayout.FloatField(""+axis, GamepadController.GetAxis(axis, selectedPlayer));
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					foreach (Button button in System.Enum.GetValues(typeof(Button)))
					{
						EditorGUILayout.Toggle(""+button, GamepadController.GetButton(button, selectedPlayer));
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
				}
				catch (UnityException e)
				{
					EditorGUILayout.LabelField("Axes are not set up");
					EditorGUILayout.LabelField(e.Message);
				}
			}
		}
		else
		{
			EditorGUILayout.LabelField("Play game to test inputs");
		}
		
		
	}
}
