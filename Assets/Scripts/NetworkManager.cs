using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject asteroidPrefab;
	public GameObject introObject;
	public AudioClip playerConnectedClip;
	public AudioClip playerDisconnectedClip;
	public string playerName = "Pilot";
	public GUISkin guiSkin;
	public Player playerObj;

	private const string typeName = "Nebulae_V0.0.1_TestServer";
	private const string gameName = "Nebulae Space";
	private HostData[] hostList;
	private SceneFadeInOut sceneFadeIn;
	private float playerTimeDisplay = 5f;
	private float timePassed = 0f;
	private State gameState;
	private List<Player> playerList = new List<Player>();

	void Start()
	{
		MasterServer.ipAddress = "188.226.229.203";
		gameState = State.GetInstance();
		sceneFadeIn = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
	}

	void OnServerInitialized()
	{
		sceneFadeIn.EndScene();
		Debug.Log("Server Initializied");
		//Network.Instantiate(asteroidPrefab, new Vector3(0f, 5f, 10f), Quaternion.identity, 0);
		//StartCoroutine(CRoutine());
		SpawnPlayer();
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnConnectedToServer()
	{
		sceneFadeIn.EndScene();
		Debug.Log("Server Joined");
		SpawnPlayer();
		//StartCoroutine(CRoutine());
	}

	void OnPlayerConnected(NetworkPlayer playerConnection)
	{
		AudioSource.PlayClipAtPoint(playerConnectedClip, Vector3.zero, 1f);
		AddPlayerToList();

		foreach(Player P in playerList)
		{
			P.SetName(playerConnection);
		}
	}

	void OnPlayerDisconnected(NetworkPlayer playerConnection)
	{
		AudioSource.PlayClipAtPoint(playerDisconnectedClip, Vector3.zero, 1f);
		Network.RemoveRPCs(playerConnection);
		Network.DestroyPlayerObjects(playerConnection);
	}

	void Update()
	{

	}

	void OnGUI()
	{
		GUI.skin = guiSkin;
		if (!Network.isClient && !Network.isServer)
		{
			playerName = GUI.TextField(new Rect(100, 50, 300, 25), playerName, 25);
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				StartServer();
			}
			if (GUI.Button(new Rect(100, 200, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName + "_" + i))
						JoinServer(hostList[i]);
				}
			}
		}

		if (Input.GetButton("Score Window"))
		{
			for (int i=0; i<playerList.Count; i++)
			{
				GUI.Box(new Rect(250, 100 + (110*i), 300, 50), playerList[i].Name);
			}
		}
	}

	void AddPlayerToList()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i<players.Length; i++)
		{
			for (int j = 0; j<playerList.Count; j++)
			{
				if (players[i].GetComponent<Player>() != playerList[j])
					playerList.Add(players[i].GetComponent<Player>());
			}
		}
	}

	private void StartServer()
	{
		Random rnd = new Random();
		int port = Random.Range(20000, 25000);
		Debug.Log(port);
		Network.InitializeServer(4, port, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	IEnumerator CRoutine()
	{
		float timetowait = 1;
		yield return new WaitForSeconds(timetowait);
		SpawnPlayer();
		//sceneFadeIn.StartScreen();
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void SpawnPlayer()
	{
		gameState.GState = State.GameState.PLAY;
		Network.Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
		Destroy(introObject);
	}
}
