using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI numberPlayersText;

    private RoomInfo _roomInfo;

    public void SetUp(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        roomNameText.text = roomInfo.Name;
        numberPlayersText.text = $"{roomInfo.PlayerCount}/10";
    }

    public void OnClick()
    {
        NetworkManager.Instance.JoinRoom(_roomInfo);
    }
}
