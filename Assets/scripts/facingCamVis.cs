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

    [SerializeField] public cameraController playerCam;
    [SerializeField] public playerController playerCon;
    
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
        StartCoroutine(tryFindPlayer());
        
    }
    IEnumerator tryFindPlayer()
    {
        
        var localClient = NetworkManager.Singleton.LocalClient;
        if(localClient.PlayerObject == null)yield return null;
        pObj = localClient.PlayerObject.gameObject;
        playerCam = pObj.GetComponent<cameraController>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
        if (playerCam == null)return;
        
        cObj.transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
        cObj.transform.position = transform.position + cObj.transform.rotation * (visSkin.size.y/2 * cObj.transform.localScale.y * Vector3.up);
        faceDirChange();
        
        
        //Debug.Log(gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale);
    }
    
    public void faceDirChange()
    {
        visSkin.sprite = spriteFaces[((int)playerCam.camRot.eulerAngles.y/90 + faceDir.Value)%4];
    }
}
