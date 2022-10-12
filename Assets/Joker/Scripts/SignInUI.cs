using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;

using Column = Asset.MySql.EAccountColumns;
using UI = Defines.ELogInUIIndex;

public class SignInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;
    
    [Header("Button")]
    [SerializeField] Button _signInButton;
    [SerializeField] Button _emailDoubleCheckButton;
    [SerializeField] Button _passwordDoubleCheckButton;
    [SerializeField] Button _nicknameDoubleCheckButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _emailInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _passwordCheckInput;
    [SerializeField] TMP_InputField _nicknameInput;
    [SerializeField] TMP_InputField _answerInput;

    [Header("Error Text")]
    [SerializeField] GameObject _emailErrorText;
    [SerializeField] GameObject _passwordErrorText;
    [SerializeField] GameObject _nicknameErrorText;

    [Header("Popup")]
    [SerializeField] GameObject _successPopup;

    private bool _hasNicknameCheck;
    private bool _hasEmailCheck;
    private bool _hasPasswordCheck;
    
    private void OnEnable()
    {
        _signInButton.onClick.RemoveListener(SignIn);
        _passwordDoubleCheckButton.onClick.RemoveListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.RemoveListener(NicknameDoubleCheck);
        _emailDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
        _signInButton.onClick.AddListener(SignIn);
        _passwordDoubleCheckButton.onClick.AddListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.AddListener(NicknameDoubleCheck);
        _emailDoubleCheckButton.onClick.AddListener(EmailDoubleCheck);

        _nicknameErrorText?.SetActive(false);
        _passwordErrorText?.SetActive(false);
        _emailErrorText?.SetActive(false);

        _successPopup.SetActive(false);

        _hasEmailCheck = false;
        _hasPasswordCheck = false;
        _hasNicknameCheck = false;
    }

    /// <summary>
    /// �Էµ� ���� ����(Email, Password, Nickname)�� �������Τ�
    /// �� ������ �ߺ�üũ�� �Ϸ�Ǿ��ٸ� ���� DB�� �����Ѵ�.
    /// </summary>
    private void SignIn()
    {
        if (!_hasEmailCheck || !_hasPasswordCheck || !_hasNicknameCheck)
        {
            return;
        }

        Debug.Assert(MySqlSetting.AddNewAccount(_emailInput.text, _passwordInput.text, _nicknameInput.text), "���� ���� ����!");

        _successPopup.SetActive(true);
    }

    /// <summary>
    /// �Էµ� Email ������ DB�� ���� �ߺ�üũ
    /// </summary>
    private void EmailDoubleCheck()
    {
        if (MySqlSetting.HasValue(Column.Email, _emailInput.text))
        {
            _hasEmailCheck = true;
            _emailErrorText.SetActive(false);
        }
        else
        {
            _hasEmailCheck = false;
            _emailErrorText.SetActive(true);
        }
    }
    
    /// <summary>
    /// �Էµ� ��й�ȣ�� ��й�ȣ üũ�� �Է��� ���� ��ġ�ϴ��� Ȯ��
    /// </summary>
    private void PasswordDoubleCheck()
    {
        if (_passwordInput.text == _passwordCheckInput.text)
        {
            _hasPasswordCheck = true;
            _passwordErrorText.SetActive(false);
        }
        else
        {
            _hasPasswordCheck = false;
            _passwordErrorText.SetActive(true);
        }
    }

    /// <summary>
    /// �Էµ� Nickname ������ DB�� ���� �ߺ�üũ
    /// </summary>
    private void NicknameDoubleCheck()
    {
        if (MySqlSetting.HasValue(Column.Nickname, _nicknameInput.text))
        {
            _hasNicknameCheck = true;
            _nicknameErrorText.SetActive(false);
        }
        else
        {
            _hasNicknameCheck = false;
            _nicknameErrorText.SetActive(true);
        }
    }

    private void OnDisable()
    {
        _nicknameInput.text = "";
        _passwordInput.text = "";
        _passwordCheckInput.text = "";
        _emailInput.text = "";
        _answerInput.text = "";

        _signInButton.onClick.RemoveListener(SignIn);
        _passwordDoubleCheckButton.onClick.RemoveListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.RemoveListener(NicknameDoubleCheck);
        _emailDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
    }
}
