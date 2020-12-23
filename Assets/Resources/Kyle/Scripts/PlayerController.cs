using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    Animator animatorCont;
    [SerializeField] private Movement characterMovement;
    float horizontalMovement;
    float verticalMovement;
    [SerializeField] float currentComboTimer;
    public float comboTimerReset = 1;
    float attackButton;
    float specialButton;
    public int gaugeMax = 100;
    [SerializeField] int gaugeCurrent;
    bool isAttacking = false;
    [SerializeField] bool comboPossible;
    [SerializeField] int comboStep;
    [SerializeField] int dragDefault = 2;
    [SerializeField] int attackFatigueDrag = 75;


    Vector3 mobileDir;
    // Start is called before the first frame update
    void Awake()
    {
        animatorCont = GetComponent<Animator>();
        characterMovement = GetComponent<Movement>();
    }

    private void Start()
    {
        currentComboTimer = comboTimerReset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                ComboTime();
                Moving();
                Attacks();
            }
        }
        else
        {
            ComboTime();
            Moving();
            Attacks();
        }
    }

    Vector3 dir;
    private void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            mobileDir = GameManager.instance.joystick.inputDirection;
        }
        else
        {
            dir = new Vector3(horizontalMovement, verticalMovement, 0);
        }

        if (Mode.mode == Mode.Modes.Online)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    characterMovement.Move(mobileDir.x, mobileDir.y, false);
                }
                else
                {
                    characterMovement.Move(dir.x, dir.y, false);
                }
            }
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                characterMovement.Move(mobileDir.x, mobileDir.y, false);
            }
            else
            {
                characterMovement.Move(dir.x, dir.y, false);
            }
        }
    }

    void Moving()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            animatorCont.SetBool("isWalking", true);
        }
        else
        {
            animatorCont.SetBool("isWalking", false);
        }
    }

    void Attacks()
    {
        attackButton = Input.GetAxis("Attack");
        specialButton = Input.GetAxis("Special");

        if (gaugeCurrent == gaugeMax)
        {
            if (Input.GetButtonDown("Special"))
            {
                AttackFatigue();
                animatorCont.Play("Player 1 Special");
            }
        }


        //  ------------------------------------------ Attack Combo ---------------------------------
        if (comboStep == 0)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (GameManager.instance.joystick.attack)
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 Attack");
                    comboStep = 1;
                    GameManager.instance.joystick.attack = false;
                    return;
                }
            }
            else
            {
                if (Input.GetButtonDown("Attack"))
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 Attack");
                    comboStep = 1;
                    return;
                }
            }
          
        }
        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }

    }
    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (GameManager.instance.joystick.attack)
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 AttackA");
                    GameManager.instance.joystick.attack = false;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.J))
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 AttackA");
                }
            }

        }

        if (comboStep == 3)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (GameManager.instance.joystick.attack)
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 AttackB");
                    GameManager.instance.joystick.attack = false;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.J))
                {
                    currentComboTimer = comboTimerReset;
                    AttackFatigue();
                    animatorCont.Play("Player 1 AttackB");
                }
            }         
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    public void CureAttackFatigue()
    {
        characterMovement.rBody2D.drag = dragDefault;
    }

    public void AttackFatigue()
    {
        characterMovement.rBody2D.drag = attackFatigueDrag;
    }

    public void ComboTime()
    {
        currentComboTimer -= Time.deltaTime;
        if (currentComboTimer <= 0)
        {
            comboStep = 0;
            currentComboTimer = comboTimerReset;
        }
        if (isAttacking)
        {
            currentComboTimer = comboTimerReset;
        }
    }
}
