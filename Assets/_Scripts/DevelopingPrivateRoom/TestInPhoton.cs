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
            Debug.LogError("방 입장 실패");
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
            Debug.LogError("씬 로드 실패");
        }
    }
}
