using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreWindow : MonoBehaviour {

	public float width = 700f;
	public float height = 400f;

	//private ArrayList playerList = new ArrayList();
	List<Player> playerList = new List<Player>();
	private Rect scoreWindow;

	void Start()
	{
		scoreWindow = new Rect(Screen.width/2 - width/2, Screen.height - height+5, width, height);
	}

	void OnGUI()
	{
		if (Input.GetButton("Score Window"))
		{
			/*for(int i=0; i<playerList.Count; i++)
			{
				Player tempPlayer = playerList[i] as Player;
				GUI.Box(new Rect(250, 100 + (110*i), 300, 50), tempPlayer.Name);
			}*/

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
			GUILayout.Label(player.Name + "\t | Kills: " + player.Kills + "\t | Deaths: " + player.Deaths);
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
			Player tempPlayer = playerList[i] as Player;
			if (player == tempPlayer.PlayerID)
			{
				playerList.RemoveAt(i);
				return;
			}
		}
		//playerList.Remove(player);
	}

	public void AddPoint(NetworkPlayer player)
	{
		Debug.Log ("Add point accessed");
		networkView.RPC("Death", RPCMode.AllBuffered, player);
	}

	[RPC]
	void Death(NetworkPlayer player)
	{
		for (int i=0; i<playerList.Count; i++)
		{
			Debug.Log("here!" + i);
			if (player == playerList[i].PlayerID)
			{
				Debug.Log(playerList[i].Deaths);
				playerList[i].Deaths = playerList[i].Deaths + 1;
				Debug.Log(playerList[i].Deaths);
				break;
			}
		}
	}
}
