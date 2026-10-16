using System;
using System.Collections;

using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class facingCamVis : NetworkBehaviour
{
    
    [SerializeField] public GameObject cObj;

    [SerializeField] public GameObject pObj;

    

    //[SerializeField] private SpriteRenderer stateVisual;

    [SerializeField] public cameraController playerCam;
    
    [SerializeField] public NetworkVariable<int> faceDir = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    
    [SerializeField] SpriteRenderer visSkin;
    [SerializeField] public Sprite[] spriteFaces = new Sprite[4];
    
    
    void Start()
    {
        cObj = transform.GetChild(0).gameObject;
        visSkin = cObj.GetComponent<SpriteRenderer>();
       
        
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("camstartfind");
        
        //StartCoroutine(tryFindPlayer());
       
        
        
    }
    /*
    IEnumerator tryFindPlayer()
    {
        
        
        while (NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null; 
        }


        pObj = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        playerCam = pObj.GetComponent<cameraController>();
        
        
    }
    */
    
    void Update()
    {
        
        if (playerCam == null)return;
       
        cObj.transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
        cObj.transform.position = transform.position + cObj.transform.rotation * (visSkin.size.y/2f * cObj.transform.localScale.y * Vector3.up);
        faceDirChange();
        
        
        
    }
    
    public void faceDirChange()
    {
        visSkin.sprite = spriteFaces[((int)playerCam.camRot.eulerAngles.y/90 + faceDir.Value)%4];
    }
}
