using System;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class facingCamVis : NetworkBehaviour
{
    [SerializeField] cameraController playerCam;

    [SerializeField] Transform parentTR;
    [SerializeField] public float dist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        tryFindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerCam != null)
        {
            transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
            transform.position = parentTR.position + transform.rotation * (gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale.y * Vector3.up);
        } else tryFindPlayer();
        
        //Debug.Log(gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale);
    }
    void tryFindPlayer()
    {
        if (NetworkManager.Singleton != null)
        {
            Debug.Log(1);
            var localClient = NetworkManager.Singleton.LocalClient;
            Debug.Log(localClient);
            if (localClient != null && localClient.PlayerObject != null)
            {
                Debug.Log(2);
                playerCam = localClient.PlayerObject.GetComponent<cameraController>();
            }
        }
    }
}
