using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (var menuElement in menus)
        {
            if (menuElement.menuName == menuName)
            {
                OpenMenu(menuElement);
            }
            else if (menuElement.isOpen)
            {
                CloseMenu(menuElement);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        foreach (var menuElement in menus)
        {
            if (menuElement.isOpen)
            {
                CloseMenu(menuElement);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    //public void OnCreateAServer()
    //{
    //    SceneManager.LoadScene("Game");
    //}
    //public void OnJoinServer()
    //{

    //}
    //public void OnSettings()
    //{

    //}

    public void OnExit()
    {
        Application.Quit();
    }
}
