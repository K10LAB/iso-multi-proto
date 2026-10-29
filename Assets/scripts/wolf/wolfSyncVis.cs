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
    

    [SerializeField] public wolfStateMachine wSM;
    public void Awake()
    {
        wSM = GetComponent<wolfStateMachine>();
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        wSM.faceDir.OnValueChanged += onFaceDirChange;
        onFaceDirChange(0f, wSM.faceDir.Value);

        wSM.networkCurrentState.OnValueChanged += onStateChange;
        onStateChange(default, wSM.networkCurrentState.Value);

        wSM.isChase.OnValueChanged += onChaseChange;
        onChaseChange(false, wSM.isChase.Value);

        wSM.detectionLevel.OnValueChanged += onDetectionChange;
        onDetectionChange(0f, wSM.detectionLevel.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        wSM.networkCurrentState.OnValueChanged -=  onStateChange;
    }
    public void onDetectionChange(float Old, float New)
    {
        maskDetection.transform.localPosition = new Vector2(0, 0.3374f + New*0.003281f);
    }

    public void onChaseChange(bool Old, bool New)
    {
        if (New)
        {
            visDetection.sprite = spriteList[4];
            visDetection.color = Color.red;
        } else
        {
            visDetection.sprite = spriteList[5];
            visDetection.color = Color.yellow;
        }
    }

    public void onFaceDirChange(float Old, float New)
    {        
        if(New>=315 || New<45)visSkin.sprite = spriteList[0];
        if(New>=45 && New<135)visSkin.sprite = spriteList[1];
        if(New>=135 && New<225)visSkin.sprite = spriteList[2];
        if(New>=225 && New<315)visSkin.sprite = spriteList[3];
        
    }    
    void onStateChange(wolfStateMachine.wolfStates Old, wolfStateMachine.wolfStates New)
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