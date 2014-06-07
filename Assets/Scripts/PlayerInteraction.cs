using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour 
{
	public GUISkin guiSkin;
	public float chatX;
	public float chatY;
	public int width = 400;
	public int height = 200;
	public ScoreWindow scoreWindow;

	private bool usingChat = false;
	private bool showChat = false;

	string inputField = "";

	Vector2 scrollPosition;
	public string playerName;
	float lastUnfocusTime = 0;
	Rect window;

	ArrayList playerList = new ArrayList();

	class PlayerNode
	{
		public string playerName;
		public NetworkPlayer player;
	}

	ArrayList chatEntries = new ArrayList();
	class ChatEntry
	{
		public string name = "";
		public string text = "";
	}
	// Use this for initialization
	void Start () 
	{
		window = new Rect(10, Screen.height - height+5, width, height);
		playerName = PlayerPrefs.GetString("playerName", "");

		if (playerName == "") playerName = "Pilot_Name_" + Random.Range(1, 999);
	}

	void OnConnectedToServer()
	{
		ShowChatWindow();
		networkView.RPC("TellServerOurName", RPCMode.Server, playerName);
		networkView.RPC ("AddPlayerToList", RPCMode.AllBuffered, playerName);
		//AddGameChatMessage(playerName + " has just joined the nebula!");
	}

	void OnServerInitialized()
	{
		ShowChatWindow();
		/*PlayerNode newEntry = new PlayerNode();
		newEntry.playerName = playerName;
		newEntry.player = Network.player;
		playerList.Add(newEntry);*/
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, playerName);
		AddGameChatMessage(playerName + " has just joined the nebula!");
	}

	void OnPlayerDisconnected(NetworkPlayer netPlayer)
	{
		AddGameChatMessage("A player has disconnected from the nebula");
		scoreWindow.RemovePlayer(netPlayer);
		//playerList.Remove(netPlayer);
	}

	void OnDisconnectedFromServer()
	{
		CloseChatWindow();
	}

	[RPC]
	void AddPlayerToList(string name)
	{
		Player newEntry = new Player();
		newEntry.Name = name;
		newEntry.PlayerID = Network.player;
		scoreWindow.AddPlayer(newEntry);
	}

	[RPC]
	void TellServerOurName(string name, NetworkMessageInfo info)
	{
		PlayerNode newEntry = new PlayerNode();
		newEntry.playerName = name;
		newEntry.player = Network.player;
		playerList.Add(newEntry);
		AddGameChatMessage(name + " has just joined the nebula!");
	}

	void CloseChatWindow()
	{
		showChat = false;
		inputField = "";
		chatEntries = new ArrayList();
	}

	void ShowChatWindow()
	{
		showChat = true;
		inputField = "";
		chatEntries = new ArrayList();
	}
	
	void OnGUI()
	{
		GUI.skin = guiSkin;
		if (State.GetInstance().GState == State.GameState.MENU)
		{
			playerName = GUI.TextField(new Rect(100, 150, 300, 25), playerName, 25);
		}

		if (!showChat) return;

		if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
		{
			if (lastUnfocusTime + .25f < Time.time)
			{
				usingChat = true;
				GUI.FocusWindow(5);
				GUI.FocusControl("Chat input field");
			}
		}

		/*if (Input.GetButton("Score Window"))
		{
			for (int i=0; i<playerList.Count; i++)
			{
				PlayerNode playerNode = playerList[i] as PlayerNode;
				GUI.Box(new Rect(250, 100 + (110*i), 300, 50), playerNode.playerName);
			}
		}*/

		window = GUI.Window(5, window, GlobalChatWindow, "");
	}

	void GlobalChatWindow(int id)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(1);
		GUILayout.EndVertical();

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		foreach(ChatEntry entry in chatEntries)
		{
			GUILayout.BeginHorizontal();
			if (entry.name == " - ")
			{
				GUILayout.Label(entry.name + entry.text);
			}
			else 
			{
				GUILayout.Label(entry.name + ": " + entry.text);
			}

			GUILayout.EndHorizontal();
			GUILayout.Space(2);
		}

		GUILayout.EndScrollView();

		if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length > 0)
		{
			HitEnter(inputField);
		}
		else if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length <= 0 && usingChat)
		{
			usingChat = false;
			GUI.UnfocusWindow();
			lastUnfocusTime = Time.time;
		}

		GUI.SetNextControlName("Chat input field");
		inputField = GUILayout.TextField(inputField);

		if (Input.GetKeyDown("mouse 0"))
		{
			if (usingChat)
			{
				usingChat = false;
				GUI.UnfocusWindow();
				lastUnfocusTime = Time.time;
			}
		}
	}

	void HitEnter(string msg)
	{
		msg = msg.Replace('\n', ' ');
		networkView.RPC ("ApplyGlobalChatText", RPCMode.All, playerName, msg);
	}

	[RPC]
	void ApplyGlobalChatText(string name, string msg)
	{
		ChatEntry entry = new ChatEntry();
		entry.name = name;
		entry.text = msg;

		chatEntries.Add(entry);

		if (chatEntries.Count > 20)
			chatEntries.RemoveAt(0);

		scrollPosition.y = 1000000;
		inputField = "";
	}

	void AddGameChatMessage(string str)
	{
		ApplyGlobalChatText(" - ", str);
		if (Network.connections.Length > 0)
			networkView.RPC ("ApplyGlobalChatText", RPCMode.Others, " - ", str);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
