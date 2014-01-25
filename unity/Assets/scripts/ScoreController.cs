using UnityEngine;
using System.Collections.Generic;

public class ScoreController : SingletonBehaviour<ScoreController> 
{
	public class PlayerInfo
	{
		public string name = "Unnnamed";
		public int score = 0;
	}

	public int winScore = 10;


	public PlayerInfo [] playerInfos;

	void Start()
	{
		playerInfos = new PlayerInfo[4];
		for (int i = 0 ; i < 4 ; i++)
		{
			playerInfos[i] = new PlayerInfo() {name = "Player "+(i+1).ToString()};
		}
		ResetScores();
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
		playerInfos[winner1.Index].score += 1;
		playerInfos[winner2.Index].score += 1;


		List<int> winningPlayers = new List<int>();
		
//		foreach (PlayerInfo info in playerInfos)
		for (int i = 0 ; i < 4 ; i++)
		{
			if (playerInfos[i].score >= winScore)
			{
				winningPlayers.Add(i);
			}
		}

		if (winningPlayers.Count > 0)
		{
			Debug.LogWarning ("Have winners!");

			// TODO display scores
			ResetScores();
		}
	}




}
