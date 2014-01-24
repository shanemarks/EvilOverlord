using UnityEngine;
using System.Collections;

public static class ColorTools
{	
	public static Color WithAlpha(this Color c, float alpha)
	{
		return new Color(c.r, c.g, c.b, alpha); 
	}
	
	public static Color MakeVibrant(this Color c)
	{
		float maxVal = Mathf.Max(c.r, c.g, c.b);
		return new Color(c.r/maxVal, c.g/maxVal, c.b/maxVal, c.a); 
	}
	
	public static Color Blend(params Color [] colours)
	{
		Color ret = new Color();
		foreach(Color c in colours) ret += c;
		ret /= colours.Length;
		return ret;
	}
	
	public static Color RandomColor
	{
		get 
		{
			return new Color(Random.value, Random.value, Random.value);
		}
	}
	public static Color RandomVibrantColor
	{
		get 
		{
			return RandomColor.MakeVibrant(); // make sure that not all three colours are high value. if they are choose one and half its value
		}
	}
}
