using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ComboTime();
        Moving();
        Attacks();
    }

    private void FixedUpdate()
    {
        characterMovement.Move(horizontalMovement, verticalMovement, false);
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

        if(gaugeCurrent == gaugeMax)
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
            if (Input.GetButtonDown("Attack"))
            {
                currentComboTimer = comboTimerReset;
                AttackFatigue();
                animatorCont.Play("Player 1 Attack");
                comboStep = 1;
                return;
            }
        }
        if(comboStep != 0)
        { 
            if(comboPossible)
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
        if(comboStep == 2)
        {
            if (Input.GetKey(KeyCode.J))
            {
                currentComboTimer = comboTimerReset;
                AttackFatigue();
                animatorCont.Play("Player 1 AttackA");
            }

        }
        if(comboStep == 3)
        {
            if (Input.GetKey(KeyCode.J))
            {
                currentComboTimer = comboTimerReset;
                AttackFatigue();
                animatorCont.Play("Player 1 AttackB");
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
        if(isAttacking)
        {
            currentComboTimer = comboTimerReset;
        }
    }
}
