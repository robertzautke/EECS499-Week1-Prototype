using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Functions : MonoBehaviour {

    public GameObject canvas_envelope;
	public GameObject wait_canvas;
	private GameObject instantiated_wait_canvas;
    public InputField inputField;
    public Button clientConnect;
    public Button startServer;

    public bool serverAlreadyStarted = false;
    public bool alreadyConnected = false;

    private int numberOfPlayers = 0;
	public int numberOfPlayersNeeded = 2;

    public void startServerButtonClick() {
        if (!serverAlreadyStarted) {
            Network.incomingPassword = "password";
            bool useNat = !Network.HavePublicAddress();
            Network.InitializeServer(32, 25000, useNat);
        }    
    }

    public void connectToServerButtonClick() {
        if (!alreadyConnected) {

            ConnectToServer();
        }
    } 

    void ConnectToServer()
    {
        Network.Connect(inputField.text.ToString(), 25000, "password");
    }

////Generic events and debug output for server/client status////

    void OnServerInitialized()
    {
        Debug.Log("Server initialized and ready");

        serverAlreadyStarted = true;
        numberOfPlayers++;
		print(numberOfPlayers);
        Destroy(canvas_envelope);
		instantiated_wait_canvas = (GameObject)GameObject.Instantiate(wait_canvas);

    }


    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
        alreadyConnected = true;
		Destroy(canvas_envelope);
    }


    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isServer)
            Debug.Log("Local server connection disconnected");
        else
            if (info == NetworkDisconnection.LostConnection)
                Debug.Log("Lost connection to the server");
            else
                Debug.Log("Successfully diconnected from the server");
    }


    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
        alreadyConnected = false;
    }


    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("New object instantiated by " + info.sender);

		Debug.Log(networkView.viewID + " spawned");
		if (Network.isServer)
		{
			Network.RemoveRPCs(networkView.viewID);
			Network.Destroy(gameObject);
		}
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
        numberOfPlayers++;
        Debug.Log("Player " + numberOfPlayers + " connected from " + player.ipAddress + ":" + player.port);
		print(numberOfPlayers);

        if (numberOfPlayers == numberOfPlayersNeeded) {
			Destroy(instantiated_wait_canvas);
			Network.Instantiate(back, new Vector3(0, 0, 0), transform.rotation, 0);
			networkView.RPC("networkSignal_StartGame", RPCMode.All);
        }

    }


    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        numberOfPlayers--;
    }

/////////////////////////////////////////////////////////////////////////////////

////Send Serialized Network Data and RPC functions///////////////////////////////

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {

		int s = 0;
		if (stream.isWriting) { 
			s = 1;
			stream.Serialize(ref s);
		}
		else if (stream.isReading) { 
			stream.Serialize(ref s);

			Debug.Log(s);
		}
	
	}

	[RPC]
	void networkSignal_StartGame() { 
		StartGame();
	}

	[RPC]
	public void networkSignal_GameWinConditionCheck(int i, int playerNum)
	{

		bs[i - 1].taken = true;
		bs[i - 1].player = playerNum;

		if (bs[0].taken && bs[0].player == 0 && bs[1].taken && bs[1].player == 0 && bs[2].taken && bs[2].player == 0 ||
			bs[3].taken && bs[3].player == 0 && bs[4].taken && bs[4].player == 0 && bs[5].taken && bs[5].player == 0 ||
			bs[6].taken && bs[6].player == 0 && bs[7].taken && bs[7].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[0].taken && bs[0].player == 0 && bs[3].taken && bs[3].player == 0 && bs[6].taken && bs[6].player == 0 ||
			bs[1].taken && bs[1].player == 0 && bs[4].taken && bs[4].player == 0 && bs[7].taken && bs[7].player == 0 ||
			bs[2].taken && bs[2].player == 0 && bs[5].taken && bs[5].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[0].taken && bs[0].player == 0 && bs[4].taken && bs[4].player == 0 && bs[8].taken && bs[8].player == 0 ||
			bs[2].taken && bs[2].player == 0 && bs[4].taken && bs[4].player == 0 && bs[6].taken && bs[6].player == 0)
		{

			print("Player 1 wins!");
			GameObject c = (GameObject)GameObject.Instantiate(wait_canvas);
			c.GetComponentInChildren<Text>().text = "Player 1 wins!";
			gameFinished = true;
			
		}
		else if (bs[0].taken && bs[0].player == 1 && bs[1].taken && bs[1].player == 1 && bs[2].taken && bs[2].player == 1 ||
		  bs[3].taken && bs[3].player == 1 && bs[4].taken && bs[4].player == 1 && bs[5].taken && bs[5].player == 1 ||
		  bs[6].taken && bs[6].player == 1 && bs[7].taken && bs[7].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[0].taken && bs[0].player == 1 && bs[3].taken && bs[3].player == 1 && bs[6].taken && bs[6].player == 1 ||
		  bs[1].taken && bs[1].player == 1 && bs[4].taken && bs[4].player == 1 && bs[7].taken && bs[7].player == 1 ||
		  bs[2].taken && bs[2].player == 1 && bs[5].taken && bs[5].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[0].taken && bs[0].player == 1 && bs[4].taken && bs[4].player == 1 && bs[8].taken && bs[8].player == 1 ||
		  bs[2].taken && bs[2].player == 1 && bs[4].taken && bs[4].player == 1 && bs[6].taken && bs[6].player == 1)
		{

			print("Player 2 wins!");
			GameObject c = (GameObject)GameObject.Instantiate(wait_canvas);
			c.GetComponentInChildren<Text>().text = "Player 2 wins!";
			gameFinished = true;
		}
	}

/////////////////////////////////////////////////////////////////////////////////

	public GameObject back;
	public GameObject cursor;
	private GameObject scriptCursor;
	private GameObject scriptCursor2 = null;
	private bool gameStarted = false;

	public int playerTurn = 0;

	Vector3 rayCollision;
	Vector2 mousePos;


	BoardSpace[] bs;
	private class BoardSpace
	{
		public bool taken;
		public int player;
	}


	void Start()
	{
		bs = new BoardSpace[9];
		for (int i = 0; i < 9; i++)
		{
			bs[i] = new BoardSpace();
		}

	}

	void StartGame() {

		Debug.Log("Game is Starting");

		gameStarted = true;
		//scriptCursor = (GameObject)Network.Instantiate(cursor, new Vector3(0, 0, -0.5f), transform.rotation, 0);
	}


	bool madeCursor = false;
	bool gameFinished = false;
	void Update() {
		if (gameStarted && gameFinished) {

			if (!madeCursor) {
				scriptCursor = (GameObject)Network.Instantiate(cursor, new Vector3(0, 0, -0.5f), transform.rotation, 0); 
				madeCursor = true;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			Debug.Log("This hit at " + hit.point);

			Vector3 c = new Vector3(hit.point.x, hit.point.y, -0.5f);
			scriptCursor.transform.position = c;
		}
	}


}
