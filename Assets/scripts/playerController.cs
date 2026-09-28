
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerCT;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] cameraController playerCam;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float verticalMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCT = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        inputKeys();
        //Vector3 movement = transform.rotation * Quaternion.Euler(0, playerCam.currentcamRot.y, 0) * (movementVector );
        if (movementVector.magnitude > 1f)
        {
            movementVector.Normalize();
        }

        playerCT.linearVelocity = playerCam.currentcamRot * movementVector * moveSpeed;
        Debug.Log(Quaternion.Euler(0, playerCam.currentcamRot.y, 0) + "  "+ playerCam.currentcamRot.y);
    }
    
    void inputKeys()
    {
        
        horizontalMovement = Mathf.Clamp(horizontalMovement, -1, 1);
        if (Keyboard.current.wKey.IsPressed())
        {
            horizontalMovement -= 0.1f;
        } else if(Keyboard.current.sKey.IsPressed()){
            horizontalMovement += 0.1f;
        } else horizontalMovement = 0;

        verticalMovement = Mathf.Clamp(verticalMovement, -1, 1);
        if (Keyboard.current.dKey.IsPressed())
        {
            verticalMovement -= 0.1f;
        } else if (Keyboard.current.aKey.IsPressed())
        {
            verticalMovement += 0.1f;
        } else verticalMovement = 0;
        movementVector = new Vector3(verticalMovement, 0, horizontalMovement);
    }
}
