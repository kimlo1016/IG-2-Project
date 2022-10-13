#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Defines
{
    public enum EPanelType
    {
        MenuPanel,
        FriendPanel,
    }
}

public class InventoryUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _socialButton;

    [Header("Inventory Panels")]
    [SerializeField] private GameObject _InventoryUI;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _socialPanel;

    /// <summary>
    /// Inventory UI�� �����ִ��� ����
    /// </summary>
    public bool IsInventoryUIOn { get { return _InventoryUI.activeSelf; } }

    [Header("Friend Panel")]
    [SerializeField] private GameObject _friendPanel;
    private TextMeshProUGUI _targetFriendName;

    private GameObject _currentShownPanel;

    private void Awake()
    {
        _currentShownPanel = _inventoryPanel;
        _targetFriendName = _friendPanel.GetComponentInChildren<TextMeshProUGUI>();

        SettingButtons();
    }

    private void SettingButtons()
    {
        _inventoryButton.onClick.AddListener(() => { ShowMenuPanel(_inventoryPanel); });
        _settingButton.onClick.AddListener(() => { ShowMenuPanel(_settingPanel); });
        _socialButton.onClick.AddListener(() => { ShowMenuPanel(_socialPanel); });
    }
    private void ShowMenuPanel(GameObject panel)
    {
        _currentShownPanel.SetActive(false);
        panel.SetActive(true);
        _currentShownPanel = panel;
    }
    public void ExitPanel(GameObject exitPanel)
    {
        Debug.Log(exitPanel.name + " is now closed");
        exitPanel.SetActive(false);
    }

    /// <summary>
    /// Inventory UI ������
    /// </summary>
    public void ShowInventoryUI()
    {
        _InventoryUI.SetActive(true);
    }
    /// <summary>
    /// ģ�� �߰� �г��� ������
    /// </summary>
    /// <param name="targetUserName"> Ÿ�� ���� �̸� </param>
    public void ShowFriendPanel(string targetUserName)
    {
#if _DEV_MODE_
        Debug.Assert(targetUserName != null, "������ ����");
#else
        targetUserName = "����";
#endif
        _targetFriendName.text = targetUserName;
        _friendPanel.SetActive(true);
    }
}