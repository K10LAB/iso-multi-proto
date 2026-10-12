using UnityEngine.InputSystem; 
using Unity.Netcode;
using UnityEngine;
using System.Linq;  
using System.Collections.Generic;

public class playerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody playerCT;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] cameraController playerCam;
    [SerializeField] facingCamVis playerVis;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float wsKeys;
    [SerializeField] private float adKeys;
    
    [SerializeField] public Vector3 velEularReading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsOwner) return;
        playerCT = gameObject.GetComponent<Rigidbody>();
        playerVis = gameObject.GetComponent<facingCamVis>();
        playerCam = gameObject.GetComponent<cameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        inputKeys();
        faceDirChange();
    }

    void FixedUpdate()
    {
        if(!IsOwner) return;
        
        playerMovement();
    }

    void playerMovement()
    {
        movementVector = new Vector3(adKeys, 0, wsKeys);
        if (movementVector.magnitude > 1f)
        {
            movementVector.Normalize();
        }
        playerCT.linearVelocity = playerCam.currentcamRot * movementVector * moveSpeed;
    }

    void inputKeys()
    {
        
        wsKeys = 0f;
        adKeys = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)wsKeys += -1f;
                
            if (Keyboard.current.sKey.isPressed)wsKeys += 1f;

            if (Keyboard.current.aKey.isPressed)adKeys += 1f;
        
            if (Keyboard.current.dKey.isPressed)adKeys += -1f;

        }
        
    }

    void faceDirChange()
    {
        switch(playerCam.currentcamRot.eulerAngles.y)
        {
            case 45:
                if(adKeys == 0)
                {
                    if(wsKeys == 1)
                    {
                        playerVis.faceDir.Value = 0;
                    }else if (wsKeys == -1)
                    {
                        playerVis.faceDir.Value = 2;
                    }
                } else if (adKeys == 1)
                {
                    playerVis.faceDir.Value = 3;
                } else if (adKeys == -1)
                {
                    playerVis.faceDir.Value = 1;
                }
            break;

            case 135:
            if(adKeys == 0)
                {
                    if(wsKeys == 1)
                    {
                        playerVis.faceDir.Value = 3;
                    }else if (wsKeys == -1)
                    {
                        playerVis.faceDir.Value = 1;
                    }
                } else if (adKeys == 1)
                {
                    playerVis.faceDir.Value = 2;
                } else if (adKeys == -1)
                {
                    playerVis.faceDir.Value = 0;
                }
            break;

            case 225:
                if(adKeys == 0)
                {
                    if(wsKeys == 1)
                    {
                        playerVis.faceDir.Value = 2;
                    }else if (wsKeys == -1)
                    {
                        playerVis.faceDir.Value = 0;
                    }
                } else if (adKeys == 1)
                {
                    playerVis.faceDir.Value = 1;
                } else if (adKeys == -1)
                {
                    playerVis.faceDir.Value = 3;
                }
            break;

            case 315:
                if(adKeys == 0)
                {
                    if(wsKeys == 1)
                    {
                        playerVis.faceDir.Value = 1;
                    }else if (wsKeys == -1)
                    {
                        playerVis.faceDir.Value = 3;
                    }
                } else if (adKeys == 1)
                {
                    playerVis.faceDir.Value = 0;
                } else if (adKeys == -1)
                {
                    playerVis.faceDir.Value = 2;
                }
            break;
        }
        
        
    }
    
    
}
