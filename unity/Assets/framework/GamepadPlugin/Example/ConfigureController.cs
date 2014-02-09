using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RaxterWorks.GamepadInputManager;

public class ConfigureController : MonoBehaviour 
{
	
	int selectedControllerIndex = 0;
	
	GamepadInput.CalibrationHelper calibrationHelper;
	
	
	void OnGUI () 
	{
		DrawGamepadSelectionGUI();
		
		DrawGamepadOutputGUI();
		
		
		if (calibrationHelper == null)
		{
			if (DrawButton(0.475f, 0.7f, "Calibrate Controller", 2f))
			{
				/* Call CalibrateController(controllerIndex) to calibrate (or recalibrate) the controller 
				 * at the specified index. This function returns a CalibrationHelper class that encapsulates
				 * many useful functions to calibrate an unknown controller.
				 */
				calibrationHelper = GamepadInput.CalibrateController(selectedControllerIndex);
				
				// If we try calibrate a controller that does not exist, CalibrateController returns null
				if (calibrationHelper != null)
				{
					// We can set a custom calibration time
					calibrationHelper.axisCalibrationTimeout = 2f;
					
					// We can call UpdateController() to reflect the intermediate controller configuration
					calibrationHelper.UpdateController();
				}
			}
			return;
		}
		
		
		if (DrawButton(0.475f, 0.7f, "Cancel Calibration", 2f))
		{
			// Restore controller restores the controller to it's initial configuration (if it had one)
			calibrationHelper.RestoreController();
			calibrationHelper = null;
			return;
		}
		
		
		GamepadInput.InputDescription inputDesc = calibrationHelper.GetUncalibratedInput();
		
		
		DrawText(0.475f, 0.75f, "Axis held down"+calibrationHelper.GetCurrentRawAxis());
		DrawText(0.475f, 0.80f, "Button pressed"+calibrationHelper.GetCurrentRawButton());
		
		if (inputDesc.inputType == GamepadInput.InputType.None)
		{
			calibrationHelper = null;
			DrawText(0.475f, 0.85f, "No more inputs to calibrate");
		}
		else 
		{
			/* the Calibration function returns a CalibrationState, which is either...
			 * 
			 * Calibrating: 				Is in the process of calibrating an input, will finish after timeout has been reached
			 * WaitingForNeutralController:	Once the input has been calibrated, the calibration helper will wait until 
		 	 * 									the contoller is neutral again before it actually saves the information
		 	 * Calibrated:					The control has been calibrated and is ready for a new input
		 	 * NotCalibrated:				The input is not yet calibrated, nor is it currently calibrating
			 * 
			 */
			GamepadInput.CalibrationState calibrationState = calibrationHelper.Calibrate(inputDesc);
			
			switch (calibrationState)
			{
			case GamepadInput.CalibrationState.Calibrating:
				DrawText(0.475f, 0.85f, "Calibrating in "+calibrationHelper.TimeUntilCalibrationComplete);
				break;
			
			case GamepadInput.CalibrationState.WaitingForNeutralController:
				DrawText(0.475f, 0.85f, "Calibrated, return controller to neutral position");
				break;
				
			case GamepadInput.CalibrationState.Calibrated:
				/* Running UpdateController() is not necessary, it simply saves the calibrated inputs to the configuration
				 * (if unfinished, it saves what inputs have been calibrated)
				 */
				calibrationHelper.UpdateController();
				break;
				
			case GamepadInput.CalibrationState.NotCalibrated:
				DrawText(0.475f, 0.85f, GetInputDescriptionText(inputDesc));
				break;
			}
		}
		
	}
	
	// Returns a string describing the given input descrition
	static string GetInputDescriptionText(GamepadInput.InputDescription inputDesc)
	{
		switch (inputDesc.inputType)
		{
		case GamepadInput.InputType.Button:
		case GamepadInput.InputType.Either:
			return "Press button: "+inputDesc.button;
		case GamepadInput.InputType.Axis:
		
			switch (inputDesc.axis)
			{
			case Axis.LeftAnalogX:
			case Axis.RightAnalogX:
				return "Hold axis "+inputDesc.axis +" in "+(inputDesc.extent == GamepadInput.Extent.Left ? "Left" : "Right");
			case Axis.LeftAnalogY:
			case Axis.RightAnalogY:
				return "Hold axis "+inputDesc.axis +" in "+(inputDesc.extent == GamepadInput.Extent.Up ? "Up" : "Down");
			case Axis.LeftTrigger:
			case Axis.RightTrigger:
			default:
				return "Hold axis "+inputDesc.axis;
				// as D-Pad can Either be a button or an axis, it is covered by the Either case above
			}
		case GamepadInput.InputType.None:
		default:
			return "No inputs to calibrate";
		}
	}
	
