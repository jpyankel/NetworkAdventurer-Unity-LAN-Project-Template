using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkAdventurer : NetworkDiscovery {
    private Dictionary<string, string> availableConnectionsList = new Dictionary<string, string>(); //First string is IP address, second is Alias.
    private Transform canvasListReference;
    private GameObject connectionUIObjPrefab;

    private void Awake() {
        canvasListReference = transform.Find("LAN-ConnectionsList/Main/Scroll View/Viewport/Content");
        connectionUIObjPrefab = Resources.Load("ConnectionObject") as GameObject;
    }
    public override void OnReceivedBroadcast(string fromAddress, string data) {
        base.OnReceivedBroadcast(fromAddress, data);
        if (availableConnectionsList.ContainsKey(fromAddress)) {
            if (availableConnectionsList[fromAddress] != data) {
                availableConnectionsList[fromAddress] = data;
            }
        }
        else {
            addNewAvailableConnection(fromAddress, data);
        }
    }
    public void onButtonRefresh() {
        refreshList();
    }
    private void refreshList () {
        availableConnectionsList.Clear();
    }
    private void addNewAvailableConnection(string fromAddress, string alias) {
        availableConnectionsList.Add(fromAddress, alias);
        GameObject newConnectionUIObj = Instantiate(connectionUIObjPrefab, canvasListReference, false);
        newConnectionUIObj.transform.Find("InfoContainer/AliasContainer/TextContainer/Text").GetComponent<Text>().text = alias;
        newConnectionUIObj.transform.Find("InfoContainer/IPContainer/TextContainer/Text").GetComponent<Text>().text = fromAddress;
    }
}
