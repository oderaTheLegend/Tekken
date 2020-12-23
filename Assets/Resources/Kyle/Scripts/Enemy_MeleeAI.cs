using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeAI : MonoBehaviour
{
    public Transform rayCast;
    public float moveSpeed;
    public float raycastLength;
    public float stalkingDistance;
    public float maxStalkTimer;
    public float maxCooldownTime;
    public Vector3 offset;
    float attackDistance;

    LayerMask raycastMask;
    RaycastHit2D hit;
    GameObject target;
    Animator animCont;
    [SerializeField] float distance;
    [SerializeField] float stalkTimer;
    [SerializeField] float currentCooldownTime;
    [SerializeField] bool isAttacking;
    [SerializeField] bool inRange;
    [SerializeField] bool isCooldown;
    [SerializeField] private bool facingRight = true;
    Vector2 playerFlip;

    private Rigidbody2D rb2D;


    private void Awake()
    {
        animCont = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {   
        distance = Vector2.Distance(transform.position, target.transform.position);
        Move();
        Flip();

    }

    void Flip()
    {
        playerFlip = transform.position - target.transform.position;
        if (transform.position.x > target.transform.position.x && facingRight)
        {
            facingRight = !facingRight;
            //transform.position += Vector3.right * 20f;
            transform.Rotate(0, 180, 0);
        }
        else if (transform.position.x < target.transform.position.x && !facingRight)
        {
            facingRight = !facingRight;
            //transform.position += Vector3.right * -20f;
            transform.Rotate(0, 180, 0);
        }
    }

    void Move()
    {
        /*Vector2 newPosition = target.transform.position;
        newPosition.y = transform.position.y;
        newPosition.x = transform.position.x;*/

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        rb2D.MovePosition(transform.position);
        animCont.SetBool("isWalking", true);
        //Stalking();
    }

    void Attack()
    {

    }

    void Stalking()
    {
        stalkTimer -= Time.deltaTime;
        if (!animCont.GetCurrentAnimatorStateInfo(0).IsName("Enemy1_Attack"))
        {
            Vector2 newPosition = new Vector2(target.transform.position.x, transform.position.y);
            Vector3 stalkTargetPos = newPosition;
            stalkTargetPos.y = transform.position.y;
            stalkTargetPos.z = transform.position.z;
            transform.position = Vector2.MoveTowards(transform.position, stalkTargetPos, moveSpeed * Time.deltaTime);

            if (distance <= stalkingDistance)
            {
                stalkTimer = maxStalkTimer;
                if (stalkTimer < 0)
                {

                }
            }
        }
    }
}
