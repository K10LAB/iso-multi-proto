using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class enemyAi : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] private GameObject playerChased;

    [SerializeField] private NavMeshAgent navAgent;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private gameManager gM;

    void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<gameManager>();
        navAgent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        chasePlayer();
    }

    void chasePlayer()
    {
        GameObject playerChased = null;
        float closeDistance = chaseDistance;
        foreach (ulong clientId in gM.playerObjects)
        {
            Transform playerObject = gM.getClientPlayerObject(clientId).transform;

            Vector3 directionToPlayer = playerObject.position - transform.position;

            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer < closeDistance)
            {
                playerChased = playerObject.gameObject;
                closeDistance = distanceToPlayer;
            }
        }
        navAgent.SetDestination(playerChased?.transform.position ?? transform.position);
    }
}
