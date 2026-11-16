using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Unity.Netcode;

public class slashAttack : NetworkBehaviour
{
    public float lifeTime;

    void Update()
    {
        lifeTime-=Time.deltaTime;
        if(lifeTime<=0)Destroy(this);
    }



}