using Unity.VisualScripting;
using UnityEngine;

public class slashAttack : MonoBehaviour
{
    public attackContext c;

    public void Start()
    {

        c = GetComponent<attackDelegation>().currentContext;
    }

    
}