using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private NavMeshAgent _agent;
    public GameObject player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Vector3 destination = player.transform.position - player.transform.forward * (2f);
        _agent.destination = destination;
    }
}
