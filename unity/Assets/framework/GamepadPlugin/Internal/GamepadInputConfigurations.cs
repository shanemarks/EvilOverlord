using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RaxterWorks
{
namespace GamepadInputManager
{

public class GamepadInputConfigurations : ScriptableObject
{
	[SerializeField]
	bool _createBackgroundProcessOnCall = true;
	
	public static bool CreateBackgroundProcessOnCall
	{
		get { return instance._createBackgroundProcessOnCall; }
		set { instance._createBackgroundProcessOnCall = value; }
	}
	
	[SerializeField]
	GamepadAxes [] customGamepadConfigurations = new GamepadAxes[0];
	
	public static void UpdateCustomControllerSupport(GamepadAxes controllerAxes)
	{
		if (controllerAxes.names.Length == 0)
			return;
		string name = controllerAxes.names[0];
		AddControllerSupport(name, controllerAxes);
	}
	
	// TODO check that the controller isn't already setup with another name!!! TODO TODO
	public static void AddControllerSupport(string controllerName, GamepadAxes controllerAxes = null)
	{
		if (controllerAxes == null)
		{
			controllerAxes = new GamepadAxes();
		}
		
		// if we are playing the game and not in the editor, we can't save to the customGamepads since on restart that info is lost (TODO test!)
		// in this case we save it to the player prefs
#if !UNITY_EDITOR
		
		AddControllerSupportToPlayerPrefs(controllerAxes);
		return;
#endif
		
		if (instance.customGamepadConfigurations == null)
		{
			instance.customGamepadConfigurations = new GamepadAxes [0];
		}
		
		//do check for controller with different name but same setup here! TODO
		
		int nameIndex = System.Array.FindIndex(instance.customGamepadConfigurations, (obj) => obj.ContainsName(controllerName));
		
		int configIndex = System.Array.FindIndex(instance.customGamepadConfigurations, (obj) => obj.SameConfigAs(controllerAxes));
		
		
		if (configIndex != -1) // then there is a Gamepad axes that is setup for this controller already
		{
			// just add the name
			instance.customGamepadConfigurations[configIndex].AddName(controllerName);
		}
		else // this is a new configuration
		{
			if (nameIndex != -1)  // controller name already exists	
			{
				// overwrites the axes with the custom control
				controllerAxes.names = instance.customGamepadConfigurations[nameIndex].names;
				instance.customGamepadConfigurations[nameIndex] = controllerAxes;
				GamepadInput.RefreshGamepads();
			}
			else// nothing by this name exists already
			{
				controllerAxes.names = new string [1] {controllerName};
				System.Array.Resize(ref instance.customGamepadConfigurations, instance.customGamepadConfigurations.Length + 1);
				instance.customGamepadConfigurations[instance.customGamepadConfigurations.Length - 1] = controllerAxes;
				GamepadInput.RefreshGamepads();
				
			}
		}
		
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			EditorUtility.SetDirty(instance);
		}
#endif
	}
	
	
	static void AddControllerSupportToPlayerPrefs(GamepadAxes controllerAxes)
	{
		// TODO for free version, only have the player prefs version, disable the scriptable object
		// for free version, make a static class that only saves to player prefs and that is stored in the DLL)
		// full version GamepadInputConfigurations will just reference this class, free version will ... something)
	}
	
	public static GamepadAxes [] GetDefaultGamepads()
	{
//		return new UnityAxes [0];
		GamepadAxes [] ret = new GamepadAxes [1];
		
		ret[0] = GetWinXBOXGamepadAxes();
		ret[0].AddName("Controller (XBOX 360 For Windows)");
		
		return ret;
	}
	
	static GamepadAxes GetWinXBOXGamepadAxes()
	{
		GamepadAxes ret = new GamepadAxes();
		ret.actionDown 			= 0;
		ret.actionRight 		= 1;
		ret.actionLeft 			= 2;
		ret.actionUp 			= 3;
		ret.actionButtonsAxisX			= new GamepadAxes.AxisDesc(GamepadAxes.Simulate, -1f, 1f);
		ret.actionButtonsAxisY			= new GamepadAxes.AxisDesc(GamepadAxes.Simulate, -1f, 1f);
		ret.leftAnalogStickDeadZone		= 0f;
		ret.leftAnalogX			= new GamepadAxes.AxisDesc(1, -1f, 1f);
		ret.leftAnalogY			= new GamepadAxes.AxisDesc(2, -1f, 1f, true);
		ret.rightAnalogStickDeadZone	= 0f;
		ret.rightAnalogX		= new GamepadAxes.AxisDesc(4, -1f, 1f);
		ret.rightAnalogY		= new GamepadAxes.AxisDesc(5, -1f, 1f, true);
//		ret.invertLeftAnalogY 	= true;
//		ret.invertRightAnalogY 	= true;
//		ret.invertDPadY 		= false;
		ret.leftTriggerAxis		= new GamepadAxes.AxisDesc(3, 0.05f, +1f);
		ret.rightTriggerAxis	= new GamepadAxes.AxisDesc(3, 0, -1f);
		ret.leftTriggerButton	= GamepadAxes.Simulate;
		ret.rightTriggerButton	= GamepadAxes.Simulate;
		ret.leftBumper			= 4;
		ret.rightBumper			= 5;
		ret.dPadAxisX			= new GamepadAxes.AxisDesc(6, -1f, 1f);
		ret.dPadAxisY			= new GamepadAxes.AxisDesc(7, -1f, 1f);
		ret.dPadUp				= GamepadAxes.Simulate;
		ret.dPadDown			= GamepadAxes.Simulate;
		ret.dPadLeft			= GamepadAxes.Simulate;
		ret.dPadRight			= GamepadAxes.Simulate;
		ret.leftAnalogButton	= 8;
		ret.rightAnalogButton	= 9;
		ret.startButton 		= 7;
		ret.selectButton 		= 6;
		ret.homeButton 			= GamepadAxes.NoInput;
		return ret;
	}
	
	// look at http://wiki.etc.cmu.edu/unity3d/index.php/Joystick/Controller
	
	
	
	public static GamepadAxes [] GetCustomControllers() // TODO
	{
		GamepadAxes [] ret = new GamepadAxes[instance.customGamepadConfigurations.Length];
		System.Array.Copy(instance.customGamepadConfigurations, ret, ret.Length);
		return ret;
	}
	
	public static GamepadAxes [] GetPlayerPrefControllers() // TODO
	{
		return new GamepadAxes [0];
	}
	
	public static GamepadAxes[] GetAllControllers(bool includeCopies = false)
	{
		List<GamepadAxes> axesList = new List<GamepadAxes>();
		
		GamepadAxes [][] axesGroups = new GamepadAxes [3][];
		axesGroups[0] = GetPlayerPrefControllers();
		axesGroups[1] = GetCustomControllers();
		axesGroups[2] = GetDefaultGamepads();
		
		foreach(GamepadAxes [] axesGroup in axesGroups)
		{
			foreach (GamepadAxes axes in axesGroup)
			{
				// if we are including copies, or if we are not and it is not included already
				if (includeCopies || axesList.FindIndex((obj) => obj.ContainsANameFrom(axes.names)) == -1)
				{
					axesList.Add(axes);
				}
			}
		}
		return axesList.ToArray();
	}
	
	
	
	#region ScriptableObjectSingleton code
	
	static GamepadInputConfigurations _instance = null;
	
	static GamepadInputConfigurations instance
	{
		get
		{
			if(_instance != null)
			{
				return _instance;
			}
			
			Debug.Log("Loading " + typeof(GamepadInputConfigurations).Name + " from resource folder");
			_instance = Resources.Load(typeof(GamepadInputConfigurations).Name, typeof(GamepadInputConfigurations)) as GamepadInputConfigurations;
			
			if(_instance == null)
			{
#if UNITY_EDITOR
				Debug.LogWarning(typeof(GamepadInputConfigurations).Name + " resource does not exist. Creating in Assets/Resources");
				_instance = ScriptableObject.CreateInstance<GamepadInputConfigurations>();
				
				System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("Assets/Resources");
				if(!directory.Exists)
				{
					directory.Create();
				}
				
				AssetDatabase.CreateAsset(_instance, "Assets/Resources/" + typeof(GamepadInputConfigurations).Name + ".asset");
				AssetDatabase.SaveAssets();
#else		
				Debug.LogError("Error getting the " + typeof(GamepadInputConfigurations).Name + " resource");
#endif
			}
			
			return _instance;
		}
	}
	
	#endregion
}
		
}
}