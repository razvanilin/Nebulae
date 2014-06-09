using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private NetworkPlayer playerID;
	private string name = "";
	private int kills = 0;
	private int assists = 0;
	private int deaths = 0;
	private ArrayList playerList;

	// getters && setters
	public NetworkPlayer PlayerID{get{return playerID;} set{playerID = value;}}
	public string Name {get{return name;} set{name = value;}}
	public int Kills {get{return kills;} set{kills = value;}}
	public int Assists {get{return assists;} set{assists = value;}}
	public int Deaths {get{return deaths;} set{deaths = value;}}
}
