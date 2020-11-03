using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public float maxTimer = 5;
    private float timer;
    public float p1Health = 100;
    public float p2Health = 100;

    private bool winner;
    private bool startRound;
    private bool checkRounds;

    public Text timerText;
    public Text winnerText;
    public Slider p1Slider;
    public Slider p2Slider;

    public GameObject p1Content;
    public GameObject p1RoundAmount;
    public GameObject p2Content;
    public GameObject p2RoundAmount;
    private GameObject p1ClonedRoundAmount;
    private GameObject p2ClonedRoundAmount;

    public List<GameObject> p1AllClonedAmounts;
    public List<GameObject> p2AllClonedAmounts;

    private int p1RoundScore = 0;
    private int p2RoundScore = 0;
    private int howManyRounds;
    private int maxRounds;

    private void Start()
    {
        timer = maxTimer;
    }

    private void Update()
    {
        UpdateInfo();
        RoundUpdate();
    }

    public void RemoveRounds()
    {
        if (p1AllClonedAmounts.Count > 1)
        {
            p1AllClonedAmounts.Remove(p1AllClonedAmounts[0]);
            Destroy(p1AllClonedAmounts[0].gameObject);
        }
        if (p2AllClonedAmounts.Count > 1)
        {
            p2AllClonedAmounts.Remove(p2AllClonedAmounts[0]);
            Destroy(p2AllClonedAmounts[0].gameObject);
        }
    }

    public void ChooseRounds()
    {
        int maxRound = 7;

        if(maxRound <= 7)
        {
            maxRound++;
            p1ClonedRoundAmount = Instantiate(p1RoundAmount, p1RoundAmount.transform.position, Quaternion.identity);
            p1AllClonedAmounts.Add(p1ClonedRoundAmount);
            p1ClonedRoundAmount.transform.parent = p1Content.transform;
            p1ClonedRoundAmount.transform.localScale = new Vector3(1.7f, 1.7f, 0);

            p2ClonedRoundAmount = Instantiate(p2RoundAmount, p2RoundAmount.transform.position, Quaternion.identity);
            p2AllClonedAmounts.Add(p2ClonedRoundAmount);
            p2ClonedRoundAmount.transform.parent = p2Content.transform;
            p2ClonedRoundAmount.transform.localScale = new Vector3(1.7f, 1.7f, 0);

            howManyRounds++;
        }

    }

    public void StartGame()
    {
        startRound = true;
        checkRounds = true;

        winnerText.text = "";
        timer = maxTimer;
        maxRounds = howManyRounds;

        if (winner)
        {
            for (int i = 1; i < p1AllClonedAmounts.Count; i++)
            {
                p1AllClonedAmounts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int i = 1; i < p2AllClonedAmounts.Count; i++)
            {
                p2AllClonedAmounts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            winner = false;
        }
    }

    void UpdateInfo()
    {
        timerText.text = timer.ToString("0");

        if (startRound)
        {
            timer -= 1 * Time.deltaTime;
        }

        if (timer <= 0)
        {
            timer = 0;
            startRound = false;
        }

        p1Slider.value = p1Health;
        p2Slider.value = p2Health;
    }

    void RoundUpdate()
    {
        if (timer <= 0)
        {
            if (p1Health > p2Health)
            {
                if (checkRounds)
                {
                    p1RoundScore++;
                    p1AllClonedAmounts[p1RoundScore].transform.GetChild(0).gameObject.SetActive(true);
                    checkRounds = false;
                }

                if (p1RoundScore == maxRounds)
                {
                    winnerText.text = "Player 1 Wins";
                    winner = true;
                }
            }
            else if (p1Health < p2Health)
            {
                if (checkRounds)
                {
                    p2RoundScore++;
                    p2AllClonedAmounts[p2RoundScore].transform.GetChild(0).gameObject.SetActive(true);
                    checkRounds = false;
                }
                if (p2RoundScore == maxRounds)
                {
                    winnerText.text = "Player 2 Wins";
                    winner = true;
                }
            }
            else
            {
                if (checkRounds)
                {
                    p2RoundScore++; 
                    p1RoundScore++;
                    p2AllClonedAmounts[p2RoundScore].transform.GetChild(0).gameObject.SetActive(true);
                    p1AllClonedAmounts[p1RoundScore].transform.GetChild(0).gameObject.SetActive(true);
                    checkRounds = false;
                }
            }
        }
    }
}