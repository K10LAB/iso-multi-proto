using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class gameManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.NonSerialized]
    //private readonly Dictionary<ulong, NetworkObject> playerObjects = new Dictionary<ulong, NetworkObject>();
    [SerializeField] public NetworkList<ulong> playerObjects = new NetworkList<ulong>(
        null,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    } 

    void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId) != null)
        {
            playerObjects.Add(clientId);
        }
    }
    void OnClientDisconnected(ulong clientId)
    {
        if (playerObjects.Contains(clientId))
        {
            playerObjects.Remove(clientId);
        }
    }
    
    public ulong getClientID(int clientOrder)
    {
        return playerObjects[clientOrder];
    }
    
    public GameObject getClientPlayerObject(ulong clientId)
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        if (playerObject != null)
        {
            return playerObject.gameObject;
        }
        return null;
    }
    

    // Update is called once per frame
   
}
