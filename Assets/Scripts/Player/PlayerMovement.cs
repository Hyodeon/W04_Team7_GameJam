using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 180f;
    [SerializeField] private Rigidbody _rb;
    private bool _onGround = true;

    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;
    void Start()
    {
    }
    void FixedUpdate()
    {
        TankMove();
    }

    void Update()
    {
        ChickenJump();
    }

    void TankMove() // WS:Move, AD:Turn
    {
        float move = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * _turnSpeed * Time.deltaTime;

        if (move != 0 || turn != 0)
        {
            _animator.SetInteger(AnimationSettings.Walk, 1);
        }
        else
        {
            _animator.SetInteger(AnimationSettings.Walk, 0);
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
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            _animator.SetTrigger(AnimationSettings.Jump);
        }

        //if (Input.GetKey(KeyCode.Space) && _rb.velocity.y < 0)
        //{
        //    ChickenGlide(); //Test
        //}
    }

    void ChickenGlide() //Test
    {
        _rb.velocity = new Vector3(_rb.velocity.x, -1f, _rb.velocity.z);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

    }
#endif

}
