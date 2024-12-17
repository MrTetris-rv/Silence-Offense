using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;

    private bool _playerIsReady = false;
    private float _fillSpeed = 0.2f;
    private float _progress = 0f;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerReady += OnPlayerReady;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerReady -= OnPlayerReady;
    }

    private void Start()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(UpdateProgressBar());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator UpdateProgressBar()
    {
        while (!_playerIsReady || _progress < 1f)
        {
            if (!_playerIsReady && _progress < 0.9f)
            {
                _progress += _fillSpeed * Time.deltaTime;
            }
            else if (_playerIsReady)
            {
                _progress = Mathf.MoveTowards(_progress, 1f, _fillSpeed * Time.deltaTime * 2f);
            }

            loadingSlider.value = _progress;
            yield return null; 
        }

        gameObject.SetActive(false);
        //Destroy(loadingScreen);
    }

    private void OnPlayerReady()
    {
        _playerIsReady = true;
    }
}
