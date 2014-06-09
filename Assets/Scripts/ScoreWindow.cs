using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreWindow : MonoBehaviour {

	public float width = 700f;
	public float height = 400f;
	
	private List<Player> playerList = new List<Player>();
	private Rect scoreWindow;

	void Start()
	{
		scoreWindow = new Rect(Screen.width/2 - width/2, Screen.height - height+5, width, height);
	}

	void OnGUI()
	{
		if (Input.GetButton("Score Window"))
		{
			scoreWindow = GUI.Window(6, scoreWindow, DisplayScoreWindow, "");
		}
	}

	void DisplayScoreWindow(int id)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(1);
		GUILayout.EndVertical();

		foreach(Player player in playerList)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(player.Name + "\t\t | Kills: " + player.Kills + "\t\t | Deaths: " + player.Deaths);
			GUILayout.EndHorizontal();
			GUILayout.Space(2);
		}
	}

	public void AddPlayer(Player player)
	{
		playerList.Add(player);
	}
	
	public void RemovePlayer(NetworkPlayer player)
	{
		for (int i = 0; i<playerList.Count; i++)
		{
			if (player.guid == playerList[i].PlayerID.guid)
			{
				playerList.RemoveAt(i);
				break;
			}
		}
	}

	public void AddKill(string player)
	{
		string id = (networkView.isMine) ? player : Network.player.guid;
		List<Player> tempList = playerList;
		for (int i=0; i<tempList.Count; i++)
		{
			if (id == tempList[i].PlayerID.guid)
			{
				tempList[i].Kills = tempList[i].Kills + 1;
				break;
			}
		}
		playerList = tempList;
	}

	public void AddPoint(NetworkPlayer player)
	{
		List<Player> tempList = playerList;
		for (int i=0; i<tempList.Count; i++)
		{
			if (player == tempList[i].PlayerID)
			{
				tempList[i].Deaths = tempList[i].Deaths + 1;
				break;
			}
		}
		
		playerList = tempList;
	}

	public List<Player> PlayerList
	{
		get{return playerList;}
		set{playerList = value;}
	}
}
