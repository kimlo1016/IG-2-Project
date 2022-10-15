using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using Column = Asset.MySql.EAccountColumns;
using UI = Defines.ELogInUIIndex;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;

public class LogInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _signInButton;
    [SerializeField] Button _findPasswordButton;
    [SerializeField] Button _quitButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;

    [Header("Popup")]
    [SerializeField] LogInErrorPopupUI _errorPopup;

    public Defines.ELogInErrorType ErrorType { get; private set; }

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
        _logInButton.onClick.AddListener(LogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _findPasswordButton.onClick.AddListener(LoadFind);
        _quitButton.onClick.AddListener(Quit);
    }

    /// <summary>
    /// �Էµ� ���� ����(Email, Password)�� ���� DB�� ����
    /// ��ġ�ϸ� ���� ���� �ε��Ѵ�.
    /// </summary>
    private void LogIn()
    {
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            return;
        }

        if (!Sql.CheckValueByBase(Column.Email, _idInput.text, 
            Column.Password, Hash.Compute(_passwordInput.text)))
        {
            return;
        }

        TempAccountDB.SetAccountData(_idInput.text, Sql.GetValueByBase(Column.Email, _idInput.text, Column.Nickname));
        Debug.Log("�α��� ����!");
        // PhotonNetwork.LoadLevel() // ���� ������ �̾����� �κ� �ʿ�
    }

    /// <summary>
    /// ȸ������ UI �ε�
    /// </summary>
    private void LoadSignIn() => _logInUIManager.LoadUI(UI.SIGNIN);

    /// <summary>
    /// ��й�ȣ ã�� UI �ε�
    /// </summary>
    private void LoadFind() => _logInUIManager.LoadUI(UI.FINDPASSWORD);

    /// <summary>
    /// ���� ����
    /// </summary>
    private void Quit() => Application.Quit();

    private void OnDisable()
    {
        _idInput.text = "";
        _passwordInput.text = "";
        
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
    }
}
