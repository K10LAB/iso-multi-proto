using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class wolfSyncVis : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer visState;

    [SerializeField] private SpriteRenderer visSkin;
    
    [SerializeField] private SpriteRenderer visDetection;

    [SerializeField] private GameObject maskDetection;
    

    [SerializeField] List<Sprite> spriteList = new List<Sprite>(6);
    
    public void onDetectionChange(float Old, float New)
    {

        maskDetection.transform.localPosition = new Vector2(0, -2 * (1 - New/100));
        
        if (New<=0)
        {
            visDetection.sprite = spriteList[6];
            visDetection.color = Color.yellow;
        } else if(New>=100){
            visDetection.sprite = spriteList[7];
            visDetection.color = Color.red;
        }
    }

    public void onFaceDirChange(float Old, float New)
    {
        
        visSkin.flipX=!(New>180);
        if(New>337.5f || New >= 0 && New<=22.5f)visSkin.sprite = spriteList[0];
        if(New>22.5f && New<=67.5f || New>202.5f && New<=247.5f)visSkin.sprite = spriteList[1];
        if(New>67.5f && New<=112.5f || New>247.5f && New<=292.5f)visSkin.sprite = spriteList[2];
        if(New>112.5f && New<=157.5f || New>292.5f && New<=337.5f)visSkin.sprite = spriteList[3];
        if(New>157.5f && New<=202.5f)visSkin.sprite = spriteList[4];
        if(New<=-1f)visSkin.sprite = spriteList[5];
        
    }    
    public void onStateChange(wolfStateMachine.wolfStates Old, wolfStateMachine.wolfStates New)
    {
        switch (New)
        {
            case wolfStateMachine.wolfStates.Idle:
            visState.color = Color.green;
            
            break;
            case wolfStateMachine.wolfStates.Patrol:
            visState.color = Color.yellow;
            break;
            case wolfStateMachine.wolfStates.Chase:
            visState.color = Color.red;
            break;
            case wolfStateMachine.wolfStates.Attack:
            visState.color = Color.orange;
            break;
        }
    }
    
}