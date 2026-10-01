using System;
using System.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class facingCamVis : NetworkBehaviour
{
    [SerializeField] cameraController playerCam;
    [SerializeField] playerController playerCon;
    [SerializeField] bool ifPlayer;

    [SerializeField] Transform parentTR;

    [SerializeField] SpriteRenderer visSkin;
    [SerializeField] int facingDirect;
    //north=0,west=1,south=2,east=3
    [SerializeField] public Sprite[] spriteFaces = new Sprite[4];
    [SerializeField] bool if4Sided;
    //[SerializeField] int tempDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        visSkin = GetComponent<SpriteRenderer>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartCoroutine(tryFindPlayer());
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerCam == null || playerCon == null)return;
        
        transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
        transform.position = transform.parent.position + transform.rotation * (visSkin.size.y/2 * transform.localScale.y * Vector3.up);
        if(if4Sided)faceDirChange();
        
        
        //Debug.Log(gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale);
    }
    IEnumerator tryFindPlayer()
    {
        if (NetworkManager.Singleton != null)
        {
            //Debug.Log(1);
            var localClient = NetworkManager.Singleton.LocalClient;
            //Debug.Log(localClient);
            if (localClient != null && localClient.PlayerObject != null)
            {
                //Debug.Log(2);
                playerCam = localClient.PlayerObject.GetComponent<cameraController>();
                playerCon = localClient.PlayerObject.GetComponent<playerController>();
                
            } else yield return null;
            
        }else yield return null;
    }
    public void faceDirChange()
    {
        if(ifPlayer)facingDirect = playerCon.movementDir.Value;
        visSkin.sprite = spriteFaces[((int)playerCam.camRot.eulerAngles.y/90+facingDirect)%4];
    }
}
