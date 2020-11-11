using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworking : MonoBehaviourPunCallbacks
{
    public GameObject playerOverviewEntryPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    public static PlayerNetworking i;

    public void Awake()
    {
        i = this;

        playerListEntries = new Dictionary<int, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerOverviewEntryPrefab);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().color = InGame.ColorOfTeam (p.GetPlayerNumber());
            entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", p.NickName, p.GetScore(), InGame.playerLives);

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