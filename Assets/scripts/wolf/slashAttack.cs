using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class slashAttack : MonoBehaviour
{
    public UnityEvent<attackDelegation.attackSystem> Ffl;
    public attackDelegation.attackSystem fff;

    public void endAttack()
    {
        Ffl.Invoke(fff);
    }
    public void Initialized(attackDelegation.attackSystem dele)
    {
        fff = dele;

        Debug.Log("Here is attack");
    }
    


}