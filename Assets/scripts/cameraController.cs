using System;
//using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class cameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTR;
    [SerializeField] public float distanceFromPL;
    [SerializeField] public float rotationOfPos;
    [SerializeField] public float camtransitionTime;
    [SerializeField] public Quaternion camRot;
    [SerializeField] public Quaternion currentcamRot;

    [SerializeField] private float mouseZoom = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camRot = Quaternion.Euler(rotationOfPos, 0, 0);
        currentcamRot = Quaternion.Euler(rotationOfPos, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        mouseZoom += Mouse.current.scroll.ReadValue().y/10f;
        mouseZoom = Mathf.Clamp(mouseZoom, 0f, 1f);
        if(Keyboard.current.qKey.wasPressedThisFrame) currentcamRot = Quaternion.Euler(currentcamRot.eulerAngles.x, currentcamRot.eulerAngles.y+45, 0);
        if(Keyboard.current.eKey.wasPressedThisFrame) currentcamRot = Quaternion.Euler(currentcamRot.eulerAngles.x, currentcamRot.eulerAngles.y-45, 0);
        camRot = Quaternion.Lerp(camRot, currentcamRot, camtransitionTime * Time.deltaTime);
        transform.position = playerTR.position + camRot * (mouseZoom * distanceFromPL * Vector3.up);
        transform.LookAt(playerTR.position);
    }
}
