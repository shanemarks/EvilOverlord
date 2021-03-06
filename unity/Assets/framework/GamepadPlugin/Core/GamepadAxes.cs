using UnityEngine;
using System.Collections;

public enum XYAxis {
	LeftAnalog,
	RightAnalog,
	DPad,
	ActionButtons
};

public enum Axis {
	LeftAnalogX, LeftAnalogY,
	RightAnalogX, RightAnalogY,
	DPadX, DPadY,
	ActionButtonsX, ActionButtonsY,
	LeftTrigger, RightTrigger,
	
	NullAxis = -1
};

public enum PadButton {
	ActionDown, ActionUp, ActionLeft, ActionRight, 
	LeftBumper, RightBumper, 
	LeftAnalog, RightAnalog, 
	Start, Select, Home,
	DPadUp, DPadDown, DPadLeft, DPadRight,
	LeftTrigger, RightTrigger,
	
	NullButton = -1
};


public static class XBOXButton
{
	public static PadButton A = PadButton.ActionDown;
	public static PadButton B = PadButton.ActionRight;
	public static PadButton X = PadButton.ActionLeft;
	public static PadButton Y = PadButton.ActionUp;
}

public static class PSButton
{
	public static PadButton Cross    = PadButton.ActionDown;
	public static PadButton Circle   = PadButton.ActionRight;
	public static PadButton Square   = PadButton.ActionLeft;
	public static PadButton Triangle = PadButton.ActionUp;
}

[System.Serializable]
public class GamepadAxes
{
	
	public GamepadAxes()
	{
	}
	
	public GamepadAxes(GamepadAxes other)
	{
		// copy names
		_names = new string [other._names.Length];
		System.Array.Copy(other._names, _names, _names.Length);
		
		// copy axes
		string serialized = other.Serialize();
		Deserialize(serialized);
	}
	
	public const int NoInput = -1;
	public const int Simulate = -2;
	public const int Undefined = -3;
	
	[SerializeField]
	string [] _names = new string [0];
	
	public string [] names 
	{
		get 
		{
			string [] ret = new string [_names.Length];
			System.Array.Copy (_names, ret, _names.Length);
			return ret;
		}
		set
		{
			System.Array.Resize(ref _names, value.Length);
			System.Array.Copy (value, _names, value.Length);
		}
	}
	public void AddName(string name)
	{
		if (ContainsName(name))
		{
			return;
		}
		System.Array.Resize(ref _names, _names.Length+1);
		_names[_names.Length-1] = name;
	}
	
	public bool ContainsName(string name)
	{
		return System.Array.FindIndex(_names, (obj) => obj == name) != -1;
	}
	public bool ContainsANameFrom(string [] otherNames)
	{
		foreach(string otherName in otherNames)
		{
			if (ContainsName(otherName))
				return true;
		}
		
		return false;
	}
	
	public bool SameConfigAs(GamepadAxes axes)	
	{
		return this.Serialize() == axes.Serialize();
	}
	
	[System.Serializable]
	public class AxisDesc
	{
		public AxisDesc(int axisCode, float minAxisExtent, float maxAxisExtent, bool inv = false)
		{
			code = axisCode;
			minExtent = minAxisExtent;
			maxExtent = maxAxisExtent;
//			invert = inv;
		}
		
		public AxisDesc(AxisDesc ac)
		{
			code = ac.code;
			minExtent = ac.minExtent;
			maxExtent = ac.maxExtent;
//			invert = ac.invert;
		}
		
		public int code;
		public float maxExtent;
		public float minExtent;
//		public bool invert;
		
//		public static bool operator !=(AxisDesc a1, AxisDesc a2) 
//		{
//			return !Equals(a1, a2);
//		}
//		public static bool operator ==(AxisDesc a1, AxisDesc a2) 
//		{
//			return Equals(a1, a2);
//		}
		
		public override bool Equals(System.Object obj)
		{
				
	        // If parameter is null return false.
	        if (obj == null)
	        {
	            return false;
	        }
	
	        // If parameter cannot be cast to Point return false.
	        AxisDesc a = obj as AxisDesc;
	        if ((System.Object)a == null)
	        {
	            return false;
	        }
	
	        // Return true if the fields match:
			return code == a.code && maxExtent == a.maxExtent && minExtent == a.minExtent;// && a1.invert == a2.invert;
		}
		
		
		public override int GetHashCode()
		{
		    return code ^ maxExtent.GetHashCode() ^ minExtent.GetHashCode();
		}
	}
	
	public static AxisDesc UndefinedAxis  
	{ 
		get 
		{ 
			return new AxisDesc(Undefined,0f,0f); 
		} 
	}
	
//	public bool useSplitTriggerAxisSetup = false;
	
