using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Vector3 destination = _player.transform.position - _player.transform.forward * (2f);
        _agent.destination = destination;
    }
}
