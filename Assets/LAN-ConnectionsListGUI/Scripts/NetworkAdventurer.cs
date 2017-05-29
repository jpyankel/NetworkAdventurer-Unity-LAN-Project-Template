using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkAdventurer : NetworkDiscovery {
    private Dictionary<string, string> availableConnectionsList = new Dictionary<string, string>(); //First string is IP address, second is Alias.
    private GameObject connectionUIObjPrefab;
    private Transform canvasListReference; //Must modify this manually if you wish to change the location of the UI container!

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

    #region --- [Input Handlers] ---
    public void onButtonRefresh() {
        refreshList();
    }
    #endregion

    private void refreshList() {
        availableConnectionsList.Clear();
    }
    private void addNewAvailableConnection(string fromAddress, string alias) {
        availableConnectionsList.Add(fromAddress, alias);
        GameObject newConnectionUIObj = Instantiate(connectionUIObjPrefab, canvasListReference, false);
        newConnectionUIObj.transform.Find("InfoContainer/AliasContainer/TextContainer/Text").GetComponent<Text>().text = alias;
        newConnectionUIObj.transform.Find("InfoContainer/IPContainer/TextContainer/Text").GetComponent<Text>().text = fromAddress;
        canvasListReference.GetComponent<ToggleGroup>().RegisterToggle(newConnectionUIObj.GetComponent<Toggle>());
    }
    /// <summary>
    /// Gets the currently toggled connection UI object from the canvasListReference.
    /// </summary>
    /// <returns>A string representing the target IP address. Null if no selection is made.</returns>
    public string getSelectedConnection () {
        ToggleGroup toggleList = canvasListReference.GetComponent<ToggleGroup>();
        if (!toggleList)
            return null;
        Toggle chosenToggle = toggleList.ActiveToggles().FirstOrDefault();
        if (chosenToggle != null) {
            return chosenToggle.transform.Find("InfoContainer/IPContainer/TextContainer/Text").GetComponent<Text>().text;
        }
        return null;
    }
}
