using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float Speed = 1;
    public float JumpForce = 10;

    public LayerMask GroundLayer = 0;

    bool isGrounded
    {
        get
        {
            Vector3 bottomCenter = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z);
            return Physics.CheckCapsule(playerCollider.bounds.center, bottomCenter, playerCollider.bounds.size.x / 2 * 0.9f, GroundLayer);
        }
    }

    Vector3 movementDirection
    {
        get
        {
            float leftRight = Input.GetAxis("Horizontal");
            float forwardBackward = Input.GetAxis("Vertical");
            return new Vector3(leftRight, 0, forwardBackward);
        }
    }

    Rigidbody rigidBody;
    Collider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        rigidBody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        if (GroundLayer == gameObject.layer)
        {
            Debug.LogError("Player SortingLayer must be different from Ground SourtingLayer!");
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        rigidBody.AddForce(movementDirection * Speed, ForceMode.Impulse);
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector3.up * JumpForce);
    }
}
