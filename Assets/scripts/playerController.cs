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
    [SerializeField] public NetworkVariable<int> movementDir = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public float fl;
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
        bool wP = Keyboard.current.wKey.isPressed;
        bool aP = Keyboard.current.aKey.isPressed;
        bool sP = Keyboard.current.sKey.isPressed;
        bool dP = Keyboard.current.dKey.isPressed;
        horizontalMovement = 0f;
        verticalMovement = 0f;
        if (Keyboard.current != null)
        {
            if (wP)
            {
                horizontalMovement = -1f;
                if(!sP)movementDir.Value = 2;
            }

            if (sP)
            {
                horizontalMovement = 1f;
                if(!wP)movementDir.Value = 4;
            }

            if (aP)
            {
                verticalMovement = -1f;
                if(!dP)movementDir.Value = 1;
            }
            if (dP)
            {
                verticalMovement = 1f;
                if(!aP)movementDir.Value = 3;
            }
            
            
            
        }
        movementVector = new Vector3(verticalMovement, 0, horizontalMovement);
        if (movementVector.magnitude > 1f)
        {
            movementVector.Normalize();
        }
    }
}
