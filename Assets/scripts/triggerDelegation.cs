using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class triggerDelegation : MonoBehaviour
{
    public Collider2D call;
    public bool isProcessing;
    void Awake()
    {
        call = GetComponent<Collider2D>();
        
    }
    
    public UnityEvent<OnTriggerDelegation> Enter;
    public UnityEvent<OnTriggerDelegation> Exit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isProcessing){Enter.Invoke(new OnTriggerDelegation(call, other)); }
    } 
    void OnTriggerExit2D(Collider2D other)
    {
        if(isProcessing){Exit.Invoke(new OnTriggerDelegation(call, other)); }
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
