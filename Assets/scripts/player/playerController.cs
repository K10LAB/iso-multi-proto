using UnityEngine.InputSystem; 
using Unity.Netcode;
using UnityEngine;
using System.Linq;  
using System.Collections.Generic;
using Unity.VisualScripting;

public class playerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D playerCT;
    [SerializeField] private Vector2 movementVector;
    //[SerializeField] cameraController playerCam;
    //[SerializeField] facingCamVis playerVis;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float wsKeys;
    [SerializeField] private float adKeys;
    
    [SerializeField] public Vector3 velEularReading;

    [SerializeField] public SpriteRenderer playerVis;
    [SerializeField] public Sprite[] spriteList; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsOwner) return;
        transform.position = Vector2.zero;
        playerCT = GetComponent<Rigidbody2D>();
        //playerVis = gameObject.GetComponent<facingCamVis>();
        //playerCam = gameObject.GetComponent<cameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        inputKeys();
        
    }

    void FixedUpdate()
    {
        if(!IsOwner) return;
        
        playerMovement();
    }

    void playerMovement()
    {
        movementVector = new Vector2(adKeys, wsKeys);
        if (movementVector.magnitude > 1f)
        {
            movementVector.Normalize();
        }
        playerCT.linearVelocity = Quaternion.Euler(0, 45, 0) * movementVector * moveSpeed;
        
    }

    void inputKeys()
    {
        wsKeys = 0f;
        adKeys = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)wsKeys += 1f;
                
            if (Keyboard.current.sKey.isPressed)wsKeys += -1f;

            if (Keyboard.current.aKey.isPressed)adKeys += -1f;
        
            if (Keyboard.current.dKey.isPressed)adKeys += 1f;

        }
        faceDirChange();
    }

    

    void faceDirChange()
    {
        if(wsKeys>0)playerVis.sprite = adKeys != 0 ? spriteList[0] : spriteList[1];
        else if(wsKeys<0)playerVis.sprite = adKeys != 0 ? spriteList[3] : spriteList[4];
        else playerVis.sprite = spriteList[2];
        if(adKeys>0)playerVis.flipX=true;
        if(adKeys<0)playerVis.flipX=false;
    }
}

