using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    bool hasSpottedPlayer = false;

    bool facingRight;

    float reactionTime = 50;

    float attackTimer = 100;

    public int hp = 10;

    public EnemyState currentState;

    public GameObject playerReference;

    public float attackBreak = 0.6f;

    Animator anim;

    public GameObject punchTrigger;

    InputState inputState;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = EnemyState.initializing;
    }

    public enum EnemyState
    {
        initializing,
        idle,
        sawPlayer,
        chasing,
        attacking
    }

    public void Update()
    {
        switch (currentState)
        {
            case EnemyState.initializing:
                playerReference = GameObject.FindGameObjectWithTag("Player");
                currentState = EnemyState.idle;
                break;
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.sawPlayer:
                SawPlayer();
                break;
            case EnemyState.chasing:
                Chasing();
                break;
            case EnemyState.attacking:
                Attacking();
                break;
            default:
                break;
        }
    }

    public void Idle()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < 5)
        {
            currentState = EnemyState.sawPlayer;
        }
    }

    public void SawPlayer()
    {
        if (!hasSpottedPlayer)
        {
            hasSpottedPlayer = true;
        }

        if (reactionTime < 0)
        {
            reactionTime = 50;
            currentState = EnemyState.chasing;
        }
        {
            reactionTime -= 1;
        }
    }

    public void Chasing()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) > 1.5f)
        {
            if (playerReference.transform.position.x < transform.position.x && !facingRight)
                Flip();
            if (playerReference.transform.position.x > transform.position.x && facingRight)
                Flip();

            anim.SetBool("Walk", true);
            Vector3 moveTowards = playerReference.transform.position - transform.position;
            moveTowards.Normalize();
            transform.position += moveTowards * 4 * Time.deltaTime;
        }
        else
        {
            anim.SetBool("Walk", false);
            currentState = EnemyState.attacking;
        }
    }

    public void Attacking()
    {
        attackBreak -= 1 * Time.deltaTime;

        if (Vector3.Distance(transform.position, playerReference.transform.position) <= 1.5f)
        {
            StartCoroutine(AttackTime());
        }

        if (attackTimer < 0)
        {
            currentState = EnemyState.chasing;
        }
        else
        {
            attackTimer -= 1;
        }

        if(GameManager.i.p1Health.value <= 0)
        {
            currentState = EnemyState.idle;
        }
    }

    IEnumerator AttackTime()
    {
        while (attackBreak <= 0)
        {
            StartCoroutine(ColliderSwitch());
            anim.SetTrigger("Light");
            attackBreak = 0.6f;
            yield return null;
        }
    }

    IEnumerator ColliderSwitch()
    {
        punchTrigger.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        punchTrigger.SetActive(false);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
    public void WaitResetInputState(float time)
    {
        StartCoroutine(RecoveryWaiter(time));
    }
    IEnumerator RecoveryWaiter(float time)
    {
        inputState = InputState.Recovery;
        yield return new WaitForSeconds(time);
        inputState = InputState.Neutral;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(playerReference.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}