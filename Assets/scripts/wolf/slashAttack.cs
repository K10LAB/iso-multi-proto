using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class slashAttack : attackMod
{
    void OnEnable() 
    {
        startTelegraph();
    }

    void Update()
    {
        timePass();

        
        updateTelegraph();
    }



}