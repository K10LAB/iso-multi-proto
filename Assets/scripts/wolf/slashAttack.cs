using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class slashAttack : MonoBehaviour
{
    public UnityEvent<attackDelegation.attackSystem> exit;
    [System.NonSerialized]
    public attackDelegation.attackSystem c;

    public void endAttack()
    {
        exit.Invoke(c);
    }
    public void Initialized(attackDelegation.attackSystem dele)
    {
        c = dele;

        Debug.Log("Here is attack");
    }
    


}