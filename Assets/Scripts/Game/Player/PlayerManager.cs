using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(_photonView.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        //Instantiate(_photonView);
    }
}