	public float leftAnalogStickDeadZone = 0f;
	public AxisDesc leftAnalogX = UndefinedAxis;
	public AxisDesc leftAnalogY = UndefinedAxis;
	public float rightAnalogStickDeadZone = 0f;
	public AxisDesc rightAnalogX = UndefinedAxis;
	public AxisDesc rightAnalogY = UndefinedAxis;
	
//	public bool invertLeftAnalogY = true;
//	public bool invertRightAnalogY = true;
//	public bool invertDPadY = false;
	
	public AxisDesc leftTriggerAxis = UndefinedAxis;
	public AxisDesc rightTriggerAxis = UndefinedAxis;
	public int leftTriggerButton = Undefined;
	public int rightTriggerButton = Undefined;
	
	public int leftBumper = Undefined;
	public int rightBumper = Undefined;
	
	public int leftAnalogButton = Undefined;
	public int rightAnalogButton = Undefined;
	
	public AxisDesc dPadAxisX 	= UndefinedAxis;
	public AxisDesc dPadAxisY 	= UndefinedAxis;
	
	public int dPadUp 		= Undefined;
	public int dPadDown 	= Undefined;
	public int dPadLeft 	= Undefined; 
	public int dPadRight 	= Undefined;
	
	public AxisDesc actionButtonsAxisX 	= UndefinedAxis;
	public AxisDesc actionButtonsAxisY 	= UndefinedAxis;
	
	public int actionDown = Undefined;
	public int actionUp = Undefined;
	public int actionLeft = Undefined;
	public int actionRight = Undefined;
	
	public int startButton = Undefined;
	public int selectButton = Undefined;
	public int homeButton = Undefined;
	
//	public static void SerializeIntTest()
//	{
//		for (int i = 0 ; i < 62 ; i++)
//		{
//			Debug.Log(i+" -> "+ToCode(i) +" -> "+ToInt(ToCode(i)));
//		}
//	}
//	public static void SerializeFloatTest()
//	{
//		for (float f = -1f ; f <= 1f ; f += 0.04f)
//		{
//			Debug.Log(f+" -> "+ToCode(f) +" -> "+ToFloat(ToCode(f)));
//		}
//	}
	static char ToCode(float f)
	{
		
		f = Mathf.Clamp(f, -1f, +1f);
//		Debug.Log (""+f+" ToCode int = "+(int)((f + 1f)*25f));
		float f2 = (f + 1f)*25f;
		int i = (int)(f2);
		char c = ToCode(i);// range between [-1f;+1f] -> [0f;+2f] -> [0;50]
		return c;
	}
	static char ToCode(int i)
	{
		if (i == NoInput)
			return '-';
		else if (i == Simulate)
			return '+';
		else if (i == Undefined)
			return '!';
		else if (i < 10)
		{
			return (char)('0'+i);
		}
		else if (i < 10+26)
		{
			return (char)('a'+i-10);
		}
		else if (i < 10+26+26)
		{
			return (char)('A'+i-10-26);
		}
		else
		{
			return '_';
		}
	}
	static float ToFloat(char c)
	{
//		Debug.Log (""+c+" ToFloat int = "+ToInt (c));
		int i = ToInt(c);
		float f = (((float)i)/25f) - 1f;
		return f;
	}
	static int ToInt(char c)
	{
		if (c == '-')
			return NoInput;
		else if (c == '+')
			return Simulate;
		else if (c == '!')
			return Undefined;
		else if (c >= '0' && c <= '9')
		{
			return c-'0';
		}
		else if (c >= 'a' && c <= 'z')
		{
			return 10+(c-'a');
		}
		else if (c >= 'A' && c <= 'Z')
		{
			return 10+26+(c-'A');
		}
		return -1;
	}
	public string Serialize()
	{
		string s = "";
		foreach (Axis a in System.Enum.GetValues(typeof(Axis)))
		{
			AxisDesc ad = this[a];
			s += ToCode(ad.code);
			s += ToCode(ad.minExtent);
			s += ToCode(ad.maxExtent);
		}
		foreach (PadButton b in System.Enum.GetValues(typeof(PadButton)))
		{
			s += ToCode(this[b]);
		}
		return s;
	}
	public void Deserialize(string s)
	{
		int i = 0;
		foreach (Axis a in System.Enum.GetValues(typeof(Axis)))
		{
			AxisDesc ad = UndefinedAxis;
			ad.code = ToInt(s[i]);
			i++;
			ad.minExtent = ToFloat(s[i]);
			i++;
			ad.maxExtent = ToFloat(s[i]);
			i++;
			this[a] = ad;
		}
		foreach (PadButton b in System.Enum.GetValues(typeof(PadButton)))
		{
			this[b] = ToInt(s[i]);
			i++;
		}
	}
	public Axis FindAxis(int i)
	{
		foreach (Axis a in System.Enum.GetValues(typeof(Axis)))
		{
			if (this[a].code == i)
				return a;
		}
		return Axis.NullAxis;
	}
	public PadButton FindButton(int i)
	{
		foreach (PadButton b in System.Enum.GetValues(typeof(PadButton)))
		{
			if (this[b] == i)
				return b;
		}
		
		return PadButton.NullButton;
	}

