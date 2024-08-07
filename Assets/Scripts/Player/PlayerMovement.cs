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

    [Header("Particles")]
    [SerializeField] private GameObject _movementParticle;



    protected override void Initialize()
    {
        base.Initialize();
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
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {

            _movementParticle.SetActive(false);
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);

            GetComponent<PlayerBase>().PropagateJump();

            _animator.SetTrigger(AnimationSettings.Jump);
        }
    }

    void ChickenGlide() //Test
    {
        _rb.velocity = new Vector3(_rb.velocity.x, -1f, _rb.velocity.z);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
            _movementParticle.SetActive(false);
        }
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
