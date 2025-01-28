using Cinemachine;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class CameraSettings : MonoBehaviourPunCallbacks
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _pov;

    private void Start()
    {
        _virtualCamera = GameObject.FindGameObjectWithTag("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (_pov == null)
        {
            Debug.LogError("CinemachinePOV is missing on the Virtual Camera. Ensure you are using the correct camera setup.");
            return;
        }

        if (photonView.IsMine)
        {
            _pov.VirtualCamera.Follow = this.transform;
        }
    }
}
