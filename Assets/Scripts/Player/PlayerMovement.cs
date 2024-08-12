using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 180f;
    [SerializeField] private Rigidbody _rb;
    private Vector3 _movementVec;
    private bool _onGround = true;


    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower;

    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;
    [Header("ground check")]
    [SerializeField] private Vector3 _feetOffset;
    [Header("Particles")]
    [SerializeField] private GameObject _movementParticle;

    // 레이어 마스크
    private LayerMask _groundLayer;

    protected override void Initialize()
    {
        base.Initialize();
        _groundLayer = GetCombinedLayerMask();
    }

    void FixedUpdate()
    {
        TankMove();
    }

    void Update()
    {
        ChickenJump();
    }

    //private void GetPlayerInput()
    //{
    //    _movementVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _moveSpeed * Time.deltaTime;
    //    _movementVec += new Vector3(0, _rb.velocity.y, 0);
    //}

    void TankMove() // WS:Move, AD:Turn
    {
        float move = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * _turnSpeed * Time.deltaTime;

        if ((move != 0 || turn != 0) && _onGround)
        {
            _animator.SetInteger(AnimationSettings.Walk, 1);
            _movementParticle.SetActive(true);
        }
        else
        {
            _animator.SetInteger(AnimationSettings.Walk, 0);
            _movementParticle.SetActive(false);
        }
        Vector3 newVelocity = transform.forward * move * 100;
        newVelocity.y = _rb.velocity.y;
        _rb.velocity = newVelocity;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }

    void ChickenJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {

            _movementParticle.SetActive(false);
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);

            GetComponent<PlayerBase>().PropagateJump();

            _animator.SetTrigger(AnimationSettings.Jump);
        }
    }


    private LayerMask GetCombinedLayerMask()
    {
        return (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("FixedObject") | (1 << LayerMask.NameToLayer("Cage")));
    }


    private bool IsGrounded()
    {
        // 바닥에 착지 여부를 판단하는 함수
        return Physics.CheckSphere(transform.position + _feetOffset, 0.25f, _groundLayer);
    }






#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _movementParticle = FindGameObjectInChildren("MovementParticle");
    }
#endif
}
