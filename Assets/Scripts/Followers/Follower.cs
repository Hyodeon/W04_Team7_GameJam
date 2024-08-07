using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Follower : MonoBehaviour
{

    #region 인스펙터 변수

    [Header("MoveTowards to")]
    public Transform FollowObject;

    [Header("Attack to")]
    public GameObject attackTarget;

    [Header("Jump")]
    public float jumpForce = 5f; // 점프 힘을 조절할 수 있는 변수

    [Header("Attack")]
    public float attackForce = 20f; // 공격 힘을 조절할 수 있는 변수

    [Header("FeetPosition")]
    [SerializeField] private Vector3 _feetOffset;

    [Header("Animator Field")]
    [SerializeField] private Animator _animator;

    #endregion

    #region 지역 변수

    // 상태 저장
    private State _currentState;
    private State _prevState;

    // 레이어 마스크
    private LayerMask _groundLayer;

    // 이동 관련 변수
    private Vector3 _target;
    private float _speed;

    // 상태 Boolean
    private bool _isFollwing;
    private bool _isAttack;
    private bool _isJumping;
    private bool _isStateTransition;

    // 상태 Action
    private Dictionary<State, Action<bool>> _stateHandler;

    // Rotation 변수
    private float _radius;
    private float _angle;

    // Patrol 변수
    private float _elapsedTime;
    private float _delay;
    private bool _isPatrolling;

    // Components
    private Rigidbody _rb;

    // Objects
    private GameObject _player;

    // Detection Mode
    private bool _isFreeDetection;

    #endregion

    #region 인터페이스

    public float MoveSpeed { get { return _speed; } }

    #endregion

    public enum State
    {
        Idle = 0,
        Patrol = 1,
        Rotate = 2,
        Follow = 3,
        Attack = 4,
        Jump = 5,
    }

    #region 초기 설정

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // 초기 상태 설정
        _currentState = State.Idle;
        _prevState = _currentState;
        _isFollwing = false;
        _isAttack = false;
        _isJumping = false;
        _isStateTransition = true;
        _isPatrolling = false;
        _isFreeDetection = false;

        _speed = 1f;
        _radius = 0f;
        _angle = 0f;

        _groundLayer = LayerMask.GetMask("Ground");

        // 상태 핸들러에 액션 등록
        _stateHandler = new Dictionary<State, Action<bool>>()
        {
            { State.Idle, Idle },
            { State.Patrol, Patrol },
            { State.Rotate, Rotate },
            { State.Follow, Follow },
            { State.Attack, Attack },
            { State.Jump, Jump },
        };

        // 오브젝트, 컴포넌트 바인딩
        _player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();

    }

    #endregion

    private void FixedUpdate()
    {
        // 상태 핸들러에 등록된 액션 수행
        if (_stateHandler.TryGetValue(_currentState, out var action))
        {
            action(_isStateTransition);
        }
    }

    private void Idle(bool isTransition)
    {

        Vector3 distVector = _player.transform.position - transform.position;

        if (distVector.magnitude < 20f)
        {
            _player.GetComponent<PlayerBase>().AddFollower(gameObject);
            TriggerState(State.Follow);
        }
    }

    private void Patrol(bool isTransition)
    {
        if (isTransition)
        {
            _speed = 12f;
            _elapsedTime = 0f;
            _isStateTransition = false;
        }

        if (_elapsedTime > 2f && !_isPatrolling)
        {
            StartCoroutine(DoPatrol(GenerateRandomDirection()));
            _isPatrolling = true;
        }
        else _elapsedTime += Time.deltaTime;

        Debug.Log(_elapsedTime);
    }

    private IEnumerator DoPatrol(Vector3 dir)
    {
        float eTime = 0f;
        float duration = 0.3f;

        transform.rotation = Quaternion.LookRotation(dir);

        while(eTime < duration)
        {
            eTime += Time.deltaTime;
            transform.position += dir * Time.deltaTime * _speed;
            yield return null;
        }

        _isPatrolling = false;
        _elapsedTime = 0f;
    }

    private void Rotate(bool isTransition)
    {
        // TODO : 초기 _angle 설정 변경, 바라보는 방향 버그 수정

        if (isTransition)
        {
            _target = transform.position + transform.forward.normalized * 2f;
            _angle = 0f;
            _speed = 6f;
            _radius = 1.5f;
            _isStateTransition = false;
        }

        _angle += _speed * Time.deltaTime;

        float x = Mathf.Cos(_angle) * _radius;
        float z = Mathf.Sin(_angle) * _radius;

        Vector3 newPosition = _target + new Vector3(x, 0f, z);
        Vector3 lookDir = (newPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0f, lookDir.z));
        transform.position = newPosition;
    }

    private void Follow(bool isTransition)
    {
        if (isTransition)
        {
            _speed = 8f;
            _isStateTransition = false;
        }

        _animator.SetInteger(AnimationSettings.Walk, 1);

        _target = _player.GetComponent<PlayerBase>().FollowPoint;

        if ((transform.position - _player.transform.position).magnitude >= 50)
        {
            Debug.Log("Delete");
            _player.GetComponent<PlayerBase>().DeleteObejctFromList(gameObject);
            TriggerState(State.Idle);
        }

        _rb.isKinematic = false;

        Vector3 directionVector = _target - transform.position;
        Vector3 playerVector = _player.transform.position - transform.position;
        playerVector.y = 0f;
        transform.rotation = Quaternion.LookRotation(playerVector);

        if (_rb.velocity.magnitude < 2f)
        {
            _animator.SetInteger(AnimationSettings.Walk, 0);
        }

        if (directionVector.magnitude > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }
        
    }

    private void Attack(bool isTransition)
    {
        if (isTransition)
        {
            // 타겟 새로 설정할 필요 있음
            attackTarget = _player.gameObject;
            StartCoroutine(AttackTarget());
            _isStateTransition = false;
        }
    }

    private void Jump(bool isTransition)
    {
        if (!_isJumping) StartCoroutine(DoJump());
    }

    private IEnumerator DoJump()
    {
        _isJumping = true;
        _animator.SetTrigger(AnimationSettings.Jump);

        _rb.AddForce(Vector3.up * 12f, ForceMode.Impulse);

        while (true)
        {
            yield return null;

            if (IsGrounded()) break;
        }

        _isJumping = false;

        TriggerState(State.Follow);
    }

    public void TriggerState(State st)
    {
        Debug.Log($"State To {st}");

        _animator.SetInteger(AnimationSettings.Walk, 0);

        _isStateTransition = true;
        _prevState = _currentState;
        _currentState = st;

    }

    IEnumerator AttackTarget()
    {
        Debug.Log("공격!");

        _isAttack = true;
        Vector3 distance = attackTarget.transform.position - transform.position;

        float nextAttackForce = attackForce * Random.Range(0.5f, 2f);
        float nextJumpForce = jumpForce * Random.Range(1, 3f);

        float randDistance = Random.Range(5f, 10f);

        while (distance.magnitude > randDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, attackTarget.transform.position, 10f * Time.deltaTime);

            distance = attackTarget.transform.position - transform.position;
            yield return null;
        }

        _rb.AddForce(distance.normalized * nextAttackForce, ForceMode.Impulse);
        _rb.AddForce(Vector3.up * nextJumpForce, ForceMode.Impulse);

        while (!IsGrounded())
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        _isAttack = false;
    }

    private bool IsGrounded()
    {
        // 바닥에 착지 여부를 판단하는 함수
        return Physics.CheckSphere(transform.position + _feetOffset, 0.25f, _groundLayer);
    }

    void OnDrawGizmos()
    {
        if (_target != null)
        {
            Gizmos.color = Color.red;

            // 십자 표시 크기
            float crossSize = 0.5f;

            // X축 선
            Gizmos.DrawLine(_target + Vector3.left * crossSize, _target + Vector3.right * crossSize);

            // Y축 선
            Gizmos.DrawLine(_target + Vector3.down * crossSize, _target + Vector3.up * crossSize);

            // Z축 선
            Gizmos.DrawLine(_target + Vector3.back * crossSize, _target + Vector3.forward * crossSize);


            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + _feetOffset, 0.25f);
        }
    }

    Vector3 GenerateRandomDirection()
    {
        // 0에서 360도 사이의 랜덤한 각도 선택
        float randomAngle = Random.Range(0f, 360f);
        float radianAngle = randomAngle * Mathf.Deg2Rad;

        // XZ 평면에서의 단위 벡터 계산
        float x = Mathf.Cos(radianAngle);
        float z = Mathf.Sin(radianAngle);

        // Y값을 0으로 하여 벡터 생성
        return new Vector3(x, 0f, z).normalized;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            _player.GetComponent<PlayerBase>().DeleteObejctFromList(gameObject);
        }
    }
}


