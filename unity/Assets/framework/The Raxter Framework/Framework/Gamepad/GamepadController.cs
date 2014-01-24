using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Gamepad {XBox};
public enum XYAxis {
	LeftAnalog,
	RightAnalog,
	DPad};

public enum Axis {
	LeftAnalogX, LeftAnalogY,
	RightAnalogX, RightAnalogY,
	DPadX, DPadY,
	LeftTrigger, RightTrigger
};

public enum Button {
	ButtonDown, ButtonUp, ButtonLeft, ButtonRight, 
	LeftBumper, RightBumper, 
	LeftAnalog, RightAnalog, 
	Start, Select, Home,
	DPadUp, DPadDown, DPadLeft, DPadRight
};


public class XBOXButton
{
	public Button A = Button.ButtonDown;
	public Button B = Button.ButtonRight;
	public Button X = Button.ButtonLeft;
	public Button Y = Button.ButtonUp;
}


public class PolarVector2
{
	public float angle;
	public float magnitude;
	
	public static PolarVector2 FromVector2(Vector2 xy)
	{
		PolarVector2 result = new PolarVector2();
		
		result.angle = (Mathf.Atan2(xy.x, xy.y) * Mathf.Rad2Deg);
		result.magnitude = xy.magnitude;
		return result;
	}
}
	

public static class GamepadController
{

//	[SerializeField]
//	GamePadUnityAxes [] customControllers = null;
	
//	const int SimulateCode = -1;
	
	static GamePadUnityAxes [] defaultControllers = 
	{
		new GamePadUnityAxes()
		{
			name = "Controller (XBOX 360 For Windows)",
			leftAnalogDeadZone	= 0f,
			leftAnalogX			= 1,
			leftAnalogY			= 2,
			rightAnalogDeadZone	= 0f,
			rightAnalogX		= 4,
			rightAnalogY		= 5,
			leftTrigger			= 9,
			rightTrigger		= 10,
			leftBumper			= 4,
			rightBumper			= 5,
			dPadAxisX			= 5,
			dPadAxisY			= 6,
			dPadUp				= -2,
			dPadDown			= -2,
			dPadLeft			= -2,
			dPadRight			= -2,
			leftAnalogButton	= 8,
			rightAnalogButton	= 9,
			buttonDown 			= 0,
			buttonUp 			= 1,
			buttonLeft 			= 2,
			buttonRight 		= 3,
			startButton 		= 7,
			selectButton 		= 6,
			homeButton 			= -1,
		}
	};
	
	// look at http://wiki.etc.cmu.edu/unity3d/index.php/Joystick/Controller
	
	[System.Serializable]
	public class GamePadUnityAxes
	{
		public string name = "Unnamed";
		public float leftAnalogDeadZone = 0f;
		public int leftAnalogX = 0;
		public int leftAnalogY = 0;
		public float rightAnalogDeadZone = 0f;
		public int rightAnalogX = 0;
		public int rightAnalogY = 0;
		
		
		public int leftTrigger = 0;
		public int rightTrigger = 0;
		public int leftBumper = 0;
		public int rightBumper = 0;
		
		public int dPadAxisX 	= 0; // -2 means simulate from buttons
		public int dPadAxisY 	= 0;
		
		public int dPadUp 		= 0; // -2 means simulate from axis
		public int dPadDown 	= 0;
		public int dPadLeft 	= 0; 
		public int dPadRight 	= 0;
		
		public int leftAnalogButton = 0;
		public int rightAnalogButton = 0;
		
		public int buttonDown = 0;
		public int buttonUp = 0;
		public int buttonLeft = 0;
		public int buttonRight = 0;
		
		public int startButton = 0;
		public int selectButton = 0;
		public int homeButton = 0;
		
	
		public float DeadZone(XYAxis axis)
		{
			switch(axis)
			{
			case XYAxis.LeftAnalog:
				return leftAnalogDeadZone;
				
			case XYAxis.RightAnalog:
				return rightAnalogDeadZone;
			}
			return 0;
		}
		public int this [Axis axis]
		{
			get
			{
				switch(axis)
				{
				case Axis.LeftAnalogX:
					return leftAnalogX;
					
				case Axis.LeftAnalogY:
					return leftAnalogY;
					
				case Axis.RightAnalogX:
					return rightAnalogX;
					
				case Axis.RightAnalogY:
					return rightAnalogY;
					
				case Axis.LeftTrigger:
					return leftTrigger;
					
				case Axis.RightTrigger:
					return rightTrigger;
				}
				return 0;
			}
		}
		public int this [Button button]
		{
			get
			{
				switch(button)
				{
				case Button.LeftAnalog:
					return leftAnalogButton;
					
				case Button.RightAnalog:
					return rightAnalogButton;
					
				case Button.LeftBumper:
					return leftBumper;
					
				case Button.RightBumper:
					return rightBumper;
					
				case Button.Start:
					return startButton;
					
				case Button.Select:
					return selectButton;
					
				case Button.ButtonDown:
					return buttonDown;
					
				case Button.ButtonUp:
					return buttonUp;
					
				case Button.ButtonLeft:
					return buttonLeft;
					
				case Button.ButtonRight:
					return buttonRight;
					
				}
				return 0;
			}
		}
	}
	
