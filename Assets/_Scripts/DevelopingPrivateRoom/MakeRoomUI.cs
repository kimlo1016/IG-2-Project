using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using _UI = Defines.EPrivateRoomUIIndex;

public class MakeRoomUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] PrivateRoomUIManager _uiManager;

    [Header("Button")]
    [SerializeField] Button _makeRoomButton;
    [SerializeField] Button _closeButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _roomNameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _roomNumberInput;

    [Space(15)]
    [SerializeField] Toggle _passwordToggle;

    private Photon.Realtime.RoomOptions _roomOptions;

    private void Awake()
    {
        _roomOptions.IsVisible = true;
        _roomOptions.IsOpen = true;
        _roomOptions.PublishUserId = true;
    }

    private void OnEnable()
    {
        _makeRoomButton.onClick.RemoveListener(RequestMakeRoom);
        _makeRoomButton.onClick.AddListener(RequestMakeRoom);

        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);

        _passwordToggle.isOn = false;
        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
        _passwordToggle.onValueChanged.AddListener(ActivePasswordInput);
    }

    public void RequestMakeRoom()
    {
        try
        {
            // �� ������ ȣ��Ʈ�� ��û?

            MakeRoom();
            // PhotonNetwork.JoinRoom("�� �̸�");
        }
        catch
        {
            Debug.LogError("�� ���� ��û ����");
        }
    }

    private void MakeRoom()
    {
        string roomName = _roomNameInput.text + "_" + _passwordInput.text;

        try
        {
            _roomOptions.MaxPlayers = byte.Parse(_roomNumberInput.text);

            _roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { "roomname", _roomNameInput.text },
                { "password", _passwordInput.text },
                { "displayname", _roomNameInput.text }
            };
            _roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                "roomname",
                "password",
                "displayname"
            };
            PhotonNetwork.CreateRoom(roomName, _roomOptions, null);
        }
        catch 
        {
            Debug.LogError("�� ���� ����");
        }
    }

    private void ActivePasswordInput(bool isOn)
    {
        _passwordInput.interactable = isOn;
        
        if(!_passwordInput.IsInteractable())
        {
            _passwordInput.text = "";
        }
    }

    private void Close() => _uiManager.LoadUI(_UI.JOIN);

    private void OnDisable()
    {
        _roomNameInput.text = "";
        _passwordInput.text = "";
        _roomNumberInput.text = "";

        _makeRoomButton.onClick.RemoveListener(RequestMakeRoom);
        _closeButton.onClick.RemoveListener(Close);
        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
    }
}
