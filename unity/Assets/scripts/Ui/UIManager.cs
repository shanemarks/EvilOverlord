using UnityEngine;
using System.Collections;

public class UIManager : SingletonBehaviour<UIManager> {

	public UIPanel PlayerPanel;


	void AnswerPhone ()
	{
		Debug.Log ("Answer Phone");
		GameController.instance.PlayInstructions();
	}

	void  StartGame ()
	{
		Debug.Log ("Start Game");
	}
}
