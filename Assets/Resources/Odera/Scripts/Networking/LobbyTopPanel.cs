using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopPanel : MonoBehaviour
{
    private string connectionStatusMessage = "Connection Status: ";

    public Text ConnectionStatusText;

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }
}