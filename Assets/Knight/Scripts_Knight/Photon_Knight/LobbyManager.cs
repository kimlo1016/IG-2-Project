using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _stateText;

    [SerializeField]
    private Button _startButton;

    private void Awake()
    {
        _stateText.text = "Ready...";

        _startButton.onClick.AddListener(() => { OnClickStartButton(); });

        PhotonNetwork.ConnectUsingSettings();

        DeactiveJoinButton();
    }

    public override void OnConnectedToMaster()
    {
        _stateText.text = "Ready!!!";
        Debug.Log("������ ���� ���� �Ϸ�");

        ActiveJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _stateText.text = "ReReady......";
        Debug.Log("������ ���� ������ ��");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        _stateText.text = "MainLobby";
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void OnClickStartButton()
    {
        _stateText.text = "Loading";

        Debug.Log("��ŸƮ ��ư Ŭ��");

        PhotonNetwork.JoinLobby();
    }

    private void DeactiveJoinButton()
    {
        _startButton.interactable = false;
    }

    private void ActiveJoinButton()
    {
        _startButton.interactable = true;
    }

}
