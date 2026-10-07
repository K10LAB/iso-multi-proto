using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class enemyAi : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] private GameObject playerChased;

    [SerializeField] private NavMeshAgent agentNav;

    [SerializeField] private facingCamVis agentVis;
    [SerializeField] private GameObject playerChased = null;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float chaseSpeed;

    //[SerializeField] private gameManager gM;

    void Start()
    {
        //gM = GameObject.Find("GameManager").GetComponent<gameManager>();
        agentNav = GetComponent<NavMeshAgent>();
        agentVis = GetComponent<facingCamVis>();
    }

    // Update is called once per frame
    void Update()
    {
        chasePlayer();
        visChange();
    }

    void chasePlayer()
    {
        
        float closeDistance = chaseDistance;
        foreach (var clientgObj in NetworkManager.Singleton.ConnectedClients)
        {
            Transform playerObject = clientgObj.Value.PlayerObject.transform;

            Vector3 directionToPlayer = playerObject.position - transform.position;

            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer < closeDistance)
            {
                playerChased = playerObject.gameObject;
                closeDistance = distanceToPlayer;
            }
        }
        agentNav.SetDestination(playerChased?.transform.position ?? transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    void visChange()
    {
        Vector3 directionToPlayer = playerChased.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        Debug.Log(lookRotation.eulerAngles.y);

        
        if (lookRotation.eulerAngles.y >= 0 && lookRotation.eulerAngles.y < 90)
        {
            agentVis.faceDir.Value = 0;
        } else if (lookRotation.eulerAngles.y >= 90 && lookRotation.eulerAngles.y < 180)
        {
            agentVis.faceDir.Value = 3;
        } else if (lookRotation.eulerAngles.y >= 180 && lookRotation.eulerAngles.y < 270)
        {
            agentVis.faceDir.Value = 2;
        } else if (lookRotation.eulerAngles.y >= 270 && lookRotation.eulerAngles.y < 360)
        {
            agentVis.faceDir.Value = 1;
        }
    }
}
