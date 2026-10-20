using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class triggerDelegation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider2D collider;
    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }
    public UnityEvent<OnTriggerDelegation> Enter;
    public UnityEvent<OnTriggerDelegation> Exit;

    void OnTriggerEnter2D(Collider2D other) => Enter.Invoke(new OnTriggerDelegation(collider, other));
    void OnTriggerExit2D(Collider2D other) => Exit.Invoke(new OnTriggerDelegation(collider, other));
    public struct OnTriggerDelegation
    {
        public Collider2D Self { get; }
        public Collider2D Other { get; }

        public OnTriggerDelegation(Collider2D self, Collider2D other)
        {
            Self = self;
            Other = other;
        }
    }

    
}
