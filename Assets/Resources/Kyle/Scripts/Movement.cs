using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float hSpeed = 5;
    [SerializeField] private float vSpeed = 10;
    public Rigidbody2D rBody2D;
    [SerializeField] private bool canMove = true;

    private bool facingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float hMove, float vMove, bool jump)
    {
        if(canMove)
        {
            Vector3 targetVelocity = new Vector2(hMove * hSpeed, vMove * vSpeed);

            rBody2D.velocity = Vector3.SmoothDamp(rBody2D.velocity, targetVelocity, ref velocity, movementSmooth);

            // Rotate to face either way depending on button
            if(hMove > 0 && !facingRight)
            {
                Flip();
            }
            else if(hMove < 0 && facingRight)
            {
                Flip();
            }

        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
