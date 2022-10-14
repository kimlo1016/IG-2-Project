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
    [SerializeField] private string _requestFriendConfirmMessage;
    [SerializeField] private string _cancelRequestConfirmMessage;
    [SerializeField] private string _cancelFriendConfirmMessage;

    [Header("Check Request")]
    [SerializeField] private string _checkRequestMessage;
    [SerializeField] private string _acceptRequestMessage;
    [SerializeField] private string _denyRequestMessage;

    [Header("Block User")]
    [SerializeField] private Button _blockFriendButton;
    [SerializeField] private string _blockConfirmMessage;
    [SerializeField] private string _unblockConfirmMessage;
    private TextMeshProUGUI _blockFriendText;
    [SerializeField] private GameObject _blockedConfirmedPanel;

    [Header("Extra")]
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;
    [SerializeField] private CheckPanelManager _checkPanelManager;

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
        OnClickRequestFriendButton();
        OnClickBlockUserButton();

        // 1-2. ģ���� ���(��ȯ���� 0)
        OnClickCancelFriendButton();
        OnClickBlockUserButton();

        // ....�׿�
        // 2. �� ���̿��� ���� ��ġ(A���� B���� �˾Ƴ�)�� Ȯ���Ͽ�
        // ��ġ�� ���� Ȯ���� ��Ʈ �ڸ����� ������ ����, �ش� ����� ������ �Ǻ�

        // 2-1. ���� ����� ��Ȳ
        ;
        OnClickUnblockUserButton();

        // 2-2. ���� ģ�� ��û�� �� ��Ȳ
        OnClickCancelRequestButton();
        OnClickBlockUserButton();

        // 2-3. ���°� ������ ģ�� ��û�� �� ��Ȳ
        OnClickCheckFriendRequestButton();
        OnClickBlockUserButton();
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

    private void AddListenerToButton(Button button, UnityEngine.Events.UnityAction newListener, bool isNeedButtonInitialize)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(newListener);
        button.onClick.AddListener(() => { InitializeButtons(); });
    }

    // ģ�� �߰� ��û
    private void OnClickRequestFriendButton()
    {
        // ģ�� �߰� ��û�� DB�� �ø�
        _confirmPanelManager.ShowConfirmPanel(_requestFriendConfirmMessage);
    }

    // ģ�� �߰� ��û ���
    private void OnClickCancelRequestButton()
    {
        // ģ�� ��� ��û�� DB�� �ø�
        _confirmPanelManager.ShowConfirmPanel(_cancelRequestConfirmMessage);
    }

    // ģ�� ����
    private void OnClickCancelFriendButton()
    {
        // ģ�� ���� ��û�� DB�� �ø�
        _confirmPanelManager.ShowConfirmPanel(_cancelFriendConfirmMessage);
    }

    // ����� ģ�� ��û �Ǵ�
    private void OnClickCheckFriendRequestButton()
    {
        _checkPanelManager.ShowCheckPanel(_targetUserNickname + _checkRequestMessage,
            () =>
            {
                // ģ�� �߰� ��û�� DB�� �ø�
                _confirmPanelManager.ShowConfirmPanel(_acceptRequestMessage);
                InitializeButtons();
            },
            () =>
            {
                // ģ�� ���� ��û�� DB�� �ø�
                _confirmPanelManager.ShowConfirmPanel(_denyRequestMessage);
                InitializeButtons();
            }
            );
    }
    
    // ��� ����
    private void OnClickBlockUserButton()
    {

    }

    // ��� ���� ����
    private void OnClickUnblockUserButton()
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
