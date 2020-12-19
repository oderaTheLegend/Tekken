using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class TopHighscore
{
    public int highestScore;
    public string _id;
}

[System.Serializable]
public class HighscoreArrayClass
{
    public TopHighscore[] highscoreModel;
}

public class Highscore : MonoBehaviour
{
    public int score;
    public string id;

    [SerializeField] private int amount;

    public Text highscoreText;

    float lastUpdate;

    PhotonView pv;

    private void Start()
    {
        StartCoroutine(CheckHighscore());
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            pv.RPC("ChangeHighscore", RpcTarget.All);
        }
        else if (Mode.mode == Mode.Modes.Story)
        {
            ChangeHighscore();
        }

       
    }

    [PunRPC]
    void ChangeHighscore()
    {
        highscoreText.text = "Highscore: " + score;

        if (Time.time - lastUpdate >= 1f)
        {
            score += amount;
            lastUpdate = Time.time;
        }
    }

    void HighestScore(HighscoreArrayClass data)
    {
        if (highestScore > data.highscoreModel[1].highestScore)
        {
            data.highscoreModel[1].highestScore = highestScore;
        }
        else
        {
            highestScore = data.highscoreModel[1].highestScore;
        }
    }

    int highestScore = 4;

    IEnumerator CheckHighscore()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:10101/ChainSaw/UpdateHighscore/" + "5fdde1861f6e6061fc63ffe0" + "/" + highestScore))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                print(request.error);
            }
            else
            {
                HighscoreArrayClass data = JsonUtility.FromJson<HighscoreArrayClass>(request.downloadHandler.text);

            //    HighestScore(data);

                Debug.Log(highestScore);
            }
        }
    }
}