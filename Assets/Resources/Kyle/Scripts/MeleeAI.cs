using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask raycastMask;
    public float raycastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;

    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private Rigidbody2D rb2D;
    private float distance;
    private bool facingRight;
    private Vector3 playerFlip;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;

    private void Awake()
    {
        intTimer = timer;
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(rayCast.position, Vector2.left, raycastLength, raycastMask);
        RaycastDebugger();

        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            inRange = true;
        }

        if(inRange == false)
        {
            anim.SetBool("isWalking", true);
            StopAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.green);
        }
    }

    void Flip()
    {
        playerFlip = transform.position - target.transform.position;
        if (transform.position.x > target.transform.position.x && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (transform.position.x < target.transform.position.x && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if(cooling)
        {
            Cooldown();
            anim.SetBool("isAttacking", false);
        }
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void Move()
    {
        anim.SetBool("isWalking", true);
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy1_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
            rb2D.MovePosition(transform.position);
        }
        Flip();
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("isAttacking", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }
}
