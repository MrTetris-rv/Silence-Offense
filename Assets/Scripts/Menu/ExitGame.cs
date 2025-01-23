using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void DoExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
