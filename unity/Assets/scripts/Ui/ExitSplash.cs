using UnityEngine;
using System.Collections;

public class ExitSplash : MonoBehaviour {

	void Start()
	{
		Screen.showCursor = false;
	}
	void Update()
	{
		for (int i = 0 ; i < Input.GetJoystickNames().Length ; i++)
		{
			if (RaxterWorks.GamepadInputManager.GamepadInput.GetButton(PadButton.Start, 0))
		    {
				ToGame();
			}	
		}
	}



void ToGame ()
	{
		Application.LoadLevel(1);
	}
}
