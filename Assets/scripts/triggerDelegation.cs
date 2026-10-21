using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class triggerDelegation : MonoBehaviour
{
    [SerializeField]public Collider2D detectionCollider;
    [SerializeField]public Collider2D hitboxCollider;
    [SerializeField]public bool isProcessingDetection;
    [SerializeField]public bool isProcessingHitBox;
    
    public UnityAction<OnTriggerDelegation> Enter;
    public UnityAction<OnTriggerDelegation> Exit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isProcessingDetection)Enter.Invoke(new OnTriggerDelegation(detectionCollider, other)); 
        if(isProcessingHitBox)Enter.Invoke(new OnTriggerDelegation(hitboxCollider, other)); 
    } 
    void OnTriggerExit2D(Collider2D other)
    {
        if(isProcessingDetection)Exit.Invoke(new OnTriggerDelegation(detectionCollider, other));
        if(isProcessingHitBox)Exit.Invoke(new OnTriggerDelegation(hitboxCollider, other));
    } 

    
    public struct OnTriggerDelegation
    {
        public Collider2D Self { get; private set;}
        public Collider2D Other { get; private set;}

        public OnTriggerDelegation(Collider2D self, Collider2D other)
        {
            Self = self;
            Other = other;
        }
    }

    
}
