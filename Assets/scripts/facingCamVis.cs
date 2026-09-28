using System;
using Unity.VisualScripting;
using UnityEngine;

public class facingCamVis : MonoBehaviour
{
    [SerializeField] cameraController playerCam;

    [SerializeField] Transform parentTR;
    [SerializeField] public float dist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCam = GameObject.Find("playerCam").GetComponent<cameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCam.camRot * Quaternion.Euler(-90, 0, 0);
        transform.position = parentTR.position + transform.rotation * (gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale.y * Vector3.up);

        //Debug.Log(gameObject.GetComponent<SpriteRenderer>().size.y/2 * transform.localScale);
    }
}
