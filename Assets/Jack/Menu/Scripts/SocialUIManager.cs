using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

public class SocialUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _socialUI;
    [SerializeField] private TextMeshProUGUI _targetUserNicknameText;

    [Header("Add Friend")]
    [SerializeField] private Button _addFriendButton;

    [Header("Block User")]
    [SerializeField] private Button _blockFriendButton;
    [SerializeField] private string _blockInfoString;
    [SerializeField] private string _blockDissableInfoString;
    private TextMeshProUGUI _blockFriendText;
    [SerializeField] private GameObject _blockedConfirmedPanel;

    [Header("Extra")]
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;

    private string _myNickname;
    private string _targetUserNickname;

    private void Awake()
    {
        _myNickname = TempAccountDB.Nickname;
        _blockFriendText = _blockFriendButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // 1. �� ������ ���踦 Ȯ����

        // 1-1. ���谡 ���� ���(��ȯ���� -1)
        RequestFriendButton();
        BlockUserButton();

        // 1-2. ģ���� ���(��ȯ���� 0)
        CancelFriendButton();
        BlockUserButton();

        // ....�׿�
        // 2. �� ���̿��� ���� ��ġ(A���� B���� �˾Ƴ�)�� Ȯ���Ͽ�
        // ��ġ�� ���� Ȯ���� ��Ʈ �ڸ����� ������ ����, �ش� ����� ������ �Ǻ�
        
        // 2-1. ���� ����� ��Ȳ
        ;
        UnblockUserButton();

        // 2-2. ���� ģ�� ��û�� �� ��Ȳ
        CancelRequestButton();
        BlockUserButton();

        // 2-3. ���°� ������ ģ�� ��û�� �� ��Ȳ
        CheckFriendRequestButton();
        BlockUserButton();
    }

    /// <summary>
    /// ģ�� �߰� �г��� ������
    /// </summary>
    /// <param name="targetUserName"> Ÿ�� ���� �̸� </param>
    public void ShowFriendPanel(string targetUserName)
    {
        _targetUserNickname = targetUserName;
        _targetUserNicknameText.text = _targetUserNickname;
        _socialUI.SetActive(true);
    }

    private void RequestFriendButton()
    {

    }

    private void CancelRequestButton()
    {

    }

    private void CancelFriendButton()
    {

    }

    private void CheckFriendRequestButton()
    {

    }
    
    private void BlockUserButton()
    {

    }

    private void UnblockUserButton()
    {

    }
    /*
    switch (MySqlSetting.CheckSocialStatus(_myNickname, _targetUserNickname))
    {
        // 1. ģ�� ��û ������ ���
        case ESocialStatus.Request:
            {
                // ģ�� ��û �ٽ� ����
                _addFriendButton.interactable = false;

                // ��� ����� ����
                _blockFriendButton.onClick.AddListener(() =>
               {
                   MySqlSetting.UpdateSocialStatus(_myNickname, _targetUserNickname, ESocialStatus.Block);
                   _blockedConfirmedPanel.SetActive(true);
                   _socialUI.SetActive(false);
               });
            }
            break;

        // 2. ��� ������ ���
        case ESocialStatus.Block:
            {
                _addFriendButton.interactable = false;

                // ��� ��ư�� ��� ���� ��ư���� ����
                _blockFriendText.text = _blockDissableInfoString;
                _blockFriendButton.onClick.AddListener(() =>
               {
                   // ��� ����(RelationshipDB���� �ش� ���� ����) ��� �߰� �ʿ�
                   Debug.Log("��� ������");
                   _blockFriendText.text = _blockInfoString;

                   // ��ư �ʱ�ȭ
                   InitializeButtons();
               });
            }
            break;

        // 3. �׳� ������ ���
        default:
            {
                _addFriendButton.interactable = true;

                _blockFriendButton.interactable = true;
            }
            break;
    }
    */
}
