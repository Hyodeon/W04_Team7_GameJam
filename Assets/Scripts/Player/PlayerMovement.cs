using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private float turnSpeed = 180f;
    private Rigidbody rb;
    private bool onGround = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        Vector3 newVelocity = transform.forward * move * 100;
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void ChickenJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.Space) && rb.velocity.y < 0)
        {
            ChikenGlide(); //Test
        }
    }
    
    void ChikenGlide() //Test
    {
        rb.velocity = new Vector3(rb.velocity.x, -1f, rb.velocity.z);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

}
