using UnityEngine;
using System.Collections;

namespace RaxterWorks
{
namespace GamepadInputManager
{

public class GamepadInputBackgroundProcess : MonoBehaviour
{
	
	class ButtonStates
	{
		public bool triggerLeft  = false;
		public bool triggerRight = false;
		
		public bool dpadLeft  = false;
		public bool dpadRight = false;
		public bool dpadUp    = false;
		public bool dpadDown  = false;
		
		public void CopyFrom(ButtonStates states)
		{
			triggerLeft  = states.triggerLeft;
			triggerRight = states.triggerRight;
		
			dpadLeft  = states.dpadLeft;
			dpadRight = states.dpadRight;
			dpadUp    = states.dpadUp;
			dpadDown  = states.dpadDown;
		}
		
		public bool GetButton(PadButton button)
		{
			switch (button)
			{
			case PadButton.DPadDown:
				return dpadDown;
			case PadButton.DPadUp:
				return dpadUp;
			case PadButton.DPadLeft:
				return dpadLeft;
			case PadButton.DPadRight:
				return dpadRight;
			case PadButton.LeftTrigger:
				return triggerLeft;
			case PadButton.RightTrigger:
				return triggerRight;
			}
			return false;
		}
	}
	
	ButtonStates [] previousStates = new ButtonStates [4];
	ButtonStates [] states = new ButtonStates [4];
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0 ; i < 4 ; i++)
		{
			if (states[i] == null)
				states[i] = new ButtonStates();
			if (previousStates[i] == null)
				previousStates[i] = new ButtonStates();

			previousStates[i].CopyFrom(states[i]);
			
			GamepadInput.GetSimulatedDPadButtons(out states[i].dpadUp, out states[i].dpadDown, out states[i].dpadLeft, out states[i].dpadRight, i);
			states[i].triggerLeft  = GamepadInput.GetSimulatedLeftTriggerButton(i);
			states[i].triggerRight = GamepadInput.GetSimulatedRightTriggerButton(i);
		}
	}

	public bool GetButtonDown (PadButton button, int controllerIndex)
	{
		if (previousStates == null || states == null || previousStates[controllerIndex] == null || states[controllerIndex] == null)
		{
			return false;
		}
		return !previousStates[controllerIndex].GetButton(button) && states[controllerIndex].GetButton(button);
	}
	public bool GetButtonUp (PadButton button, int controllerIndex)
	{
		if (previousStates == null || states == null || previousStates[controllerIndex] == null || states[controllerIndex] == null)
		{
			return false;
		}
		return previousStates[controllerIndex].GetButton(button) && !states[controllerIndex].GetButton(button);
	}
	
	#region AutoSingletonBehaviour code
	
	protected static GamepadInputBackgroundProcess _instance = null;
    public static GamepadInputBackgroundProcess instance
    {
        get 
		{ 
			// if we we do not want to create a background instance, we do not autocreate it
			if (!GamepadInputConfigurations.CreateBackgroundProcessOnCall)
			{
				return null;
			}
			if ( _instance == null )
			{	
				GameObject newGo = new GameObject("_Gamepad Background Process");
				_instance = newGo.AddComponent<GamepadInputBackgroundProcess>();
			}
			return _instance; 
		}
    }
	
	public static bool hasInstance
    {
        get { return ( instance != null ); }
    }
   
    public virtual void Awake()
    {
        if ( _instance == null )
        {
            _instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
    }

    public virtual void OnDestroy()
    {
        if (_instance == this)
        {
            GameObject.Destroy(_instance.gameObject);
			_instance = null;			
        }
    }
	
	#endregion
}

}
}