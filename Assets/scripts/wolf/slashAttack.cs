using Unity.VisualScripting;
using UnityEngine;

public class slashAttack : MonoBehaviour
{
    public attackContext c;

    public void Initialized(attackDelegation.attackSystem dele)
    {
        c = dele.c;

        Debug.Log("Here is attack");
    }


}