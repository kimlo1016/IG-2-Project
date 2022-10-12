using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkPractice : MonoBehaviourPunCallbacks
{
    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI _infoText;

    [Header("Buttons")]
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _resetRoomListButton;
    [SerializeField] private Button _exitRoomButton;

    [Header("Room List")]
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _scrollViewContent;

    private RoomOptions _roomOption = new RoomOptions()
    {
        MaxPlayers = 5
    };

    // ���⿡ RoomList�� �����.
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private int _roomCount = 0;

    /* ����/�κ� ���� ���� �Լ� */
    private void Awake()
    {
        ResetButton();

        _infoText.text = "Connecting...";
        
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        _infoText.text = "Connected! Joinning Lobby...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        _infoText.text = "Lobby Joined!";
        SetInteractableOfAllButtons(true);
    }
    public override void OnLeftLobby()
    {
        _infoText.text = "Reconnecting Lobby...";
        SetInteractableOfAllButtons(false);
        PhotonNetwork.JoinLobby();
    }


    /// <summary>
    /// �� ����Ʈ�� ������Ʈ �� ������ ȣ��Ǵ� �Լ�.
    /// �κ� ���� ���� ����ȴ�.
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _infoText.text = "Room List Updated";
        RoomListUpdate(roomList);
        Invoke("setInfoText", 0.5f);
    }
    /// <summary>
    /// cachedRoomList�� �ʱ�ȭ ��
    /// </summary>
    /// <param name="roomList">OnRoomListUpdate���� �޾ƿ� roomList</param>
    private void RoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(roomList.Count);

        foreach (RoomInfo room in roomList)
        {
            // �� ����Ʈ�� �ִ� ��� �� �̹� ���� �� ����� cachedRoomList���� ���ܵȴ�.
            if(room.RemovedFromList)
            {
                cachedRoomList.Remove(room.Name);
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }

        SetRoomList();
    }
    /// <summary>
    /// cachedRoomList�� ������� UI�� ������.
    /// ȿ�������� ���� ��ũ��Ʈ... ���� �ʿ�
    /// </summary>
    private void SetRoomList()
    {
        // Room List Panel �ʱ�ȭ
        foreach(Transform roomPanel in _scrollViewContent.GetComponentsInChildren<Transform>())
        {
            if(roomPanel.gameObject == _scrollViewContent.gameObject)
            {
                continue;
            }

            Destroy(roomPanel.gameObject);
        }

        // cachedRoomList�� ���� Panel ����
        foreach(KeyValuePair<string, RoomInfo> room in cachedRoomList)
        {
            // �ش� ���� �̹� �����Ǿ��ٸ� ui���� ������
            if(room.Value.RemovedFromList)
            {
                continue;
            }

            GameObject newRoomPanel = Instantiate(_roomPanel, _scrollViewContent.transform);
            newRoomPanel.GetComponentInChildren<Text>().text = room.Key;
        }

        _roomCount = cachedRoomList.Count;
    }
    /// <summary>
    /// �� ���� ��ħ
    /// </summary>
    private void ResetRoomList()
    {
        SetInteractableOfAllButtons(false);
        _infoText.text = "Resetting Room List...";

        SetRoomList();

        SetInteractableOfAllButtons(true);
        _infoText.text = "Done!";
        Invoke("setInfoText", 0.5f);
    }



    /* �游��� ���� �Լ� */
    private void CreateRoom()
    {
        ++_roomCount;
        PhotonNetwork.CreateRoom(_roomCount.ToString(), _roomOption, null, null);
        _infoText.text = "Creating Room...";
        SetRoomList();
    }
    public override void OnCreatedRoom()
    {
        _infoText.text = $"Created Room {PhotonNetwork.CurrentRoom.Name}";
        
        _createRoomButton.gameObject.SetActive(false);
        _resetRoomListButton.gameObject.SetActive(false);
        _exitRoomButton.gameObject.SetActive(true);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _infoText.text = $"Fail to Created Room {_roomCount}";
    }

    /* ��Ÿ �Լ�, �߿����� ���� */
    private void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        _createRoomButton.gameObject.SetActive(true);
        _resetRoomListButton.gameObject.SetActive(true);
        _exitRoomButton.gameObject.SetActive(false);
    }
    private void ResetButton()
    {
        _createRoomButton.onClick.AddListener(() => { CreateRoom(); });
        _resetRoomListButton.onClick.AddListener(() => { ResetRoomList(); });
        _exitRoomButton.onClick.AddListener(() => { ExitRoom(); });

        SetInteractableOfAllButtons(false);

        _exitRoomButton.gameObject.SetActive(false);
    }
    private void SetInteractableOfAllButtons(bool value)
    {
        _createRoomButton.interactable = value;
        _resetRoomListButton.interactable = value;
        _exitRoomButton.interactable = value;
    }
    private void ResetInfoText()
    {
        _infoText.text = "Waiting...";
    }
}
