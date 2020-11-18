using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerNumbering : MonoBehaviourPunCallbacks
{
    public static PlayerNumbering instance;

    public static Player[] SortedPlayers;

    public delegate void PlayerNumberingChanged();

    public static event PlayerNumberingChanged OnPlayerNumberingChanged;

    public const string RoomPlayerIndexedProp = "pNr";

    public void Awake()
    {
        instance = this;

        this.RefreshData();
    }

    public override void OnJoinedRoom()
    {
        this.RefreshData();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove(RoomPlayerIndexedProp);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        this.RefreshData();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        this.RefreshData();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps != null && changedProps.ContainsKey(RoomPlayerIndexedProp))
        {
            this.RefreshData();
        }
    }

    public void RefreshData()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() >= 0)
        {
            SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();
            if (OnPlayerNumberingChanged != null)
            {
                OnPlayerNumberingChanged();
            }
            return;
        }

        HashSet<int> usedInts = new HashSet<int>();
        Player[] sorted = PhotonNetwork.PlayerList.OrderBy((p) => p.ActorNumber).ToArray();

        string allPlayers = "all players: ";
        foreach (Player player in sorted)
        {
            allPlayers += player.ActorNumber + "=pNr:" + player.GetPlayerNumber() + ", ";

            int number = player.GetPlayerNumber();


            if (player.IsLocal)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if (!usedInts.Contains(i))
                    {
                        player.SetPlayerNumber(i);
                        break;
                    }
                }
                break;
            }
            else
            {
                if (number < 0) // check if that user has a number
                {
                    break;
                }
                else
                {
                    usedInts.Add(number);  // else remember used numbers
                }
            }
        }

        SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();

        OnPlayerNumberingChanged?.Invoke();
    }
}

public static class PlayerNumberingExtensions
{
    public static int GetPlayerNumber(this Player player)
    {
        if (player == null)
        {
            return -1;
        }

        if (PhotonNetwork.OfflineMode)
        {
            return 0;
        }
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            return -1;
        }

        object value;

        if (player.CustomProperties.TryGetValue(PlayerNumbering.RoomPlayerIndexedProp, out value))
        {
            return (byte)value;
        }

        return -1;
    }

    public static void SetPlayerNumber(this Player player, int playerNumber)
    {
        if (player == null)
        {
            return;
        }

        if (PhotonNetwork.OfflineMode)
        {
            return;
        }

        if (playerNumber < 0)
        {
            Debug.LogWarning("Setting invalid playerNumber: " + playerNumber + " for: " + player.ToStringFull());
        }

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("SetPlayerNumber was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
            return;
        }

        int current = player.GetPlayerNumber();

        if (current != playerNumber)
        {
            player.SetCustomProperties(new Hashtable() { { PlayerNumbering.RoomPlayerIndexedProp, (byte)playerNumber } });
        }
    }
}