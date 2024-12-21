using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject loadingScreen;

    private bool _isOpenedMenu;
    private bool _isReadyPlayer = false;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerReady += InitializeMiniMap;
        Instantiate(loadingScreen);
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerReady -= InitializeMiniMap;
        _isReadyPlayer = false;
    }

    private void InitializeMiniMap()
    {
        _isReadyPlayer = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_isReadyPlayer && Input.GetKeyUp(KeyCode.Escape))
        {
            _isOpenedMenu = !_isOpenedMenu;
            SetMenuState(_isOpenedMenu);
        }
    }

    //public void OpenMenu()
    //{

    //}

    private void SetMenuState(bool isOpened)
    {
        //if (_isOpenedMenu) {
        //    MainMenuManager.Instance.OpenMenu("menuSettings");
        //}
        ////else {
        ////    MainMenuManager.Instance.CloseMenu();
        ////}

        panelMenu.SetActive(isOpened);
        gameUI.SetActive(!isOpened);

        Cursor.lockState = isOpened ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isOpened;
    }

    public void OnSettings()
    {

    }

    public void OnBackToMenu()
    {
        if (_isOpenedMenu && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Disconnect");
        }
    }
}

