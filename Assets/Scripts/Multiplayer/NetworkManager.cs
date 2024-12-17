using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private int maxPlayers = 10;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private Transform roomListContent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MainMenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        if (string.IsNullOrEmpty(roomNameInputField.text)) return;

        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions, TypedLobby.Default);

        MainMenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MainMenuManager.Instance.OpenMenu("room");
        roomNameText.text = roomNameInputField.text;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = $"Room creation failed:\n{message}";
        MainMenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MainMenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MainMenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Update");
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        foreach (var room in roomList)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
        }
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MainMenuManager.Instance.OpenMenu("loading");
    }
}
