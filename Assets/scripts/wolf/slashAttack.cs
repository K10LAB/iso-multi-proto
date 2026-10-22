using Unity.VisualScripting;
using UnityEngine;

public class slashAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject projectile{get; set;}
    public SpriteRenderer projectileSprite{get; set;}
    public float attackRange{get; set;}
    public float attackSpeed{get; set;}
    public float colliderWidth{get; set;}
    public float colliderLength{get; set;}
    public GameObject source{get; set;}
    public Vector2 rspawnPoint{get; set;}
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
