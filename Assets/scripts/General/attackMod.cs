using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Collections.Generic;
[System.Serializable]
public class attackMod : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    
    public attackDelegation.attackContext c;
    public wolfStateMachine wSM;
    public bool processingAttack;
    //[SerializeField] public GameObject visTell;
    public GameObject visMask;
   public SpriteRenderer visSprite;
   
    public float telegraphTime;
    public float attackSpeed;
    public float attackRange;
    public List<GameObject> listOfAttacks;

    public void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, wSM.faceDir.Value);
    }

    public virtual void Initialized()
    {

        
        visSprite.transform.localScale = new Vector2(ff.sprite.rect.width/ff.sprite.pixelsPerUnit, 0);
        visSprite.transform.localPosition = new Vector2(0, visSprite.transform.localScale.y/2);
    }
}
