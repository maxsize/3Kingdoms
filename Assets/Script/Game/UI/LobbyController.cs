using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Adic;
using ThreeK.Game.Event;
using ThreeK.Game.UI;

public class LobbyController : MonoBehaviour
{
    [Inject][HideInInspector] public EventDispatcher Dispatcher;
    [Inject][HideInInspector] public MainUI MainUI;

    private NetworkManager _manager;

    private void Start()
    {
        this.Inject();
        Debug.Log(Dispatcher.UID);
        _manager = FindObjectOfType<NetworkManager>();
    }

    public void OnStartHost()
    {
        //NetworkServer.Listen(7777);
        //var client = ClientScene.ConnectLocalServer();
        //Dispatcher.DispatchWith<ClientConnectEvent>(client);
        _manager.StartHost();

        MainUI.HandleInput(new GameSceneInput());
    }

    public void OnStartClient()
    {
        //var client = new NetworkClient();
        //client.Connect(_manager.networkAddress, _manager.networkPort);
        //Dispatcher.DispatchWith<ClientConnectEvent>(client);

        _manager.StartClient();

        MainUI.HandleInput(new GameSceneInput());
    }

    public void OnServerAddressChange()
    {
        var input = GetComponentInChildren<InputField>();
        var address = input.text;
        _manager.networkAddress = address;
        Debug.Log("Server address changed: " + address);
    }
}
