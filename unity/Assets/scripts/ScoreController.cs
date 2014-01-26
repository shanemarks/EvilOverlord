using UnityEngine;
using System.Collections.Generic;

public class ScoreController : SingletonBehaviour<ScoreController> 
{
	public class PlayerInfo
	{
		public string name = "Unnnamed";
		public int score = 0;
	}

	public int winScore = 3;


	public PlayerInfo [] playerInfos;
	 bool[] isLiving = {true,true,true,true};

	void Start()
	{


		playerInfos = new PlayerInfo[4];
		for (int i = 0 ; i < 4 ; i++)
		{
		
			playerInfos[i] = new PlayerInfo() {name = "Player"+(i+1).ToString()};
			playerInfos[i].score = PlayerPrefs.GetInt( "Player"+(i+1).ToString());
		}

	}

	void ResetScores()
	{
		foreach (PlayerInfo info in playerInfos)
		{
			info.score = 0;
		}
	}


	public void UpdateGameFinished(Player winner1, Player winner2)
	{
		Debug.Log (winner1);
		Debug.Log (winner2);
		PlayerController.instance.RoundWon = true;
		if (winner1 !=  null)
		{
			playerInfos[winner1.Index].score += 1;
			PlayerPrefs.SetInt("Player" + (winner1.Index+1).ToString(),playerInfos[winner1.Index].score);
			PlayerPrefs.Save();


		}
		if (winner2 != null)
		{
			playerInfos[winner2.Index].score += 1;
			PlayerPrefs.SetInt("Player" + (winner2.Index+1).ToString(),playerInfos[winner2.Index].score);
			PlayerPrefs.Save();
		}
		if (winner1 == null && winner2==null)
		{
			Debug.Log ("None Wins");

			UIManager.instance.ScreenMessage.text = "No one wins this round";
		}
		else
		{	
			
			CheckWinners();
			UIManager.instance.ScreenMessage.text = winningPlayers.Count.ToString()+ " Round Winners: \n" + winner1.name  + ((winner2 != null) ?  " - " + winner2.name : "") ;


			if (HaveWinners)
			{
				string [] names = winningPlayers.ConvertAll((i) => PlayerController.instance.Players[i].name).ToArray();
				
				UIManager.instance.ScreenMessage.text += "\nGame Winners: "+ string.Join(", ", names);
			}
		}


	}


	void CheckWinners()
	{
		
		winningPlayers.Clear();
		
		//		foreach (PlayerInfo info in playerInfos)
		for (int i = 0 ; i < 4 ; i++)
		{
			if (playerInfos[i].score >= winScore)
			{
				winningPlayers.Add(i);
			}
		}
	}

	void OnGUI()
	{
		GUILayout.Label(winningPlayers.Count.ToString());
		for (int i = 0 ; i < 4 ; i++)
		{
			GUILayout.Label(playerInfos[i].score.ToString());
		}
	}

	public bool HaveWinners { get { return winningPlayers.Count > 0; } }
	
	List<int> winningPlayers = new List<int>();

	float CheckTimer = 1f;
	public float timer =0;
	public 	void LateUpdate ()
	{	
	
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F5))
		{
			playerInfos[2].score = winScore;
		}

#endif

		CheckWinners();


//		if (winningPlayers.Count > 0)
//		{
//			Debug.LogWarning ("Have winners!");
//			
//			
//			
//			Application.LoadLevel(0);
//		}
	

		int n = 0;
		int count = 0;
		if (PlayerController.instance != null)
		{

			for (int i = 0 ; i < 4 ; i++)
			{
				
				UIManager.instance.ScoreIcons[i].text =  playerInfos[i].score.ToString() ;
				
			}
			if (timer < CheckTimer)
			{
				timer +=Time.deltaTime;	

				return;
			}
		
			
		



			if (!PlayerController.instance.RoundWon)
			{
				isLiving[n] = true;
				foreach (Player pl in PlayerController.instance.Players)
				{
					isLiving[n] = true;
					if (!pl.IsAlive)
					{
						Debug.Log ("PlayerDEAD");
						isLiving[n] = false;
						n++;
						count++;
					}



				}
				if (count >= 2)
				{
				
					bool firstFound = false;
					Player p = null, p1 = null;
					for (int i=0; i<PlayerController.instance.PlayerCount; i++)
					{
						if(!firstFound)
						{
							 if (PlayerController.instance.Players[i].IsAlive)
							{
								p = PlayerController.instance.Players[i];
								firstFound = true;
							}
						}

						else
						{
							if (PlayerController.instance.Players[i].IsAlive)
							{
								p1 = PlayerController.instance.Players[i];
							}
						}

					}
					UpdateGameFinished (p,p1);
				
				}
			}
		}

		timer = 0;
	}



}
