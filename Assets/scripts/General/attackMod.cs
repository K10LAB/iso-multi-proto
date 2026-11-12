using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;
[System.Serializable]
public class attackMod : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public attackDelegation c;
    public float timeBeforeAttack;
    public float timeBeforeTelegraph;
    public float currentTime;

    //[SerializeField] public GameObject visTell;
    public SpriteMask visMask;
    public SpriteRenderer visSprite;
    public Sprite teleSprite;

    public Component attackTrigger;

    public bool isAoe;
    public float trackingTime;
    public float trackSpeed;
    public Vector2 spawnPoint;


    public void timePass()
    {
        currentTime += Time.deltaTime;
    }
    public bool updateTelegraph()
    {
        float time = Math.Max(0, currentTime/timeBeforeAttack);
        if(time<1)return true;
        if (!isAoe)
        {
            if(time<trackingTime){
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, c.currentFaceDir, 0), trackSpeed * Time.deltaTime);
                transform.localPosition = transform.localRotation * Vector3.up * (visSprite.transform.localScale.y / 2f);
            }
            visMask.transform.localPosition = spawnPoint + new Vector2(0, -visSprite.transform.localScale.y * (time - 1));
        }
        else
        {
            if(time<trackingTime)transform.position = Vector2.Lerp(transform.position, c.playerFocus.transform.position, trackSpeed * Time.deltaTime);
            visMask.transform.localScale = new Vector2(time, time);
        }
        return false;
        
        
    }
    public void startTelegraph()
    {
        visSprite.sprite = teleSprite;
        visMask.sprite = teleSprite;
        spawnPoint = transform.localPosition;
        currentTime = -timeBeforeTelegraph;
    
    }



    

    
}
