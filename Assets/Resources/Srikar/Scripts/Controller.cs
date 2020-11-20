using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public enum JumpState
{
    Grounded,
    Jumping,
    Falling
}

[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviourPun
{
    protected Rigidbody rigidbody;

    [Header("Jump Values")]
    [SerializeField] protected float airTime = 1f;
    [SerializeField] protected float jumpHeight = 1f;
    [SerializeField] protected float groundAllowance = 0;
    protected float gravity;
    protected JumpState jumpState;

    [Header("Movement Values")]
    [SerializeField] protected float speedUpFactor = 1f;
    [SerializeField] protected float movementSpeed = 6f;
    protected float appliedSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        SetGravity();

        jumpState = JumpState.Grounded;
    }

    /// <summary>
    /// Used in Fixed Update
    /// </summary>
    /// <param name="dir"></param>
    protected virtual void Move(Vector3 dir, out bool move)
    {
        Vector3 currentVel = rigidbody.velocity;
        currentVel.y = 0;

        if (currentVel.magnitude <= 0.2f)
            appliedSpeed = 0;

        float interpolation = speedUpFactor * Time.deltaTime;

        appliedSpeed = Mathf.Lerp(appliedSpeed, movementSpeed, interpolation);

        Vector3 velocity = dir.normalized * appliedSpeed;

        float vertVal = rigidbody.velocity.y;

        rigidbody.velocity = Vector3.forward * velocity.x;
        rigidbody.velocity += Vector3.right * velocity.z;

        rigidbody.velocity += Vector3.up * vertVal;

        if (velocity.magnitude > 0)
            move = true;
        else
            move = false;
    }

    /// <summary>
    /// Used in Update
    /// </summary>
    virtual protected void GroundCheck()
    {
        if (jumpState != JumpState.Jumping)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, groundAllowance))
            {
                if (jumpState == JumpState.Falling)
                {
                    jumpState = JumpState.Grounded;
                }
            }
            else
            {
                //StartCoroutine(JumpStateWaiter(JumpState.Falling, 0.1f));
                jumpState = JumpState.Falling;
            }
        }
    }

    /// <summary>
    /// Mention function condition
    /// </summary>
    protected virtual void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }

    /// <summary>
    /// Use in Fixed Update
    /// </summary>
    protected virtual void Gravity()
    {
        rigidbody.velocity += Vector3.up * gravity;
    }

    protected void SetGravity()
    {
        gravity = -2f * jumpHeight / (airTime * airTime) * Time.deltaTime;
    }

    protected IEnumerator JumpStateWaiter(JumpState state, float time)
    {
        yield return new WaitForSeconds(time);
        jumpState = state;
    }

    protected IEnumerator JumpCoroutine()
    {
        jumpState = JumpState.Jumping;
        float jump = (2f * jumpHeight) / airTime;
        rigidbody.velocity += Vector3.up * jump;

        float t = 0;

        while (t <= airTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        jumpState = JumpState.Falling;
    }
}