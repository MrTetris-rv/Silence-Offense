using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class MouseSensitivityController : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    private CinemachinePOV _pov;

    private void Start()
    {
        _pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (_pov == null)
        {
            Debug.LogError("CinemachinePOV is missing on the Virtual Camera. Ensure you are using the correct camera setup.");
            return;
        }

        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", _pov.m_HorizontalAxis.m_MaxSpeed);
        mouseSensitivityText.text = PlayerPrefs.GetString("MouseSensitivityText", Mathf.Round(sensitivitySlider.value).ToString());
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    private void OnSensitivityChanged(float newValue)
    {
        if (_pov != null)
        {
            _pov.m_HorizontalAxis.m_MaxSpeed = newValue;
            _pov.m_VerticalAxis.m_MaxSpeed = newValue;
            mouseSensitivityText.text = newValue.ToString(); 
            PlayerPrefs.SetFloat("MouseSensitivity", newValue);
            PlayerPrefs.SetString("MouseSensitivityText", Mathf.Round(newValue).ToString());
        }
    }

    private void OnDestroy()
    {
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }
}