	// Draws the gamepad selection GUI elements that allow for selecting controllers
	void DrawGamepadSelectionGUI()
	{
		string [] joystickNames = Input.GetJoystickNames();
		
		List<string> controllerIndicies = new List<string>();
		for (int i = 0 ; i < joystickNames.Length ; i++)
		{
			controllerIndicies.Add(""+(i+1)+" - \""+joystickNames[i]+"\"");
		}
		
		if (DrawButton(0.7f, 0.00f, "Refresh", 3f))
		{
			GamepadInput.RefreshGamepads();
		}
		for (int i = 0 ; i < controllerIndicies.Count ; i++)
		{
			if (DrawButton(0.7f, 0.1f+0.05f*(i+1), controllerIndicies[i], 3f))
			{
				selectedControllerIndex = i;
			}
		}
	}
	
	// Draws the currently selected controller and it's inputs (if calibrated). This also draws the raw inputs on the left of the screen
	void DrawGamepadOutputGUI()
	{
		GamepadAxes selectedAxes = GamepadInput.GetAllAxes(selectedControllerIndex);
		
		string [] controllerNames = Input.GetJoystickNames();
			
		if(selectedAxes != null && selectedControllerIndex < controllerNames.Length)
		{
			DrawText(0.3f, 0.025f, 	"Selected: "+selectedControllerIndex+" - "+controllerNames[selectedControllerIndex]+"\n"+
									"Serialized: "+selectedAxes.Serialize());
		}
		else
		{
			DrawText(0.3f, 0.025f, 	"No controllers connected");
		}
		
		
		DrawUnitVectorBox(0.35f, 0.70f, GamepadInput.GetXYAxis(XYAxis.LeftAnalog), GamepadInput.GetButton(PadButton.LeftAnalog));
		DrawText(0.3f, 0.8f, "LS "+GamepadInput.GetXYAxis(XYAxis.LeftAnalog, selectedControllerIndex));
		DrawText(0.3f, 0.85f, ""+GamepadInput.GetAxis(Axis.LeftAnalogX, selectedControllerIndex));
		DrawText(0.3f, 0.9f, ""+GamepadInput.GetAxis(Axis.LeftAnalogY, selectedControllerIndex));

		
		DrawUnitVectorBox(0.75f, 0.70f, GamepadInput.GetXYAxis(XYAxis.RightAnalog), GamepadInput.GetButton(PadButton.RightAnalog));
		DrawText(0.7f, 0.80f, "RS "+GamepadInput.GetXYAxis(XYAxis.RightAnalog, selectedControllerIndex));
		DrawText(0.7f, 0.85f, ""+GamepadInput.GetAxis(Axis.RightAnalogX, selectedControllerIndex));
		DrawText(0.7f, 0.90f, ""+GamepadInput.GetAxis(Axis.RightAnalogY, selectedControllerIndex));
		
		
		DrawText(0.3f, 0.5f, "DP "+GamepadInput.GetXYAxis(XYAxis.DPad, selectedControllerIndex));
		DrawText(0.3f, 0.55f, ""+GamepadInput.GetAxis(Axis.DPadX, selectedControllerIndex));
		DrawText(0.3f, 0.6f, ""+GamepadInput.GetAxis(Axis.DPadY, selectedControllerIndex));
		
		
		DrawBox(0.35f, 0.4f, "U", GamepadInput.GetButton(PadButton.DPadUp, selectedControllerIndex));
		DrawBox(0.35f, 0.45f, "D", GamepadInput.GetButton(PadButton.DPadDown, selectedControllerIndex));
		DrawBox(0.325f, 0.425f, "L", GamepadInput.GetButton(PadButton.DPadLeft, selectedControllerIndex));
		DrawBox(0.375f, 0.425f, "R", GamepadInput.GetButton(PadButton.DPadRight, selectedControllerIndex));
		
		DrawUnitVectorBox(0.35f, 0.3f, GamepadInput.GetXYAxis(XYAxis.DPad, selectedControllerIndex));
		
		DrawBox(0.75f, 0.4f, "U",  GamepadInput.GetButton(PadButton.ActionUp, selectedControllerIndex));
		DrawBox(0.75f, 0.45f, "D", GamepadInput.GetButton(PadButton.ActionDown, selectedControllerIndex));
		DrawBox(0.7f, 0.425f, "L", GamepadInput.GetButton(PadButton.ActionLeft, selectedControllerIndex));
		DrawBox(0.8f, 0.425f, "R", GamepadInput.GetButton(PadButton.ActionRight, selectedControllerIndex));
		
		DrawUnitVectorBox(0.75f, 0.3f, GamepadInput.GetXYAxis(XYAxis.ActionButtons, selectedControllerIndex));
		
		DrawText(0.75f, 0.5f, "Action "+GamepadInput.GetXYAxis(XYAxis.ActionButtons, selectedControllerIndex));
		DrawText(0.75f, 0.55f, ""+GamepadInput.GetAxis(Axis.ActionButtonsX, selectedControllerIndex));
		DrawText(0.75f, 0.6f, ""+GamepadInput.GetAxis(Axis.ActionButtonsY, selectedControllerIndex));
		
		DrawBox(0.5f,  0.5f, "Select", GamepadInput.GetButton(PadButton.Select, selectedControllerIndex), 2f);
		DrawBox(0.55f, 0.5f, "Home", GamepadInput.GetButton(PadButton.Home, selectedControllerIndex),   2f);
		DrawBox(0.6f,  0.5f, "Start", GamepadInput.GetButton(PadButton.Start, selectedControllerIndex),   2f);
		
		
		DrawText(0.5f, 0.15f, "LT "+GamepadInput.GetAxis(Axis.LeftTrigger, selectedControllerIndex));
		DrawUnitFloatBox(0.5f,  0.2f, 1f-GamepadInput.GetAxis(Axis.LeftTrigger, selectedControllerIndex));
		
		DrawBox(0.5f,  0.3f, "LB", GamepadInput.GetButton(PadButton.LeftBumper, selectedControllerIndex), 1f);
		
		DrawText(0.6f, 0.15f, "RT "+GamepadInput.GetAxis(Axis.RightTrigger, selectedControllerIndex));
		DrawUnitFloatBox(0.6f,  0.2f, 1f-GamepadInput.GetAxis(Axis.RightTrigger, selectedControllerIndex));
		
		DrawBox(0.6f,  0.3f, "RB", GamepadInput.GetButton(PadButton.RightBumper, selectedControllerIndex), 1f);
		
//		if (selectedAxes == null) return;
		
		int inputs = GamepadInput.MaxAxes + GamepadInput.MaxButtons;
		for (int a = 1 ; a <= GamepadInput.MaxAxes ; a++)
		{
			
			Axis axis = selectedAxes != null ? selectedAxes.FindAxis(a) : (Axis)(-1);
			string axisString = axis == Axis.NullAxis ? "" : ""+axis;
			float axisValue = GamepadInput.GetRawAxis(a, selectedControllerIndex);
			bool axisAtExtreme = axisValue < -0.9 || axisValue > 0.9;
			DrawText(0.001f, (float)(a-1)/inputs, "Axis "+a+"\t"+ axisValue, axisAtExtreme);
			DrawText(0.15f, (float)(a-1)/inputs, axisString);
			
		}
		for (int b = 0 ; b < GamepadInput.MaxButtons ; b++)
		{
			PadButton button = selectedAxes != null ? selectedAxes.FindButton(b) : (PadButton)(-1);
			string buttonString = button == PadButton.NullButton ? "" : ""+button;
			bool buttonValue = GamepadInput.GetRawButton(b, selectedControllerIndex);
			DrawText(0.001f, (float)(GamepadInput.MaxAxes+b)/inputs, "Button "+b+"\t"+ buttonValue, buttonValue);
			DrawText(0.15f, (float)(GamepadInput.MaxAxes+b)/inputs, buttonString);
			
		}
	}
	
