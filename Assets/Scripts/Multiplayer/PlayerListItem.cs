using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject masterImage;

    private Player _player;

    public void SetUp(Player player)
    {
        _player = player;
        playerNameText.text = _player.NickName;
        masterImage.SetActive(_player.IsMasterClient);
    }

    public void IsMaster(Player player)
    {
        _player = player;
        masterImage.SetActive(_player.IsMasterClient);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
