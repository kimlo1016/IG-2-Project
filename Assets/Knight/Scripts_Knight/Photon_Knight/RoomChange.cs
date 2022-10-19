using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomChange : MonoBehaviourPunCallbacks
{
    [Header("Buttons")]
    [SerializeField] private Button _room;

    /// <summary>
    /// �̵��ϰ� ���� ���ѹ� ����
    /// </summary>
    [SerializeField]
    private Defines.ESceneNumder _sceneNumber;

    private void Start()
    {
        _room.onClick.AddListener(() => { GoRoom(); });
    }

    private void GoRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel((int)_sceneNumber);
    }
}
