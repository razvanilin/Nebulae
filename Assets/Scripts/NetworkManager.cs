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

	private const string typeName = "Nebulae_V0.0.1_TestServer";
	private const string gameName = "Nebulae Space";
	private HostData[] hostList;
	private SceneFadeInOut sceneFadeIn;
	private Dictionary<NetworkPlayer, string> playerList = new Dictionary<NetworkPlayer, string>();
	private float playerTimeDisplay = 5f;
	private float timePassed = 0f;

	void Start()
	{
		sceneFadeIn = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
	}

	void OnServerInitialized()
	{
		sceneFadeIn.EndScene();
		Debug.Log("Server Initializied");
		Network.Instantiate(asteroidPrefab, new Vector3(0f, 5f, 10f), Quaternion.identity, 0);
		StartCoroutine(CRoutine());
		//SpawnPlayer();
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
		StartCoroutine(CRoutine());
	}

	void OnPlayerConnected(NetworkPlayer playerConnection)
	{
		playerList.Add(playerConnection, playerName);
		AudioSource.PlayClipAtPoint(playerConnectedClip, Vector3.zero, 1f);
	}

	void OnPlayerDisconnected(NetworkPlayer playerConnection)
	{
		playerList.Remove(playerConnection);
		AudioSource.PlayClipAtPoint(playerDisconnectedClip, Vector3.zero, 1f);
		Network.RemoveRPCs(playerConnection);
		Network.DestroyPlayerObjects(playerConnection);
	}

	void Update()
	{

	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			playerName = GUI.TextField(new Rect(100, 400, 300, 25), playerName, 25);
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				StartServer();
			}
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
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
			if (playerList != null)
			{
				int i = 0;
				foreach(NetworkPlayer pilot in playerList.Keys)
				{
					Debug.Log(playerList[pilot]);
					GUI.Box(new Rect(250, 100 + (110*i), 300, 50), playerList[pilot]);
					i++;
				}
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
		Network.Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
		Destroy(introObject);
	}
}
