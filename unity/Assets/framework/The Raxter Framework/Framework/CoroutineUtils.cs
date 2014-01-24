using UnityEngine;
using System.Collections;

public class CoroutineUtils : AutoSingletonBehaviour<CoroutineUtils> 
{
	
	public static Coroutine WaitForGameSecondAndDo(float time, System.Action action)
	{
		return instance.StartCoroutine(WaitForGameSecondAndDoCoroutine(time, action));
	}
	
	
	private static IEnumerator WaitForGameSecondAndDoCoroutine(float time, System.Action action)
	{
		yield return new WaitForSeconds(time);
		if (action != null)
		{
			action();
		}
	}
	public static Coroutine WaitOneFrameAndDo(System.Action action)
	{
		return instance.StartCoroutine(WaitOneFrameAndDoCoroutine(action));
	}
	
	
	private static IEnumerator WaitOneFrameAndDoCoroutine(System.Action action)
	{
		yield return null;
		if (action != null)
		{
			action();
		}
	}
}