	public float DeadZone(XYAxis axis)
	{
		switch(axis)
		{
		case XYAxis.LeftAnalog:
			return leftAnalogStickDeadZone;
			
		case XYAxis.RightAnalog:
			return rightAnalogStickDeadZone;
		}
		return 0;
	}
	
	
	
	public AxisDesc this [Axis axis]
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
				return leftTriggerAxis;
				
			case Axis.RightTrigger:
				return rightTriggerAxis;
				
			case Axis.DPadX:
				return dPadAxisX;
				
			case Axis.DPadY:
				return dPadAxisY;
				
			case Axis.ActionButtonsX:
				return actionButtonsAxisX;
				
			case Axis.ActionButtonsY:
				return actionButtonsAxisY;
			}
			return UndefinedAxis;
		}
		set
		{
//			AxisDesc oldVal = this [axis];
			switch(axis)
			{
			case Axis.LeftAnalogX:
				leftAnalogX = value;
				break;
				
			case Axis.LeftAnalogY:
				leftAnalogY = value;
				break;
				
			case Axis.RightAnalogX:
				rightAnalogX = value;
				break;
				
			case Axis.RightAnalogY:
				rightAnalogY = value;
				break;
				
			case Axis.LeftTrigger:
				leftTriggerAxis = value;
				break;
				
			case Axis.RightTrigger:
				rightTriggerAxis = value;
				break;
				
			case Axis.DPadX:
				dPadAxisX = value;
				break;
				
			case Axis.DPadY:
				dPadAxisY = value;
				break;
				
			case Axis.ActionButtonsX:
				actionButtonsAxisX = value;
				break;
				
			case Axis.ActionButtonsY:
				actionButtonsAxisY = value;
				break;
			}
			
//			if(this[axis] != oldVal)
//			{
//				GamepadInputConfigurations.UpdateCustomControllerSupport(this);
//			}
		}
	}
	public int this [PadButton button]
	{
		get
		{
			switch(button)
			{
			case PadButton.LeftAnalog:
				return leftAnalogButton;
				
			case PadButton.RightAnalog:
				return rightAnalogButton;
				
			case PadButton.LeftBumper:
				return leftBumper;
				
			case PadButton.RightBumper:
				return rightBumper;
				
			case PadButton.LeftTrigger:
				return leftTriggerButton;
				
			case PadButton.RightTrigger:
				return rightTriggerButton;
				
			case PadButton.Start:
				return startButton;
				
			case PadButton.Select:
				return selectButton;
				
			case PadButton.Home:
				return homeButton;
				
			case PadButton.ActionDown:
				return actionDown;
				
			case PadButton.ActionUp:
				return actionUp;
				
			case PadButton.ActionLeft:
				return actionLeft;
				
			case PadButton.ActionRight:
				return actionRight;
				
			case PadButton.DPadUp:
				return dPadUp;
				
			case PadButton.DPadDown:
				return dPadDown;
				
			case PadButton.DPadLeft:
				return dPadLeft;
				
			case PadButton.DPadRight:
				return dPadRight;
				
			}
			return -1;
		}
		set
		{
//			int oldVal = this[button];
			switch(button)
			{
			case PadButton.LeftAnalog:
				leftAnalogButton = value;
				break;
				
			case PadButton.RightAnalog:
				rightAnalogButton = value;
				break;
				
			case PadButton.LeftBumper:
				leftBumper = value;
				break;
				
			case PadButton.RightBumper:
				rightBumper = value;
				break;
				
			case PadButton.Start:
				startButton = value;
				break;
				
			case PadButton.Select:
				selectButton = value;
				break;
				
			case PadButton.Home:
				homeButton = value;
				break;
				
			case PadButton.ActionDown:
				actionDown = value;
				break;
				
			case PadButton.ActionUp:
				actionUp = value;
				break;
				
			case PadButton.ActionLeft:
				actionLeft = value;
				break;
				
			case PadButton.ActionRight:
				actionRight = value;
				break;
				
			case PadButton.DPadUp:
				dPadUp = value;
				break;
				
			case PadButton.DPadDown:
				dPadDown = value;
				break;
				
			case PadButton.DPadLeft:
				dPadLeft = value;
				break;
				
			case PadButton.DPadRight:
				dPadRight = value;
				break;
				
			}
			
			
//			if(this[button] != oldVal)
//			{
//				GamepadInputConfigurations.UpdateCustomControllerSupport(this);
//			}
		}
	}
	
	
}