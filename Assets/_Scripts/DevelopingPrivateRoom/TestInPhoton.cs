using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestInPhoton : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        try
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        catch
        {
            Debug.LogError("�� ���� ����");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        try
        {
            PhotonNetwork.LoadLevel("PrivateRoom_Interaction_Joker");
            Debug.Log(PhotonNetwork.CurrentRoom.Name);
        }
        catch
        {
            Debug.LogError("�� �ε� ����");
        }
    }
}
