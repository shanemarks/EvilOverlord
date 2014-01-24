using UnityEngine;
using System.Collections;

public static class TweenTools 
{
	// TODO add tween chainer
	
	
	public static TweenColor FromTo(this TweenColor tweenColor, Color from, Color to)
	{
		tweenColor.from = from;
		tweenColor.to = to;
		return tweenColor;
	}
	
	public static TweenColor From(this TweenColor tweenColor, Color from)
	{
		tweenColor.from = from;
		return tweenColor;
	}
	
//	public static T OnFinish<T>(this T tweener, UITweener.OnFinished onFinished) where T : UITweener
//	{
//		tweener.onFinished = onFinished;
//		return tweener;
//	}
//	
//	public static T SetStyle<T>(this T tweener, UITweener.Style style) where T : UITweener
//	{
//		tweener.style = style;
//		return tweener;
//	}
//	
//	public static T AniCurve<T>(this T tweener, params Keyframe [] keyFrames) where T : UITweener
//	{
//		tweener.animationCurve = new AnimationCurve(keyFrames);
//		return tweener;
//	}
//	
//	public static T TimeScaled<T>(this T tweener) where T : UITweener
//	{
//		tweener.ignoreTimeScale = false;
//		return tweener;
//	}
}
