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
    
    public GameObject playerFocus;

    //[SerializeField] public GameObject visTell;
    public SpriteRenderer visMask;
    public SpriteRenderer visSprite;
    public Sprite teleSprite;
    public bool isAoe;

    public float trackingTime;
    public float trackSpeed;
    public Vector2 spawnPoint;
    public float[] attackIntervals;
    public int currentAttackInt;
    public GameObject attackProj;

    public GameObject closestPlayer()
    {
        if(c.playerList.Count !=0){
            float closeDistance = float.MaxValue;
            foreach (GameObject player in c.playerList)
            {
                float distanceToPlayer = Vector2.Distance(spawnPoint, player.transform.position);
                if (distanceToPlayer <= closeDistance)
                {
                    playerFocus = player;
                    closeDistance = distanceToPlayer;
                }
            }
        }
        return playerFocus;
    }

    public float faceDirChange(Vector2 flatDir)
    {
        
        flatDir -= new Vector2(spawnPoint.x, spawnPoint.y);
        float mathDir = Mathf.Atan2(flatDir.x, flatDir.y) * Mathf.Rad2Deg;
        if (mathDir < 0)
        {
            return 360f + mathDir;
        } else return mathDir;
    }
    
    
    public void Update()
    {
        float time = Mathf.Max(0, (c.currentTime-timeBeforeTelegraph)/timeBeforeAttack);
        if (time < 1)
        {
            if (!isAoe)
            {
                if(time<trackingTime){
                    transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -faceDirChange(closestPlayer().transform.position)), trackSpeed * Time.deltaTime);
                } 
                transform.localPosition = transform.localRotation * Vector3.up * (visSprite.transform.localScale.y / 2f);
                visMask.transform.localPosition = new Vector2(0, time-1);
            }
            else
            {
                if(time<trackingTime){
                    transform.position = Vector2.Lerp(transform.position, closestPlayer().transform.position, trackSpeed * Time.deltaTime);
                }
                visMask.transform.localScale = new Vector2(time, time);
            }
        } else
        {
            visSprite.enabled = false;
            visMask.enabled = false;
            spawnAttack();
        }
    }

    public void spawnAttack()
    {
        if(currentAttackInt == attackIntervals.Length) {
            gameObject.SetActive(false);
            return;
        }
        float time = c.currentTime-timeBeforeAttack;
        for (int i = 0; i<currentAttackInt; i++)
        {
            time -= attackIntervals[currentAttackInt];
        }
        if(time>=attackIntervals[currentAttackInt])
        {
            GameObject g = Instantiate(attackProj, spawnPoint, transform.rotation);
            NetworkObject f = g.GetComponent<NetworkObject>();
            f.Spawn();
            currentAttackInt++;
        }
        
        
    }
    public void OnEnable()
    {
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.Euler(0, 0, -faceDirChange(closestPlayer().transform.position));
        visSprite.sprite = teleSprite;
        visMask.sprite = teleSprite;
        spawnPoint = transform.position;
        visSprite.enabled = true;
        visMask.enabled = true;
        currentAttackInt = 0;
        
        
    }



    

    
}
