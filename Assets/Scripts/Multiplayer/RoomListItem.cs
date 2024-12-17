using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;

    private RoomInfo _roomInfo;

    public void SetUp(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        roomNameText.text = roomInfo.Name;
    }

    public void OnClick()
    {
        NetworkManager.Instance.JoinRoom(_roomInfo);
    }
}
