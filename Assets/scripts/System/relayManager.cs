using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models; 
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class relayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private TextMeshProUGUI joinText;
    [SerializeField]private TMP_InputField relayInput;
    [SerializeField]private GameObject menuScreen;
    [SerializeField]private GameObject loadingScreen;
    
    //[SerializeField]private Canvas gameCanvas;
    //[SerializeField]private Canvas loadingScreen;


    
    


    

    // Update is called once per frame
    private void Awake()
    {
        
        Resolution currentRes = Screen.currentResolution;
        
        
        Screen.SetResolution(currentRes.width, currentRes.height, FullScreenMode.FullScreenWindow);
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
    }
    
    
    public async void startRelay()
    {
        
        
        menuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        //loadingScreen.enabled = true;
        string joinCode = "Join Code:" + await startHost();
        joinText.text = joinCode;
        loadingScreen.SetActive(false);
        
        
        
    }
    

    public async void joinRelay()
    {
        
        menuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        await startClient(relayInput.text);
        loadingScreen.SetActive(false);
        joinText.text = "Join Code:" + relayInput.text;
        
        //loadingScreen.enabled = false;
        //gameCanvas.enabled = true;
    }

    

    public async void shutdownRelay()
    {
        
        //gameCanvas.enabled = false;
        //loadingScreen.enabled = true;
        loadingScreen.SetActive(true);

        joinText.text = "";
        if(NetworkManager.Singleton.IsClient){
            NetworkManager.Singleton.Shutdown();
        } else if(NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }
        loadingScreen.SetActive(false);
        menuScreen.SetActive(true);
        //loadingScreen.enabled = false;
        
        
    }

    private void OnEnable() 
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }
    }

    private void OnDisable() 
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(!NetworkManager.Singleton.IsServer && clientId == NetworkManager.Singleton.LocalClientId)
        {
            shutdownRelay();
        }
    }
    




    public async Task<string> startHost(int max = 3)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(max);
        var relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }
    
    public async Task<bool> startClient(string relayCode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
        var relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        return !string.IsNullOrEmpty(relayCode) &&NetworkManager.Singleton.StartClient();
    }
    
}