#region 이전 코드
//IEnumerator AttackTarget()
//{
//    if (forceMode == Mode.NavMesh)
//    {
//        _isAttack = true;

//        _agent.destination = attackTarget.transform.position;
//        _agent.speed = 10f;
//        Vector3 distance = attackTarget.transform.position - transform.position;
//        while (distance.magnitude > 10f)
//        {
//            distance = attackTarget.transform.position - transform.position;
//            yield return null;
//        }

//        //to AddForce, navmeshagent should be disabled and rigidbody should be enabled
//        _agent.enabled = false;
//        _rb.isKinematic = false;


//        //Randomize Chicken
//        jumpForce *= Random.Range(0.5f, 1.5f);
//        attackForce *= Random.Range(0.5f, 1.5f);

//        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        Vector3 direction = attackTarget.transform.position - transform.position;
//        _rb.AddForce(direction.normalized * attackForce, ForceMode.Impulse);

//        while (!IsGrounded())
//        {
//            yield return null;
//        }
//        yield return new WaitForSeconds(1f);

//        //end of attack
//        _rb.isKinematic = true;
//        _isAttack = false;
//        _agent.enabled = true;
//    }
//    else if (forceMode == Mode.MoveTowards)
//    {
//        _isAttack = true;
//        Vector3 distance = attackTarget.transform.position - transform.position;

//        float nextAttackForce = attackForce * Random.Range(0.5f, 2f);
//        float nextJumpForce = jumpForce * Random.Range(1, 3f);

//        float randDistance = Random.Range(5f, 10f);

//        while (distance.magnitude > randDistance)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, attackTarget.transform.position, 10f * Time.deltaTime);
//            distance = attackTarget.transform.position - transform.position;
//            yield return null;
//        }
//        _rb.AddForce(distance.normalized * nextAttackForce, ForceMode.Impulse);
//        _rb.AddForce(Vector3.up * nextJumpForce, ForceMode.Impulse);

//        while (!IsGrounded())
//        {
//            yield return null;
//        }
//        yield return new WaitForSeconds(2f);
//        _isAttack = false;
//    }
//}

//private enum Mode
//{
//    NavMesh,
//    MoveTowards
//}

//if (!_isAttack)
//{
//    FollowPlayer();
//}

//if (Input.GetKeyDown(KeyCode.E))
//{
//    StartCoroutine(AttackTarget());
//}



#endregion