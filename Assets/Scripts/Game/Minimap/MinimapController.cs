using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject miniMap;

    private void Awake()
    {
        miniMap.SetActive(true);
    }

    public void SetMiniMapActive(bool isActive)
    {
        if (miniMap != null) {
            miniMap.SetActive(isActive);
        }
    }
}
