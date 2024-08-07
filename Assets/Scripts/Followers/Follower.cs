using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private NavMeshAgent _agent;
    [Header("Player")]
    public GameObject player;
    [Header("MoveTowards to")]
    public Transform FollowObject;
    [Header("Attack to")]
    public GameObject attackTarget;
    private Rigidbody rb;
    private bool isAttack = false;
    [Header("Jump")]
    public float jumpForce = 5f; // 점프 힘을 조절할 수 있는 변수
    [Header("Attack")]
    public float attackForce = 20f; // 공격 힘을 조절할 수 있는 변수

    private enum Mode
    {
        NavMesh,
        MoveTowards
    }
    [SerializeField] private Mode forceMode;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isAttack)
        {
            FollowPlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(AttackTarget());
        }
    }

    public void FollowPlayer()
    {
        if(forceMode == Mode.NavMesh)
        {
            _agent.enabled = true;
            rb.isKinematic = true;
            Vector3 destination = player.transform.position - player.transform.forward * 2f;
            _agent.destination = destination;
            _agent.speed = 5f;
        }


        else if(forceMode == Mode.MoveTowards)
        {
            _agent.enabled = false;
            rb.isKinematic = false;
            Vector3 destination = FollowObject.transform.position;
            Vector3 distance = destination - transform.position;
            if(distance.magnitude > 2f)
                transform.position = Vector3.MoveTowards(transform.position, destination, 5f * Time.deltaTime);
        }
    }

    IEnumerator AttackTarget()
    {
        if(forceMode == Mode.NavMesh)
        {
            isAttack = true;

            _agent.destination = attackTarget.transform.position;
            _agent.speed = 10f;
            Vector3 distance = attackTarget.transform.position - transform.position;
            while (distance.magnitude > 10f)
            {
                distance = attackTarget.transform.position - transform.position;
                yield return null;
            }

            //to AddForce, navmeshagent should be disabled and rigidbody should be enabled
            _agent.enabled = false;
            rb.isKinematic = false;


            //Randomize Chicken
            jumpForce *= Random.Range(0.5f, 1.5f);
            attackForce *= Random.Range(0.5f, 1.5f);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Vector3 direction = attackTarget.transform.position - transform.position;
            rb.AddForce(direction.normalized * attackForce, ForceMode.Impulse);

            while (!IsGrounded())
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);

            //end of attack
            rb.isKinematic = true;
            isAttack = false;
            _agent.enabled = true;
        }
        else if(forceMode == Mode.MoveTowards)
        {
            isAttack = true;
            Vector3 distance = attackTarget.transform.position - transform.position;

            float nextAttackForce = attackForce * Random.Range(0.5f, 2f);
            float nextJumpForce = jumpForce * Random.Range(1, 3f);

            float randDistance = Random.Range(5f, 10f);

            while (distance.magnitude > randDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, attackTarget.transform.position, 10f * Time.deltaTime);
                distance = attackTarget.transform.position - transform.position;
                yield return null;
            }
            rb.AddForce(distance.normalized * nextAttackForce, ForceMode.Impulse);
            rb.AddForce(Vector3.up * nextJumpForce, ForceMode.Impulse);

            while(!IsGrounded())
            {
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            isAttack = false;
        }
    }

    private bool IsGrounded()
    {
        // 바닥에 착지 여부를 판단하는 함수 (Raycast를 사용)
        return Physics.Raycast(transform.position, Vector3.down,0.5f);
    }
}