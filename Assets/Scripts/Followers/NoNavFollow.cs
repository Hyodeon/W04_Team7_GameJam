using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoNavFollow : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    [SerializeField]private Transform FollowObject;
    [SerializeField]private GameObject AttackObject;
    private Rigidbody rb;
    private bool isAttack = false;
    [Header("Jump")]
    public float jumpForce = 5f; // 점프 힘을 조절할 수 있는 변수
    [Header("Attack")]
    public float attackForce = 20f; // 공격 힘을 조절할 수 있는 변수
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttack)
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
        Vector3 destination = FollowObject.transform.position;
        Vector3 distance = destination - transform.position;
        transform.LookAt(destination);
        if(distance.magnitude > 4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, 10f * Time.deltaTime);
            _animator.SetInteger("Walk", 1);
        }
        else
        {
            _animator.SetInteger("Walk", 0);
        }
    }

    IEnumerator AttackTarget()
    {
        isAttack = true;
        Vector3 distance = AttackObject.transform.position - transform.position;

        float nextAttackForce = attackForce * Random.Range(0.5f, 2f);
        float nextJumpForce = jumpForce * Random.Range(1, 3f);

        float randDistance = Random.Range(5f, 10f);

        while (distance.magnitude > randDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, AttackObject.transform.position, 10f * Time.deltaTime);
            distance = AttackObject.transform.position - transform.position;
            yield return null;
        }
        rb.AddForce(distance.normalized * nextAttackForce, ForceMode.Impulse);
        rb.AddForce(Vector3.up * nextJumpForce, ForceMode.Impulse);

        while(!isGrounded())
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        isAttack = false;
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }
}
