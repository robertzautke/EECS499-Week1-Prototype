using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Functions : MonoBehaviour {

    public GameObject canvas_envelope;
    public GameObject canvas2_envelope;
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
            Destroy(canvas_envelope);
            Network.Instantiate(canvas2_envelope, transform.position, transform.rotation, 0);

            add = GameObject.Find("Add").GetComponent<Button>();
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
            Destroy(canvas_envelope);

        }
    } 


    public void addToCounter() {
        networkView.RPC("addToAllCounters", RPCMode.All);
    }

    [RPC]
    public void addToAllCounters() {
        count++;
        countText.text = count.ToString();
    }

    void ConnectToServer()
    {
        Network.Connect(inputField.text.ToString(), 25000, "password");
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
        alreadyConnected = true;

        add = GameObject.Find("addToCounter").GetComponent<Button>();
        add.onClick.AddListener(() => { addToCounter(); });
        countText = GameObject.Find("Count").GetComponent<Text>();
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
