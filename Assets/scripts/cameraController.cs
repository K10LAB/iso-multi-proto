using System;
//using System.Numerics;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class cameraController : NetworkBehaviour
{
    [SerializeField] private Transform camTR;
    [SerializeField] public float distanceFromPL;
    [SerializeField] public float rotationOfPos;
    [SerializeField] public float camtransitionTime;
    [SerializeField] public Quaternion camRot;
    [SerializeField] public Quaternion currentcamRot;

    [SerializeField] private float mouseZoom = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsOwner)
        {
            camTR.gameObject.SetActive(false);
        } else
        {
            camTR.gameObject.SetActive(true);
            camRot = Quaternion.Euler(rotationOfPos, 45, 0);
            currentcamRot = Quaternion.Euler(rotationOfPos, 45, 0);
        }
        
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        mouseZoom += Mouse.current.scroll.ReadValue().y/10f;
        mouseZoom = Mathf.Clamp(mouseZoom, 0f, 1f);
        if(Keyboard.current.qKey.wasPressedThisFrame) currentcamRot = Quaternion.Euler(currentcamRot.eulerAngles.x, currentcamRot.eulerAngles.y+90, 0);
        if(Keyboard.current.eKey.wasPressedThisFrame) currentcamRot = Quaternion.Euler(currentcamRot.eulerAngles.x, currentcamRot.eulerAngles.y-90, 0);
        camRot = Quaternion.Lerp(camRot, currentcamRot, camtransitionTime * Time.deltaTime);
        camTR.position = transform.position + camRot * (mouseZoom * distanceFromPL * Vector3.up);
        camTR.LookAt(transform.position);
    }
}
