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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float verticalMovement;
    [SerializeField] public int movementDir;
    [SerializeField] public Vector3 velEularReading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsOwner) return;
        playerCT = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        
        if(!IsOwner) return;
        if (Keyboard.current.fKey.isPressed) transform.position = Vector3.zero;

        inputKeys();
        
        playerCT.linearVelocity = playerCam.currentcamRot * movementVector * moveSpeed;
        
        
        
    }
    
    void inputKeys()
    {
        int roundVER = Mathf.RoundToInt(velEularReading.y);
        velEularReading = Quaternion.LookRotation(playerCT.linearVelocity).eulerAngles;
        if(roundVER == 45){movementDir = 0;}
        if(roundVER == 135){movementDir = 3;}
        if(roundVER == 225){movementDir = 2;}
        if(roundVER == 315){movementDir = 1;}
        horizontalMovement = 0f;
        verticalMovement = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)horizontalMovement = -1f;
                
            if (Keyboard.current.sKey.isPressed)horizontalMovement = 1f;

            if (Keyboard.current.aKey.isPressed)verticalMovement = 1f;
        
            if (Keyboard.current.dKey.isPressed)verticalMovement = -1f;
            
            
            
        }
        movementVector = new Vector3(verticalMovement, 0, horizontalMovement);
        if (movementVector.magnitude > 1f)
        {
            movementVector.Normalize();
        }
    }
}
