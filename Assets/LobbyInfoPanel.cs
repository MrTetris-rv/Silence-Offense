using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ConnectionStatusText;

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }
}
