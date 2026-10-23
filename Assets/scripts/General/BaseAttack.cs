using UnityEngine;

public  class BaseAttack
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SpriteRenderer projectileSprite{get; set;}
    public float attackRange{get; set;}
    public float attackSpeed{get; set;}
    
    public GameObject source{get; set;}
    public Vector2 spawnPoint{get; set;}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
