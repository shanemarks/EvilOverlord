using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace RaxterWorks
{
namespace GamepadInputManager
{

public class PolarVector2
{
	public float angle;
	public float magnitude;
	
	public PolarVector2 (Vector2 xy)
	{
		angle = (Mathf.Atan2(xy.x, xy.y) * Mathf.Rad2Deg);
		magnitude = xy.magnitude;
	}
}
	

// TODO use partials to separate Helper and Axes classes
public static partial class GamepadInput
{
	
	
	public static int MaxJoysticks { get { return 4; } }
	
	#region Raw input data
	public static int MaxButtons { get { return 20; } }
	
	public static bool GetRawButton(int button) { return GetRawButton(button, 0); }
	public static bool GetRawButton(int button, int joystickIndex)
	{
		if (button < 0 || button >= MaxButtons || joystickIndex < 0 || joystickIndex >= MaxJoysticks)
			return false;
		
		
		return Input.GetKey(KeyCode.Joystick1Button0+(MaxButtons*joystickIndex)+button);
	}
	
	public static bool GetRawButtonUp(int button) { return GetRawButtonUp(button, 0); }
	public static bool GetRawButtonUp(int button, int joystickIndex)
	{
		if (button < 0 || button >= MaxButtons || joystickIndex < 0 || joystickIndex >= MaxJoysticks)
			return false;
		
		
		return Input.GetKeyUp(KeyCode.Joystick1Button0+(MaxButtons*joystickIndex)+button);
	}
	
	public static bool GetRawButtonDown(int button) { return GetRawButtonDown(button, 0); }
	public static bool GetRawButtonDown(int button, int joystickIndex)
	{
		if (button < 0 || button >= MaxButtons || joystickIndex < 0 || joystickIndex >= MaxJoysticks)
			return false;
		
		
		return Input.GetKeyDown(KeyCode.Joystick1Button0+(MaxButtons*joystickIndex)+button);
	}
		
	
	public static int MaxAxes { get { return 8; } } // Axis 9 and 10 are broken, so they cannot be supported
	public static string GetInputAxisName(int joystickIndex, int axis) { return "_GPC Player "+(joystickIndex+1) + " Axis "+axis; }
	
	public static float GetRawAxis(int axis) { return GetRawAxis(axis, 0); }
	public static float GetRawAxis(int axis, int joystickIndex)
	{
		if (axis <= 0)
			return 0f;
		
		return Input.GetAxisRaw(GetInputAxisName(joystickIndex, axis));
	}
	
	#endregion
	
	#region Simulated button presses from axes
	static bool GetSimulatedDPadButton(PadButton dPadButton, int joystickIndex)
	{
		switch (dPadButton)
		{
		case PadButton.DPadUp:
			return GetAxis(Axis.DPadY, joystickIndex) > 0;
		case PadButton.DPadDown:
			return GetAxis(Axis.DPadY, joystickIndex) < 0;
		case PadButton.DPadLeft:
			return GetAxis(Axis.DPadX, joystickIndex) < 0;
		case PadButton.DPadRight:
			return GetAxis(Axis.DPadX, joystickIndex) > 0;
		}
		return false;
	}
	
	public static void GetSimulatedDPadButtons(out bool dpadUp, out bool dpadDown, out bool dpadLeft, out bool dpadRight, int joystickIndex)
	{
		dpadUp    = GetAxis(Axis.DPadY, joystickIndex) > 0;
		dpadDown  = GetAxis(Axis.DPadY, joystickIndex) < 0;
		dpadLeft  = GetAxis(Axis.DPadX, joystickIndex) < 0;
		dpadRight = GetAxis(Axis.DPadX, joystickIndex) > 0;
	}
	
	public static bool GetSimulatedLeftTriggerButton(int joystickIndex)
	{
		return GetAxis(Axis.LeftTrigger, joystickIndex) > 0f;
	}
	public static bool GetSimulatedRightTriggerButton(int joystickIndex)
	{
		return GetAxis(Axis.RightTrigger, joystickIndex) > 0f;
	}
	#endregion
	
	#region Simulated Axes from button pressees
	static Vector2 GetSimulatedDPadAxes(int joystickIndex)
	{
		float x = 	(GetButton(PadButton.DPadUp,    joystickIndex) ? +1f : 0f) +
					(GetButton(PadButton.DPadDown,  joystickIndex) ? -1f : 0f);
		
		float y =	(GetButton(PadButton.DPadRight, joystickIndex) ? +1f : 0f) +
					(GetButton(PadButton.DPadLeft,  joystickIndex) ? -1f : 0f);
		return new Vector2(x,y).normalized;
	}
	
	static Vector2 GetSimulatedButtonsAxes(int joystickIndex)
	{
		float x = 	(GetButton(PadButton.ActionRight, joystickIndex) ? +1f : 0f) +
					(GetButton(PadButton.ActionLeft,  joystickIndex) ? -1f : 0f);
		
		float y =	(GetButton(PadButton.ActionUp,    joystickIndex) ? +1f : 0f) +
					(GetButton(PadButton.ActionDown,  joystickIndex) ? -1f : 0f);
		return new Vector2(x,y).normalized;
	}
	static float GetSimulatedLeftTriggerAxes(int joystickIndex)
	{
		return GetButton(PadButton.LeftTrigger, joystickIndex) ? 1f : 0f;
	}
	static float GetSimulatedRightTriggerAxes(int joystickIndex)
	{
		return GetButton(PadButton.RightTrigger, joystickIndex) ? 1f : 0f;
	}
	#endregion
	
	#region Abstracted input data
	
	public static PolarVector2 GetPolarAxis(XYAxis axis) { return GetPolarAxis(axis, 0); }
	public static PolarVector2 GetPolarAxis(XYAxis axis, int joystickIndex)
	{
		return new PolarVector2(GetXYAxis(axis, joystickIndex));
	}
	
	public static Vector2 GetXYAxis(XYAxis axis) { return GetXYAxis(axis, 0); }
	public static Vector2 GetXYAxis(XYAxis axis, int joystickIndex)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return Vector2.zero;
		
		Vector2 xy = Vector2.zero;
		switch (axis)
		{
		case XYAxis.LeftAnalog:
			xy = new Vector2(GetAxis(Axis.LeftAnalogX, joystickIndex), GetAxis(Axis.LeftAnalogY, joystickIndex));
			break;
		case XYAxis.RightAnalog:
			xy = new Vector2(GetAxis(Axis.RightAnalogX, joystickIndex), GetAxis(Axis.RightAnalogY, joystickIndex));
			break;
		case XYAxis.DPad:
			int rawXAxis = connectedGamepads[joystickIndex][Axis.DPadX].code;
			int rawYAxis = connectedGamepads[joystickIndex][Axis.DPadY].code;
			if (rawXAxis == GamepadAxes.Simulate || rawYAxis == GamepadAxes.Simulate)
			{
				return GetSimulatedDPadAxes(joystickIndex); // no need to deadzone this one;
			}
			else
			{
				xy = new Vector2(GetAxis(Axis.DPadX, joystickIndex), GetAxis(Axis.DPadY, joystickIndex));
			}
			break;
		case XYAxis.ActionButtons:
			rawXAxis = connectedGamepads[joystickIndex][Axis.ActionButtonsX].code;
			rawYAxis = connectedGamepads[joystickIndex][Axis.ActionButtonsY].code;
			if (rawXAxis == GamepadAxes.Simulate || rawYAxis == GamepadAxes.Simulate)
			{
				return GetSimulatedButtonsAxes(joystickIndex); // no need to deadzone this one;
			}
			else
			{
				xy = new Vector2(GetAxis(Axis.ActionButtonsX, joystickIndex), GetAxis(Axis.ActionButtonsY, joystickIndex));
			}
			break;
		}
		float deadZone = connectedGamepads[joystickIndex].DeadZone(axis);
		if (xy.sqrMagnitude < deadZone*deadZone)
			return Vector2.zero;
		
		return xy;
	}
	
	public static float GetAxis(Axis axis) { return GetAxis(axis, 0); }
	public static float GetAxis(Axis axis, int joystickIndex)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return 0f;
		
		
		GamepadAxes.AxisDesc rawAxis = connectedGamepads[joystickIndex][axis];
			
		if (rawAxis.code == GamepadAxes.Simulate)
		{
			switch (axis)
			{
			case Axis.DPadX:
				return GetSimulatedDPadAxes(joystickIndex).x;
			case Axis.DPadY:
				return GetSimulatedDPadAxes(joystickIndex).y;
				
			case Axis.ActionButtonsX:
				return GetSimulatedButtonsAxes(joystickIndex).x;
			case Axis.ActionButtonsY:
				return GetSimulatedButtonsAxes(joystickIndex).y;
				
			case Axis.LeftTrigger:
				return GetSimulatedLeftTriggerAxes(joystickIndex);
			case Axis.RightTrigger:
				return GetSimulatedRightTriggerAxes(joystickIndex);
			}
		}
		else
		{
			float minVal = rawAxis.minExtent;
			float maxVal = rawAxis.maxExtent;
			float raxAxisValue = GetRawAxis(rawAxis.code, joystickIndex);
			
			float normalisedValue = Mathf.Clamp01((raxAxisValue - minVal) / (maxVal - minVal));
			if (axis != Axis.LeftTrigger && axis != Axis.RightTrigger)
				normalisedValue = (normalisedValue * 2f) -1f; //[0;1] -> [-1;+1]
			

			
			return normalisedValue;
		}
		return 0f;
	}
	
	public static bool GetButtonUp(PadButton button) { return GetButtonUp(button, 0); }
	public static bool GetButtonUp(PadButton button, int joystickIndex)	
	{
		return GetButtonUpDown(button, joystickIndex, true);
	}
	public static bool GetButtonDown(PadButton button) { return GetButtonDown(button, 0); }
	public static bool GetButtonDown(PadButton button, int joystickIndex)	
	{
		return GetButtonUpDown(button, joystickIndex, false);
	}
	
	static bool GetButtonUpDown(PadButton button, int joystickIndex, bool up)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return false;
		
		int rawButton = connectedGamepads[joystickIndex][button];
		
		if (rawButton == GamepadAxes.Simulate)
		{
			if (GamepadInputBackgroundProcess.hasInstance)
			{
				if (up)
					return GamepadInputBackgroundProcess.instance.GetButtonUp(button, joystickIndex);
				else
					return GamepadInputBackgroundProcess.instance.GetButtonDown(button, joystickIndex);
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (up)	
				return GetRawButtonUp(rawButton, joystickIndex);
			else
				return GetRawButtonDown(rawButton, joystickIndex);
		}
//		return false;
	}
	
	
	public static bool GetButton(PadButton button) { return GetButton(button, 0); }
	public static bool GetButton(PadButton button, int joystickIndex)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return false;
		
		int rawButton = connectedGamepads[joystickIndex][button];
		
		if (rawButton == GamepadAxes.Simulate)
		{
			switch (button)
			{
			case PadButton.DPadUp:
			case PadButton.DPadDown:
			case PadButton.DPadLeft:
			case PadButton.DPadRight:
				return GetSimulatedDPadButton(button, joystickIndex);
				
			case PadButton.LeftTrigger:
				return GetSimulatedLeftTriggerButton(joystickIndex);
			case PadButton.RightTrigger:
				return GetSimulatedRightTriggerButton(joystickIndex);
			}
		}
		else
		{
			return GetRawButton(rawButton, joystickIndex);
		}
		return false;
	}
	#endregion
	
	#region automatic gamepad config redirection
	
	/* Holds the Gamepad axes config for each controller as per Input.GetJoystickNames()
	 * So connectedGamepads[0] refers to Input.GetJoystickNames()[0] etc
	 * This array is set up initially with CheckGamepads(), which is called everytime GetButton, GetAxis or GetXYAxis is called
	 * If a gamepad connects or disconnect, this array becomes invalid and RefreshGamepads() needs to be called
	 * */
	static GamepadAxes [] connectedGamepads = null;
	
	public static GamepadAxes GetAllAxes(int joystickIndex)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return null;
		return new GamepadAxes(connectedGamepads[joystickIndex]);
	}
	
	static GamepadAxes GetGamepadAxesRef(int joystickIndex)
	{
		CheckGamepads();
		if (joystickIndex >= connectedGamepads.Length)
			return null;
		return connectedGamepads[joystickIndex];
	}
	
	public static void RefreshGamepads()
	{
		connectedGamepads = null;
	}
	
	// This function does a quick check to see if the 
	static void CheckGamepads()
	{
		if (Application.isPlaying)
		{
			if (connectedGamepads != null && connectedGamepads.Length != 0)
			{
				return;
			}
			
			string [] joystickNames = Input.GetJoystickNames();
			
			if (joystickNames.Length == 0) 
			{
				connectedGamepads = new GamepadAxes[0];
				return;
			}
			
			GamepadAxes [] allControllers  = GamepadInputConfigurations.GetAllControllers();

			
			connectedGamepads = System.Array.ConvertAll(joystickNames, (gpName) => 
			{
				GamepadAxes ua = null;// allControllers.FindLast((gpUnityAxes) => gpUnityAxes.ContainsName(gpName));
				
				for (int i = allControllers.Length - 1 ; i >= 0 ; i--)
				{
					if (allControllers[i].ContainsName(gpName))
					{
						ua = allControllers[i];
						break;
					}
				}
				if (ua == null)
				{
					ua = new GamepadAxes();
					ua.AddName(gpName);
				}
				return ua;
			});
		}
		else
		{
			connectedGamepads = null;
		}
	}
		
	#endregion
	
}


}
}


