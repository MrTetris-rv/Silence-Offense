using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItems : MonoBehaviour
{
    [MenuItem("Scenes/Main Menu")]
    static void MainMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scenes/Game")]
    static void Game()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
    }
}