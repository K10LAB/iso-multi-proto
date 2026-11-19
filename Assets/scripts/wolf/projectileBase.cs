using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] public float range = 5f;

    private Rigidbody2D rb2d;

    public GameObject source;

    //private Damage damageComponent;

    void Awake()
    {
        //Rigidbody2D playerRb = FindAnyObjectByType<PlayerController>().GetComponent<Rigidbody2D>();
        rb2d = GetComponent<Rigidbody2D>();

        //if (playerRb != null)
            //Physics2D.IgnoreCollision(rb2d.GetComponent<Collider2D>(), playerRb.GetComponent<Collider2D>());
    }

    void Start()
    {
        //damageComponent = GetComponent<Damage>();

        //Destroy(gameObject, lifetime);
    }

    void Update()
    {
        range-= Time.deltaTime;
        if(range<0)Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == source)return;

        //if (damageComponent != null)
            //damageComponent.OnTriggerDamage(other);

        Destroy(gameObject);
    }

    public void SetProjectileDirection(Vector2 _projectileDirection)
    {
        Vector2 _linearVelocity = _projectileDirection * speed;

        rb2d.linearVelocity = _linearVelocity;
    }
}