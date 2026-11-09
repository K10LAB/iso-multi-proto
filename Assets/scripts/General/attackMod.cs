using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Unity.VisualScripting;

public class attackMod : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent<attackDelegation.attackSystem> exit;
    [System.NonSerialized]
    public attackDelegation.attackSystem c;
    [SerializeField] public wolfStateMachine wSM;
    [SerializeField] public bool processingAttack;
    //[SerializeField] public GameObject visTell;
    [SerializeField] public GameObject visMask;
    [SerializeField] public SpriteRenderer visSprite;

    

   

    public void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, wSM.faceDir.Value);
    }

    

    

    public void Initialized(attackDelegation.attackSystem dele)
    {
        c = dele;
        SpriteRenderer ff = c.c.attackGobj.GetComponent<SpriteRenderer>();
        visSprite.transform.localScale = new Vector2(ff.sprite.rect.width/ff.sprite.pixelsPerUnit, 0);
        visSprite.transform.localPosition = new Vector2(0, visSprite.transform.localScale.y/2);
    }
}
