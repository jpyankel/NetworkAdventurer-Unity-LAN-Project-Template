using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExampleImplementation : MonoBehaviour {

    private NetworkAdventurer networkAdventurer;
    private NetworkClient networkClient;

    [SerializeField]
    [Tooltip("Optional IP-address input field for determining the client's connect target.")]
    private InputField ipInputField;

    [SerializeField]
    [Tooltip("Optional port input field for determining the client/host's port settings.")]
    private InputField portInputField;

    private void Awake() {
        networkAdventurer = GetComponent<NetworkAdventurer>();
    }
    private void Start() {
        if (portInputField) {
            portInputField.characterValidation = InputField.CharacterValidation.Integer; //To ensure that no errors are called during an Int.parse
        }
        networkAdventurer.Initialize();
        networkAdventurer.StartAsClient();
    }

    private void startAsServer() {
        int port = 4444;
        if (portInputField) {
            port = int.Parse(portInputField.text);
        }
        NetworkServer.Listen(port);
        networkAdventurer.StopBroadcast(); //Stop the client from listening anymore.
        networkAdventurer.StartAsServer();
    }
    private void startAsClient() {
        networkAdventurer.StopBroadcast();
        networkClient = new NetworkClient();
        networkClient.RegisterHandler(MsgType.Connect, OnConnected_Client);
        int port = 4444;
        if (portInputField) {
            port = int.Parse(portInputField.text);
        }
        string ipAddress = "127.0.0.1";
        if (ipInputField) {
            ipAddress = ipInputField.text;
        }
        networkClient.Connect(ipAddress, port);
    }
    private void startAsLocalClient() {
        networkClient = ClientScene.ConnectLocalServer();
        networkClient.RegisterHandler(MsgType.Connect, OnConnected_Client);
    }
    private void startAsHost() {
        startAsServer();
        startAsLocalClient();
    }

	#region --- [Input Handlers] ---
	public void onButton_Host () {
        startAsHost();
    }
	public void onButton_Server () {
        startAsServer();
    }
	public void onButton_Client () {
        startAsClient();
    }
	#endregion


    #region --- [Server-Side Implementation] ---

    #endregion

    #region --- [Client-Side Implementation] ---
    private void OnConnected_Client(NetworkMessage netMsg) {
        Debug.Log("This client has connected to server");
    }
    #endregion
}
