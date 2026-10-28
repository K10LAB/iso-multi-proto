using Unity.VisualScripting;
using UnityEngine;

public class slashAttack : MonoBehaviour
{
    public attackContext c;

    public void Intialize(attackDelegation.attackSystem dele)
    {
        c = dele.c;
    }
    

    
}