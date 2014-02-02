using UnityEngine;
using System.Collections;
using RaxterWorks.GamepadInputManager;
public class PassPhonePanel : MonoBehaviour 
{
	[SerializeField] UISprite[] _playerIcons;


	KeyCode[] keys = {KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4};

	void Update ()
	{
		int i = 0;
		foreach (PlayerIcon p in PlayerIcon.First(typeof(PlayerIcon)))
		{

			_playerIcons[i].spriteName = p.Icon.spriteName;


			if (Input.GetKeyUp(keys[i]))
			{
				if (PlayerController.instance.PlayerWithPhone != PlayerController.instance.Players[i] && PlayerController.instance.Players[i].IsAlive)
				{
					PlayerController.instance.PlayerWithPhone.PassPhone(PlayerController.instance.Players[i]);
			
					UIManager.instance.AnswerPhone();
					gameObject.SetActive(false);
				}
			}

			i++;

		}

	
	
	}


}
