using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.ELogInErrorType;
using Scene = Defines.ESceneNumder;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;

public class LogInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;
    [SerializeField] LogInServerManager _logInServerManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _signInButton;
    [SerializeField] Button _changePasswordButton;
    [SerializeField] Button _quitButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;

    [Header("Popup")]
    [SerializeField] LogInErrorPopupUI _errorPopup;

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LogIn);
        _logInButton.onClick.AddListener(LogIn);
        
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        
        _changePasswordButton.onClick.RemoveListener(LoadFind);
        _changePasswordButton.onClick.AddListener(LoadFind);
        
        _quitButton.onClick.RemoveListener(Quit);
        _quitButton.onClick.AddListener(Quit);

        Sql.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _errorPopup.ErrorPopup(Error.ID);
        }
    }

    /// <summary>
    /// �Էµ� ���� ����(Email, Password)�� ���� DB�� ����
    /// ��ġ�ϸ� ���� ���� �ε��Ѵ�.
    /// </summary>
    private void LogIn()
    {
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            _errorPopup.ErrorPopup(Error.ID);
            return;
        }

        if (!Sql.CheckValueByBase(Column.Email, _idInput.text, 
            Column.Password, Hash.Compute(_passwordInput.text)))
        {
            _errorPopup.ErrorPopup(Error.PASSWORD);
            return;
        }

        TempAccountDB.SetAccountData(_idInput.text, Sql.GetValueByBase(Column.Email, _idInput.text, Column.Nickname));
        Debug.Log("�α��� ����!");
        //PhotonNetwork.LoadLevel((int)Scene.StartRoom);
        _logInServerManager.LogIn();
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
        _changePasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
    }
}