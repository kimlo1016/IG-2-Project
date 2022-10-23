using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using SceneType = Defines.ESceneNumder;

public class LogInServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _loginButton;


    private void Awake()
    {
        _loginButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        _loginButton.interactable = true;
    }

    public void LogIn()
    {
        SceneManager.LoadScene((int)SceneType.StartRoom);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel((int)SceneType.StartRoom);
    }
}