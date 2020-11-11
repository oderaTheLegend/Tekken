using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class OverviewPanel : MonoBehaviourPunCallbacks
{
    public GameObject PlayerOverviewEntryPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    public void Awake()
    {
        playerListEntries = new Dictionary<int, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().color =  InGame.ColorOfTeam(p.GetPlayerNumber());
            entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", p.NickName, p.GetScore(), InGame.ColorOfTeam((p.GetPlayerNumber())));

            playerListEntries.Add(p.ActorNumber, entry);
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", targetPlayer.NickName, targetPlayer.GetScore(), targetPlayer.CustomProperties[InGame.playerLives]);
        }
    }
}