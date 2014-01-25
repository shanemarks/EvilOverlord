 
======================================================
Cross-platform, Cross-Controller Gamepad Input Manager 
Copyright © 2013 raxterworks
Open-Beta Version 0.1
http://raxterworks.blogspot.com/
raxterworks@gmail.com
======================================================

--------
Overview
--------

See GamepadInputManager.pdf for full details.

-----------------------------
Feature List / Basic Overview
-----------------------------

Simple, intuitive interface, that is device-independant
 - Input class-esque functions for all buttons, e.g:
   - GamepadInput.GetButtonUp(Button.LeftBumper)		Gets whether the left bumper has been released
   - GamepadInput.GetButtonDown(Button.DPadUp)			Whether the dpad's up direction was pressed
   - GamepadInput.GetButton(XBOXButton.X)				Whether the X button is held down (i.e. the lower action button)
   - GamepadInput.GetButton(PSButton.Circle, 2))		Whether Controller 2's Circle button is held down (i.e. the right action button)
   - GamepadInput.GetButton(Button.ActionLeft))			Whether the left action button (X for XBOX, Square for PS, U for OUYA)
   - GamepadInput.GetButtonDown(Button.Start))			Whether the start button was pressed
 - Input class-esque function for all axes e.g:
    - GamepadInput.GetAxis(Axis.LeftAnalogX) 			Gets the left analog stick's horizontal value between -1 and 1
    - GamepadInput.GetXYAxis(XYAxis.RightAnalog)  		Gets the right analog's XY values between (-1,-1) and (1,1)
    - GamepadInput.GetPolarAxis(XYAxis.LeftAnalog))		Gets the left analogs polar coordinates (angle, magnitude)
    - GamepadInput.GetXYAxis(XYAxis.DPad))				Gets the DPad as a Vector2
 - Negligable overhead
 - Support for up to 4 controllers

Automatic setup of axes and deadzones for calibrated controllers
 - Controllers effectively work the same, but all have differnt configurations in Unity's input settings. The GamePad Input Plugin abstracts that away to a homogonised input system
 - No need to mess around with axes for different controllers. Just plug in the gamepad and the Gamepad Input plugin does everything for you in the background!
 - No DLLs! All code is in Unity C# script and works on any platform
 - Come included with calibrations of all common controllers/drivers (XBOX for Win/Mac/Linux/Android[SOON]/iOS[SOON], PS3, OUYA[HOPEFULLY], MOGA[HOPEFULLY], Logitech/Generic)
 - Comes included with a helper classes to create a tool to configure custom controllers
    - Ability to add custom controllers to the game at build time (stored in a ScriptableObject)
    - Ability to add custom controllers to the game at runtime (stored in PlayerPrefs)

Simple setup of Input Settings Axes via window
 - No more manually setting up 20, 40, 60, 80+ Axes in the Input Settings, one click and it's done for you!
 - If you have Input Settings Axes you want to keep, the plugin can append to your current Input Settings [requires PRO version of Unity]
 
 
----------------
Planned features
---------------- 

 - Recieve callbacks in code when a controller is connected or disconencted.
 - In-game configurable, simple to use left-hand/southpaw configuration, e.g. GamepadInput.EnableLeftHandMode(bool leftHanded, int controllerIndex)
 - In-game configurable, simple to use invert-Y configurations for analog sticks, e.g. GamepadInput.InvertY(XYAxis toInvert, int controllerIndex)
 - In-game configurable, simple to use deadzone configurations (i.e. change deadzone in game rather than in controller configurations)
 - More deadzone options (Axis-Deadzone, Radial-Deadzone, Scaled-Radial-Deadzone)
 - Generic controller configuration helper tools to allow in-game controller configuration and storage in PlayerPrefs
 - Support for other game controllers such as flight joysticks, driving wheels, <insert your suggestion here>
 - Android, iOS, and OUYA support 
    - 	I'm just one developer with a full time job creating this plugin in his spare time. 
			I unfortunately don't have the funds right now to setup and test devices on Android, iOS and OUYA (I don't even have an OUYA, or even a Mac device :p), 
			but by showing your support and donating this plugin I hope to be able to support many more platforms in the not too distant future!
	- XInput support (if requested enough)


---------------------------------------
Support and documentation
---------------------------------------

See included OverviewAndSetup.pdf for setup instructions and troubleshooting

Any bugs, feature requests or suggestions, please leave a comment at TODO TODO TODO or email raxterworks@gmail.com

---------------
Version History 
---------------

0.1 (beta)

Initial working version!
 