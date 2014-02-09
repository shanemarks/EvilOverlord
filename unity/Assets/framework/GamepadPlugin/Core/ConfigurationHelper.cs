using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace RaxterWorks
{
namespace GamepadInputManager
{

public static partial class GamepadInput
{

	
	/* Represents an input type, either a button or an axis, 
	 * For instance some controllrs define the dpad as buttons, whereas others define then as axes
	 * Namely, the Dpad and Triggers can be either axes or buttons
	 * The 'Action' buttons have an axis defined for southpaw configurations
	 * 
	 * InputType.Either refers to:
	 * Button.DPadDown/Up/Left/Right 	<-> Axis.DPadX/Y
	 * Button.ButtonDown/Up/Left/Right 	<-> Axis.ButtonX/Y
	 * Button.TriggerLeft/Right 		<-> Axis.TriggerLeft/Right
	 * 
	 */
	public enum InputType {None, Button, Axis, Either};
	
	/* The extent represents a direction in which an axis can be held down
	 * 
	 * This is:
	 * Left/Right  for the Analog and D-Pad X-axes
	 * Up/Down     for the Analog and D-Pad Y-axes
	 * Max?Neutral for the Triggers
	 * 
	 * Essentially Max, Left, and Up are the same thing and Min, Neutral, Right, and Down are the same values but in different contexts
	 */
	public enum Extent {Max = 1, Min = 0, Neutral = 0, Right = 1, Up = 1, Left = 0, Down = 0, None = -1}
	

	public struct InputDescription
	{
		public InputType inputType;// = InputType.None;
		public PadButton button;// = Button.NullButton;
		public Axis axis;// = Axis.NullAxis;
		public Extent extent;// = Extent.None;
		
	}
	
	#region CalibrationHelper
	// TODO document how to use
	
	public enum CalibrationState {NotCalibrated, Calibrating, WaitingForNeutralController, Calibrated}
	
	public static CalibrationHelper CalibrateController(int joystickIndex)
	{
		return CalibrationHelper.CalibrateController(joystickIndex);
	}
	
	public class CalibrationHelper
	{
		private CalibrationHelper() {}
		
		bool IsCalibrating
		{
			get { return _axesRef != null; }
		}
		
		GamepadAxes _axesRef = null;
		string _calibrationBackup = "";
		int _joystickIndex = -1;
		Dictionary<PadButton, int> buttonCodes = new Dictionary<PadButton, int>();
		float _timeSinceCalibration = -1f;
		
		
		public float axisCalibrationTimeout = 0.5f;
		
		struct AxisExtent
		{
			public AxisExtent(Axis a, Extent e) { axis = a; extent = e; }
			public Axis axis;
			public Extent extent;
		}
		AxisExtent NullAxisExtent { get { return new AxisExtent(Axis.NullAxis, Extent.None); } }
		struct CodeExtent
		{
			public CodeExtent(int c, float e) { code = c; extentValue = e; }
			public int code;
			public float extentValue;
		}
		CodeExtent NullCodeExtent { get { return new CodeExtent(-1, 0f); } }
		
		Dictionary<AxisExtent, CodeExtent> axesDescs = new Dictionary<AxisExtent, CodeExtent>();
		AxisExtent _currentlyCalibrating = new AxisExtent();
		CodeExtent _currentlyCalibratingValues = new CodeExtent();
		
		List<float> neutralPositions = new List<float>();
		
		bool _waitingForNeutralController = false;
	
		public static CalibrationHelper CalibrateController(int joystickIndex)
		{
			GamepadAxes axesRef = GamepadInput.GetGamepadAxesRef(joystickIndex);
			if (axesRef == null)
			{
				return null;
			}
			CalibrationHelper helper = new CalibrationHelper();
			
			helper._axesRef = axesRef;
			helper._calibrationBackup = helper._axesRef.Serialize();
			helper._joystickIndex = joystickIndex;
			
			helper.CalibrateNeutralPositions();
			
			return helper;
			
		}
		
		public void CalibrateNeutralPositions()
		{
			for (int a = 0 ; a < MaxAxes ; a++)
			{
				neutralPositions.Add(GetRawAxis(a, _joystickIndex));
			}
		}
		
		
		public int GetCurrentRawButton()
		{
			for (int b = 0 ; b < MaxButtons ; b++)
			{
				if (GetRawButton(b, _joystickIndex))
				{
					return b;
				}
			}
			return -1;
		}
		
		
		public int GetCurrentRawAxis(out float extent)
		{
			for (int a = 0 ; a < MaxAxes ; a++)
			{
				float ext = GetRawAxis(a, _joystickIndex);
				if (Mathf.Abs (ext-neutralPositions[a]) > 0.4f)
				{
					extent = ext;
					return a;
				}
			}
			
			extent = 0f;			
			return -1;
		}
		
		public int GetCurrentRawAxis()
		{
			float extent;
			return GetCurrentRawAxis(out extent);
		}
		
		
		public bool UpdateController()
		{
			// create GamePadAxes
			GamepadAxes ua = new GamepadAxes();
			ua.AddName(Input.GetJoystickNames()[_joystickIndex]);
			
			foreach (Axis a in System.Enum.GetValues(typeof(Axis)))
			{
				if (a == Axis.NullAxis) continue;
				
				GamepadAxes.AxisDesc axisDesc = GamepadAxes.UndefinedAxis;
				AxisExtent maxAxisExtent = new AxisExtent(a, Extent.Max);
				AxisExtent minAxisExtent = new AxisExtent(a, Extent.Min);
				
				if (axesDescs.ContainsKey(maxAxisExtent))
				{
					axisDesc.maxExtent = axesDescs[maxAxisExtent].extentValue;
					axisDesc.code = axesDescs[maxAxisExtent].code;
				}
				if (axesDescs.ContainsKey(minAxisExtent))
				{
					axisDesc.minExtent = axesDescs[minAxisExtent].extentValue;
					if (axisDesc.code != axesDescs[minAxisExtent].code) // both extnts have differnt codes
						return false;
				}
				ua[a] = axisDesc;
			}
			foreach (PadButton b in System.Enum.GetValues(typeof(PadButton)))
			{
				if (buttonCodes.ContainsKey(b))
					ua[b] = buttonCodes[b];
			}
			
			
			GamepadInputConfigurations.UpdateCustomControllerSupport(ua);
			return true;
		}
		
		public void RestoreController()
		{
			GamepadAxes ua = new GamepadAxes();
			ua.AddName(Input.GetJoystickNames()[_joystickIndex]);
			ua.Deserialize(_calibrationBackup);
			GamepadInputConfigurations.UpdateCustomControllerSupport(ua);
		}
		
		public InputDescription GetUncalibratedInput()
		{
			InputDescription inputDesc;
			
			inputDesc.inputType = GetUncalibratedInput(out inputDesc.button, out inputDesc.axis, out inputDesc.extent);
			return inputDesc;
		}
		
		public InputType GetUncalibratedInput(out PadButton button, out Axis axis, out Extent extent)
		{
			button = PadButton.NullButton;
			axis = Axis.NullAxis;
			extent = Extent.None;
			
			
			Extent [] extents = {Extent.Min, Extent.Max};
			foreach (Axis a in System.Enum.GetValues(typeof(Axis)))
			{
				if (a == Axis.NullAxis) continue;
				foreach (Extent e in extents)
				{
//					Debug.Log(a+":"+e);
					
					if ((a == Axis.LeftTrigger || a == Axis.RightTrigger) && e == GamepadInput.Extent.Min)
						continue;
					
					if (!axesDescs.ContainsKey(new AxisExtent(a,e)))
					{
						axis = a;
						extent = e;
						
						button = GetMirroredButton(a,e);
						if (button == PadButton.NullButton)
							return InputType.Axis;
						else
							return InputType.Either;
					}
				}
			}
			
			foreach (PadButton b in System.Enum.GetValues(typeof(PadButton)))
			{
				if (b == PadButton.NullButton) continue;
				if (!buttonCodes.ContainsKey(b))
				{
					button = b;
					
					axis = GetMirroredAxis(b, out extent);
					if (axis == Axis.NullAxis)
						return InputType.Button;
					else
						return InputType.Either;
					
				}
			}
					
			return InputType.None;
		}
		
		public void CalibrateAsNoInput(PadButton button)
		{
			buttonCodes[button] = GamepadAxes.NoInput;
		}
		public void CalibrateAsNoInput(Axis axis, Extent extent)
		{
			axesDescs[new AxisExtent(axis, extent)] = new CodeExtent(GamepadAxes.NoInput, 0f);
		}
		
		bool CheckForNeutralController()
		{
			bool isNeutral = GetCurrentRawAxis() == -1 && GetCurrentRawButton() == -1;
			if (isNeutral)
			{
				_waitingForNeutralController = false;
			}
			
			return isNeutral;
		}
		
		public CalibrationState Calibrate(InputDescription inputDesc)
		{
			if (inputDesc.inputType == GamepadInput.InputType.Button || 
				inputDesc.inputType == GamepadInput.InputType.Either)
			{
				return Calibrate(inputDesc.button);
			}
			else if (inputDesc.inputType == GamepadInput.InputType.Axis)
			{
				return Calibrate(inputDesc.axis, inputDesc.extent);
			}
			
			// nothing to calibrate
			return CalibrationState.NotCalibrated;
		}
		
		public CalibrationState Calibrate(PadButton button)
		{
			// we are waiting for a neutral controller
			if (_waitingForNeutralController)
				return  CheckForNeutralController() ? CalibrationState.Calibrated : CalibrationState.WaitingForNeutralController;
			
			Extent mirroredExtent;
			Axis mirroredAxis = GetMirroredAxis(button, out mirroredExtent);
			
			CalibrationState success;
			if (mirroredAxis != Axis.NullAxis)
			{
				success = CalibrateButtonAndAxis(button, mirroredAxis, mirroredExtent);
			}
			else
			{
				success = CalibrateButton(button);
			}
			
			if (success == CalibrationState.Calibrated) 
			{
				_waitingForNeutralController = true;
				return CalibrationState.WaitingForNeutralController;
			}
			return success; // either Calibrating (still), or not calibrated
		}
		
		// TODO if calibrating an axis, wait for max value to remain unchanged for 0.5s then return true
		public CalibrationState Calibrate(Axis axis, Extent extent)
		{
			// we are waiting for a neutral controller
			if (_waitingForNeutralController)
				return  CheckForNeutralController() ? CalibrationState.Calibrated : CalibrationState.WaitingForNeutralController;
			
			PadButton mirroredButton = GetMirroredButton(axis, extent);
			
			CalibrationState success;
			if (mirroredButton != PadButton.NullButton)
			{
				success = CalibrateButtonAndAxis(mirroredButton, axis, extent);
			}
			else
			{
				success = CalibrateAxis(axis, extent);
			}
//			Debug.Log ("Calibration Success? "+success);
			if (success == CalibrationState.Calibrated) 
			{
				_waitingForNeutralController = true;
				return CalibrationState.WaitingForNeutralController;
			}
			return success; // either Calibrating (still), or not calibrated
		}
		
		
		CalibrationState CalibrateButton(PadButton button)
		{
			int buttonIndex = GetCurrentRawButton();
			
			if (buttonIndex == -1)
			{
				return CalibrationState.NotCalibrated;
			}
			
			buttonCodes[button] = buttonIndex; 
			return CalibrationState.Calibrated;
		}
		CalibrationState CalibrateAxis(Axis axis, Extent extent)
		{
			float extentValue;
			int axisIndex = GetCurrentRawAxis(out extentValue);
			
			if (axisIndex == -1)
			{
				_currentlyCalibrating = new AxisExtent();
				_currentlyCalibratingValues = new CodeExtent();
				return CalibrationState.NotCalibrated;
			}
			
			AxisExtent ae = new AxisExtent(axis, extent);
			
			// assume that we will overwrite any calibration attempt
			bool overwriteCalibration = true;
			// TODO check that the Dictionary is hashing correctly, also check this equality // TODO looks right
			
			// we are indeed calibrating the same axis-extent
			if (ae.axis == _currentlyCalibrating.axis && ae.extent == _currentlyCalibrating.extent)
			{
				// if this extent is less than the old one, we do not wish to overwrite the calibration
				float oldExtent = _currentlyCalibratingValues.extentValue;
				if (Mathf.Abs(extentValue) <= Mathf.Abs(oldExtent))
				{
					overwriteCalibration = false;
				}
			}
			
//			Debug.Log("Has AxisExtent? "+axesDescs.ContainsKey(ae));
			if (overwriteCalibration)
			{
				// we overwrite teh calibration
				Debug.Log ("Calibrating "+axis+"("+extent+") with "+axisIndex+" @ "+extentValue);
				_currentlyCalibrating = ae;
				_currentlyCalibratingValues = new CodeExtent(axisIndex, extentValue);
				_timeSinceCalibration = Time.realtimeSinceStartup;
				// we have not yet calibrated
				return CalibrationState.Calibrating;
			}
			else // check if we've not calibrated in a while
			{
//				Debug.Log ((_timeSinceCalibration + _axisCalibrationTimeout) +" > "+Time.realtimeSinceStartup);
				if ( TimeUntilCalibrationComplete <= 0f)
				{
					// if we've been without change for a while now, we save the calibration
					axesDescs[_currentlyCalibrating] = _currentlyCalibratingValues;
					_currentlyCalibrating = new AxisExtent();
					_currentlyCalibratingValues = new CodeExtent();
					return CalibrationState.Calibrated;
				}
				else
				{
					// otherwise it's not yet calibrated
					return CalibrationState.Calibrating;
				}
			}
		}
		
		public float TimeUntilCalibrationComplete
		{
			get
			{
				if (_currentlyCalibrating.axis == Axis.NullAxis) 
					return 0f;
				return Mathf.Max (axisCalibrationTimeout - (Time.realtimeSinceStartup - _timeSinceCalibration), 0f);
			}
		}
		
		// TODO Trigger Buttons don't seem to be calibrating
		CalibrationState CalibrateButtonAndAxis(PadButton button, Axis axis, Extent extent)
		{
			CalibrationState buttonCalibrated = CalibrateButton(button);
			if (buttonCalibrated == CalibrationState.Calibrated)
			{
				AxisExtent ae = new AxisExtent(axis, extent);
				axesDescs[ae] = new CodeExtent(GamepadAxes.Simulate, GetDefaultExtent(axis, extent));
				return CalibrationState.Calibrated;
			}
			
			CalibrationState axisCalibrated = CalibrateAxis(axis, extent);
			if (axisCalibrated == CalibrationState.Calibrated)
			{
				buttonCodes[button] = GamepadAxes.Simulate;
				return CalibrationState.Calibrated;
			}
			
			return axisCalibrated;
		}
		
		static PadButton GetMirroredButton(Axis axis, Extent extent)
		{
			switch (axis)
			{
			case Axis.DPadX:		return extent == GamepadInput.Extent.Left 	? PadButton.DPadLeft	: PadButton.DPadRight;
			case Axis.DPadY:		return extent == GamepadInput.Extent.Up 	? PadButton.DPadUp		: PadButton.DPadDown;
				
			case Axis.ActionButtonsX:		return extent == GamepadInput.Extent.Left 	? PadButton.ActionLeft	: PadButton.ActionRight;
			case Axis.ActionButtonsY:		return extent == GamepadInput.Extent.Up 	? PadButton.ActionUp	: PadButton.ActionDown;
				
			case Axis.LeftTrigger:	return PadButton.LeftTrigger;
			case Axis.RightTrigger:	return PadButton.RightTrigger;
			}
			
			return PadButton.NullButton;
		}
		
		static Axis GetMirroredAxis(PadButton button, out Extent extent)
		{
			switch (button)
			{
			case PadButton.DPadLeft:		extent = Extent.Left;	return Axis.DPadX;
			case PadButton.DPadRight:		extent = Extent.Right; 	return Axis.DPadX;
			case PadButton.DPadUp:			extent = Extent.Up;  	return Axis.DPadY;
			case PadButton.DPadDown:		extent = Extent.Down; 	return Axis.DPadY;
				
			case PadButton.ActionLeft:		extent = Extent.Left;	return Axis.ActionButtonsX;
			case PadButton.ActionRight:	extent = Extent.Right;	return Axis.ActionButtonsX;
			case PadButton.ActionUp:		extent = Extent.Up;		return Axis.ActionButtonsY;
			case PadButton.ActionDown:		extent = Extent.Down;	return Axis.ActionButtonsY;
				
			case PadButton.LeftTrigger:	extent = Extent.Max;	return Axis.LeftTrigger;
			case PadButton.RightTrigger:	extent = Extent.Max;	return Axis.RightTrigger;
			}
			extent = Extent.None;
			return Axis.NullAxis;
		}
		
		static float GetDefaultExtent(Axis axis, Extent extent)
		{
			if (extent == Extent.Max)
			{
				return 1f;
			}
			else
			{
				if (axis == Axis.LeftTrigger || axis == Axis.RightTrigger)
				{
					return 0f;
				}
				else
				{
					return -1f;
				}
			}
		}
		
		
	}

	#endregion
}
		
		
}
}