using Photon.Pun;
using TMPro;
using UnityEngine;

public class NetworkingSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pingInfoText;

    private void FixedUpdate()
    {
        pingInfoText.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }
}
