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

    [SerializeField] public SpriteRenderer stateVisual;

    [SerializeField] public cameraController playerCam;

    
    [SerializeField] public NetworkVariable<int> faceDir = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    
    [SerializeField] SpriteRenderer visSkin;
    [SerializeField] public Sprite[] spriteFaces = new Sprite[4];
    
    
    void Awake()
    {
        cObj = transform.GetChild(0).gameObject;
        visSkin = cObj.GetComponent<SpriteRenderer>();
        StartCoroutine(tryFindPlayer());
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        StartCoroutine(tryFindPlayer());
        //wSM.networkCurrentState.OnValueChanged += onStateChange;
        //onStateChange(default, wSM.networkCurrentState.Value);
        
        
    }
    IEnumerator tryFindPlayer()
    {
        
        var localClient = NetworkManager.Singleton.LocalClient;
        Debug.Log(localClient);
        if(localClient.PlayerObject == null)yield return null;
        pObj = localClient.PlayerObject.gameObject;
        playerCam = pObj.GetComponent<cameraController>();
    }

    //public override void OnNetworkDespawn()
    //{
        //base.OnNetworkDespawn();
        //wSM.networkCurrentState.OnValueChanged -=  onStateChange;
    //}
    /*
    void onStateChange(wolfStateMachine.wolfStates old, wolfStateMachine.wolfStates New)
    {
        switch (New)
        {
            case wolfStateMachine.wolfStates.Idle:
            stateVisual.color = Color.green;
            break;
            case wolfStateMachine.wolfStates.Patrol:
            stateVisual.color = Color.yellow;
            break;
            case wolfStateMachine.wolfStates.Chase:
            stateVisual.color = Color.red;
            break;
        }
    }
    */
    
    

    // Update is called once per frame
    void Update()
    {
        
        if (playerCam == null)return;
        
        cObj.transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
        cObj.transform.position = transform.position + cObj.transform.rotation * (visSkin.size.y/2f * cObj.transform.localScale.y * Vector3.up);
        faceDirChange();
        
        
        //Debug.Log(gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale);
    }
    
    public void faceDirChange()
    {
        visSkin.sprite = spriteFaces[((int)playerCam.camRot.eulerAngles.y/90 + faceDir.Value)%4];
    }
}
