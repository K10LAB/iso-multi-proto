using UnityEngine;
using Unity.Netcode;

public class enemyAi : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform playerChased;

    [SerializeField] private gameManager gM;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ulong clientId in gM.playerObjects)
        {
            Transform playerObject = gM.getClientPlayerObject(clientId).transform;

            Vector3 directionToPlayer = playerObject.position - transform.position;

            float distanceToPlayer = directionToPlayer.magnitude;

            if (playerObject != null)
            {
                // Do something with the playerObject
                Debug.Log($"Player Object for Client ID {clientId}: {playerObject.name}");
            }
        }
    }
}
