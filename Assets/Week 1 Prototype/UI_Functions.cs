using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Functions : MonoBehaviour {

    public GameObject canvas_envelope;
    public GameObject local_canvas_envelope;
    public InputField inputField;
    public Button clientConnect;
    public Button startServer;
    public Button add;
    public Text countText;

    private int count = 0;
    private bool serverAlreadyStarted = false;
    private bool alreadyConnected = false;

	void Start () {
	}
	
	void Update () {
	
	}

    public void startServerButtonClick() {
        if (!serverAlreadyStarted) {

            Network.incomingPassword = "password";
            bool useNat = !Network.HavePublicAddress();
            Network.InitializeServer(32, 25000, useNat);

            Destroy(local_canvas_envelope);
            Network.Instantiate(canvas_envelope, transform.position, transform.rotation, 0);

            add = GameObject.Find("addToCounter").GetComponent<Button>();
            add.onClick.AddListener(() => { addToCounter(); });
            print(add.GetComponentInChildren<Text>().text);
            countText = GameObject.Find("Count").GetComponent<Text>();
            print(countText.text);
            count = 0;
            serverAlreadyStarted = true;
        }    
    }

    public void connectToServerButtonClick() {
        if (!alreadyConnected) {

            ConnectToServer();

            alreadyConnected = true;
        }
    } 

    [RPC]
    public void addToCounter() {
        count++;
        countText.text = count.ToString();
    }

    void ConnectToServer()
    {
        Network.Connect("216.106.154.95", 25000, "password");
    }

////Generic debug output for server/client status////

    private int playerCount = 0;

    void OnServerInitialized()
    {
        Debug.Log("Server initialized and ready");
    }
    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
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
    }
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
    }
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
}
