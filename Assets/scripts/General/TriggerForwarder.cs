using UnityEngine;
using UnityEngine.Events;

public  class TriggerFowarder : MonoBehaviour
{
    [SerializeField] public bool isProcessing;
    public UnityEvent<Collider2D> Enter;
    public UnityEvent<Collider2D> Exit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isProcessing)Enter.Invoke(other);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(isProcessing)Exit.Invoke(other);
    }
}