	public static int MaxPlayers { get { return 4; } }
	public static int MaxAxes { get { return 10; } }
	public static int MaxButtons { get { return 19; } }
	public static string GetInputAxisName(int player, int axis) { return "_GPC Player "+(player+1) + " Axis "+axis; }
	public static string GetInputButtonName(int player, int button) { return "_GPC Player "+(player+1) + " Button "+button; }
	
	public static float GetRawAxis(int axis) { return GetRawAxis(axis, 0); }
	public static float GetRawAxis(int axis, int player)
	{
		// TODO check for simulation code (-2)
		if (axis <= 0)
			return 0f;
		
		return Input.GetAxis(GetInputAxisName(player, axis));
	}
	
	public static bool GetRawButton(int button) { return GetRawButton(button, 0); }
	public static bool GetRawButton(int button, int player)
	{
		// TODO check for simulation code (-2)
		if (button <= 0)
			return false;
		
		return Input.GetButton(GetInputButtonName(player, button));
	}
		
	public static Vector2 GetXYAxis(XYAxis axis) { return GetXYAxis(axis, 0); }
	public static Vector2 GetXYAxis(XYAxis axis, int playerIndex)
	{
		CheckGamepads();
		if (playerIndex >= gamepads.Length)
			return Vector2.zero;
		
		Vector2 xy = Vector2.zero;
		switch (axis)
		{
		case XYAxis.LeftAnalog:
			xy = new Vector2(GetAxis(Axis.LeftAnalogX, playerIndex), GetAxis(Axis.LeftAnalogY, playerIndex));
			break;
		case XYAxis.RightAnalog:
			xy = new Vector2(GetAxis(Axis.RightAnalogX, playerIndex), GetAxis(Axis.RightAnalogY, playerIndex));
			break;
		case XYAxis.DPad:
			xy = new Vector2(GetAxis(Axis.DPadX, playerIndex), GetAxis(Axis.DPadY, playerIndex));
			break;
		}
		float deadZone = gamepads[playerIndex].DeadZone(axis);
		if (xy.sqrMagnitude < deadZone*deadZone)
			return Vector2.zero;
		else
			return xy;
	}
	
	public static float GetAxis(Axis axis) { return GetAxis(axis, 0); }
	public static float GetAxis(Axis axis, int playerIndex)
	{
		CheckGamepads();
		if (playerIndex >= gamepads.Length)
			return 0f;
		
		return GetRawAxis(gamepads[playerIndex][axis], playerIndex);
	}
	
	
	public static bool GetButton(Button button) { return GetButton(button, 0); }
	public static bool GetButton(Button button, int playerIndex)
	{
		CheckGamepads();
		if (playerIndex >= gamepads.Length)
			return false;
		
		return GetRawButton(gamepads[playerIndex][button], playerIndex);
	}
	
	static GamePadUnityAxes [] gamepads = null;
	
	static void CheckGamepads()
	{
		if (gamepads != null && gamepads.Length != 0)
		{
			return;
		}
		
		string [] joystickNames = Input.GetJoystickNames();
		
		if (joystickNames.Length == 0) return;
		
		List<GamePadUnityAxes> allControllers = new List<GamePadUnityAxes>();
		allControllers.AddRange(defaultControllers);
		
//		Dictionary<string, GamePadUnityAxes> nameToAxes = allControllers.CreateLookup((controller) => controller.name);
//		Debug.Log (string.Join(", ", joystickNames));
//		gamepads = System.Array.ConvertAll(joystickNames, (gpName) => nameToAxes[gpName]);
		gamepads = System.Array.ConvertAll(joystickNames, (gpName) => allControllers.Find((gpUnityAxes) => gpUnityAxes.name == gpName));
		
	}
	
	
}
















