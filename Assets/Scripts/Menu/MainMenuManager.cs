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
                menuElement.Open();
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

    //public void OnSettings()
    //{

    //}
}
