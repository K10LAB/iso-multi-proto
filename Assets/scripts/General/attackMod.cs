using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Unity.VisualScripting;
[System.Serializable]
public class attackMod : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public attackDelegation c;
    

    
    public float timeBeforeAttack;
    public float timeBeforeTelegraph;
    public float currentTime;
    //[SerializeField] public GameObject visTell;
    public GameObject visMask;
    public SpriteRenderer visSprite;
    public Sprite teleSprite;

    
    public void updateTelegraph()
    {
        currentTime += Time.deltaTime;
        if(currentTime<0)return;
        transform.localRotation = Quaternion.Euler(0, 0, c.currentFaceDir);
        visMask.transform.localPosition = new Vector2(0, -visSprite.transform.localScale.y * currentTime/timeBeforeAttack - 1);
        
    }
    public void OnEnable()
    {
        visSprite.sprite = teleSprite;
        
        currentTime = -timeBeforeTelegraph;
        transform.localScale = new Vector2(visSprite.sprite.rect.width/visSprite.sprite.pixelsPerUnit, 0);
        transform.localPosition = new Vector2(0, visSprite.transform.localScale.y/2);
    }



    

    
}
