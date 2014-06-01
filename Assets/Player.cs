using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private string name = "Pilot";
	private int kills = 0;
	private int assists = 0;
	private int deaths = 0;


	void Start()
	{

	}

	public void SetName(NetworkPlayer netPlayer)
	{
		networkView.RPC ("UpdateName", netPlayer, name);
	}

	[RPC]
	void UpdateName(string newName)
	{
		if (networkView.isMine)
			this.name = newName;
	}

	// getters && setters
	public string Name {get{return name;} set{name = value;}}
	public int Kills {get{return kills;} set{kills = value;}}
	public int Assists {get{return assists;} set{assists = value;}}
	public int Deaths {get{return deaths;} set{deaths = value;}}
}
