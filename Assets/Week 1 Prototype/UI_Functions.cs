using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Functions : MonoBehaviour {

    public GameObject canvas_envelope;
    public InputField inputField;
    public Button clientConnect;
    public Button startServer;

    public bool serverAlreadyStarted = false;
    public bool alreadyConnected = false;
    private int numberOfPlayers = 0;
	public int numberOfPlayersNeeded = 1;

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
        Destroy(canvas_envelope);

		if (numberOfPlayers == numberOfPlayersNeeded)
		{
			Debug.Log("Game is Starting");
			StartGame();
		}
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

        if (numberOfPlayers == numberOfPlayersNeeded) {
            Debug.Log("Game is Starting");
			StartGame();
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

	public GameObject back;
	public GameObject cursor;
	private GameObject scriptCursor;
	private bool gameStarted = false;

	Vector3 rayCollision;
	Vector2 mousePos;

	void StartGame() {

		gameStarted = true;
		back.renderer.enabled = true;
		scriptCursor = (GameObject)Network.Instantiate(cursor, new Vector3(0, 0, -0.5f), transform.rotation, 0);
	}

	void Update() {
		if (gameStarted) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			Debug.Log("This hit at " + hit.point);

			Vector3 c = new Vector3(hit.point.x, hit.point.y, -0.5f);
			scriptCursor.transform.position = c;
		}
	}
}
