using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TempStart : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI _infoText;
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        _infoText.text = "���� ����";
    }

    public override void OnConnectedToMaster()
    {
        _infoText.text = "���� �����";
        PhotonNetwork.JoinLobby();
    }

    private RoomOptions _roomOption = new RoomOptions
    {
        MaxPlayers = 30
    };
    public override void OnJoinedLobby()
    {
        _infoText.text = "�κ� �����";
        PhotonNetwork.JoinOrCreateRoom(Defines.ESceneNumder.StartRoom.ToString(),
            _roomOption, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        _infoText.text = "�� �����";
        PhotonNetwork.LoadLevel((int)Defines.ESceneNumder.StartRoom);
    }
}
