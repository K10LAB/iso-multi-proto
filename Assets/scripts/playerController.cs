
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] private CharacterController playerCT;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] cameraController playerCam;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float verticalMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCT = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        inputKeys();
        playerCT.Move(transform.rotation*Quaternion.Euler(0, playerCam.currentcamRot.y, 0) * (movementVector * moveSpeed * Time.deltaTime));
    }
    
    void inputKeys()
    {
        
        horizontalMovement = Mathf.Clamp(horizontalMovement, -1, 1);
        if (Keyboard.current.wKey.IsPressed())
        {
            horizontalMovement += 0.1f;
        } else if(Keyboard.current.sKey.IsPressed()){
            horizontalMovement -= 0.1f;
        } else horizontalMovement = 0;

        verticalMovement = Mathf.Clamp(verticalMovement, -1, 1);
        if (Keyboard.current.dKey.IsPressed())
        {
            verticalMovement += 0.1f;
        } else if (Keyboard.current.aKey.IsPressed())
        {
            verticalMovement -= 0.1f;
        } else verticalMovement = 0;
        movementVector = new Vector3(verticalMovement, 0, horizontalMovement);
    }
}