	#region Functinos to draw various GUI elements
	
	void DrawText(float percWidth, float percHeight, string text, bool highlight = false)
	{
		GUIStyle style = new GUIStyle();
		style.normal.textColor = highlight ? Color.red : Color.white ;
		GUI.Label(	new Rect(percWidth*Screen.width,percHeight*Screen.height,1000,1000), text, style);
	}
	
	
	bool DrawButton(float percWidth, float percHeight, string text, float aspect)
	{
		return GUI.Button( new Rect(percWidth*Screen.width,percHeight*Screen.height,Screen.width*0.1f*aspect,Screen.height*0.05f), text);
	}
	
	bool DrawToggle(float percWidth, float percHeight, bool val, string text, float aspect = 1f)
	{
		return GUI.Toggle( new Rect(percWidth*Screen.width,percHeight*Screen.height,Screen.width*0.1f*aspect,Screen.height*0.05f), val, text);
	}
	
	
	void DrawBox(float percWidth, float percHeight, string text, bool highlight = false, float aspect = 1f)
	{
		for (int i = 0 ; i < (highlight ? 2 : 1) ; i++)
		{
			float boxWidth = Screen.width*0.025f;
			float offset = Screen.width*0.005f*i;
			GUI.Box(new Rect(percWidth*Screen.width+offset/2,percHeight*Screen.height+offset/2, boxWidth*aspect-offset, boxWidth-offset), i == 0 ? text : "");
		}
	}
	
	void DrawUnitVectorBox(float percWidth, float percHeight, Vector2 vec, bool highlight = false)
	{
		DrawBox(percWidth+vec.x*0.05f, percHeight-vec.y*0.05f, "", highlight);
		
		DrawBox(percWidth+1*0.05f, percHeight+0*0.05f, "");
		DrawBox(percWidth-1*0.05f, percHeight+0*0.05f, "");
		DrawBox(percWidth+0*0.05f, percHeight+1*0.05f, "");
		DrawBox(percWidth+0*0.05f, percHeight-1*0.05f, "");
	}
	
	void DrawUnitFloatBox(float percWidth, float percHeight, float val, bool highlight = false)
	{
		DrawBox(percWidth, percHeight+val*0.05f, "", highlight);
		
		DrawBox(percWidth+0*0.05f, percHeight+0*0.05f, "");
		DrawBox(percWidth+0*0.05f, percHeight+1*0.05f, "");
	}
	
	#endregion
}
