using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainGameController : MonoBehaviour {

    private NetworkManager _manager;

    // Use this for initialization
    void Start () {
        _manager = FindObjectOfType<NetworkManager>();
	}

    public void Disconnect()
    {
        _manager.StopClient();
    }
}
