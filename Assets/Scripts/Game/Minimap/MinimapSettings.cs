using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSettings : MonoBehaviour
{
    private Transform _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, _mainCamera.eulerAngles.y, 0f);
    }
}
