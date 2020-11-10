using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSelectManager : MonoBehaviour
{
    public enum Mode { Training, Online }
    public Mode mode;

    public List<Sprite> characterSprite;
    public List<Sprite> characterLogo;
    public List<string> characterName;

    [Tooltip("Keep track of current character, so you are able to re-choose characters too")]
    public List<string> playableCharacter;

    public Image[] playerImages;
    public Text[] playerNames;

    public Image versus;

    [Tooltip("This is the square select thing. (Play the game, you'll see)")]
    public GameObject slotPos;

    [HideInInspector]
    public int currentCharacter;

    [Tooltip("Just a value to keep track of the random character number")]
    int randomPCharacter;

    [Tooltip("Just a value to keep track of the random character number")]
    int randomECharacter;

    [Tooltip("This is how long randomization lasts")]
    float tickTime = 5;

    [Tooltip("Begin picking random character")]
    bool startRandomize;
    bool enemySelected;

    [Tooltip("Use this int as the playable character")]
    public static int chosenCharacter;

    [Tooltip("Character enemy chose for randomization")]
    public static int enemyChosenCharacter;

    [Tooltip("Just to set CPU info in the beginning")]
    int x = 0;

    private void Start()
    {
        currentCharacter = 0;
    }

    private void Update()
    {
        PlayerDetails();
        ChooseCharacter();
        ChooseRandom();
        GameMode();
    }

    /*Functions*/
    #region
    void PlayerDetails()
    {
        playerImages[0].sprite = characterSprite[currentCharacter];
        playerNames[0].text = characterName[currentCharacter];
    }

    public void ChooseCharacter()
    {
        //Keyboard Input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (playerNames[0].text == "Random")
            {
                tickTime = 5;
                startRandomize = true;
            }
            else
            {
                enemySelected = true;
                SpriteLogoSlot.selected = true;

                StartCoroutine(Fade(false));

                chosenCharacter = currentCharacter;
                playableCharacter.Add(characterName[chosenCharacter]);

                if (playableCharacter.Count >= 2)
                {
                    playableCharacter.RemoveAt(0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playableCharacter.Count == 1)
            {
                tickTime = 5;
                SpriteLogoSlot.selected = false;
                StartCoroutine(Fade(true));
                playableCharacter.RemoveAt(0);
            }
        }
    }

    void ChooseRandom()
    {
        if (startRandomize)
        {
            tickTime -= 5 * Time.deltaTime;

            if (tickTime > 0)
            {
                randomPCharacter = Random.Range(0, 3);
                currentCharacter = randomPCharacter;
            }
            else
            {
                enemySelected = true;
                tickTime = 5;
                chosenCharacter = randomPCharacter;
                startRandomize = false;
                SpriteLogoSlot.selected = true;
                StartCoroutine(Fade(false));

                playableCharacter.Add(characterName[chosenCharacter]);

                if (playableCharacter.Count == 2)
                {
                    playableCharacter.RemoveAt(0);
                }
            }
        }       
    }

    void GameMode()
    {
        if (mode == Mode.Training)
        {
            if (enemySelected)
            {
                tickTime -= 5 * Time.deltaTime;

                if (tickTime > 0)
                {
                    x = 1;
                    randomECharacter = Random.Range(0, 3);
                    enemyChosenCharacter = randomECharacter;
                }
                else
                {
                    tickTime = 0;
                    enemySelected = false;
                    enemyChosenCharacter = randomECharacter;
                }
            }

            if (x != 1)
            {
                playerImages[1].sprite = characterSprite[3];
                playerNames[1].text = "Pick Your Fighter!";
                return;
            }

            playerImages[1].sprite = characterSprite[enemyChosenCharacter];
            playerNames[1].text = characterName[enemyChosenCharacter];
        }
    }

    IEnumerator Fade(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1f; i >= 0; i -= 2f * Time.deltaTime)
            {
                versus.color = new Color(versus.color.r, versus.color.g, versus.color.b, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += 1.5f * Time.deltaTime)
            {
                versus.color = new Color(versus.color.r, versus.color.g, versus.color.b, i);
                yield return null;
            }
        }
    }

    #endregion
}