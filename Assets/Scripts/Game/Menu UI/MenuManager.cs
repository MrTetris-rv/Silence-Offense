using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject gameUI;

    private MinimapController minimapController;
    private bool _isOpenedMenu;
    private bool _isReadyPlayer = false;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerReady += InitializeMiniMap;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerReady -= InitializeMiniMap;
        _isReadyPlayer = false;
    }

    private void InitializeMiniMap(MinimapController controller)
    {
        minimapController = controller;
        if (minimapController != null)
        {
            minimapController.SetMiniMapActive(true);
        }

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

    private void SetMenuState(bool isOpened)
    {
        panelMenu.SetActive(isOpened);
        gameUI.SetActive(!isOpened);
        if (minimapController != null)
        {
            minimapController.SetMiniMapActive(!isOpened);
        }

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
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

