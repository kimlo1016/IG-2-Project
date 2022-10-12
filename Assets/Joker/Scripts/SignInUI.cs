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
    [SerializeField] Button _idDoubleCheckButton;
    [SerializeField] Button _passwordDoubleCheckButton;
    [SerializeField] Button _nicknameDoubleCheckButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _passwordCheckInput;
    [SerializeField] TMP_InputField _nicknameInput;
    [SerializeField] TMP_InputField _answerInput;

    [Header("Error Text")]
    [SerializeField] GameObject _idErrorText;
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
        _idDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
        _signInButton.onClick.AddListener(SignIn);
        _passwordDoubleCheckButton.onClick.AddListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.AddListener(NicknameDoubleCheck);
        _idDoubleCheckButton.onClick.AddListener(EmailDoubleCheck);

        _nicknameErrorText?.SetActive(false);
        _passwordErrorText?.SetActive(false);
        _idErrorText?.SetActive(false);

        _successPopup.SetActive(false);

        _hasEmailCheck = false;
        _hasPasswordCheck = false;
        _hasNicknameCheck = false;
    }

    /// <summary>
    /// 입력된 계정 정보(Email, Password, Nickname)를 바탕으로ㄴ
    /// 각 정보의 중복체크가 완료되었다면 계정 DB에 저장한다.
    /// </summary>
    private void SignIn()
    {
        if (!_hasEmailCheck || !_hasPasswordCheck || !_hasNicknameCheck)
        {
            return;
        }

        Debug.Assert(MySqlSetting.AddNewAccount(_idInput.text, _passwordInput.text, _nicknameInput.text), "계정 생성 실패!");

        _successPopup.SetActive(true);
    }

    /// <summary>
    /// 입력된 Email 정보를 DB와 비교해 중복체크
    /// </summary>
    private void EmailDoubleCheck()
    {
        if (MySqlSetting.HasValue(Column.Email, _idInput.text))
        {
            _hasEmailCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            _hasEmailCheck = false;
            _idErrorText.SetActive(true);
        }
    }
    
    /// <summary>
    /// 입력된 비밀번호와 비밀번호 체크용 입력을 비교해 일치하는지 확인
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
    /// 입력된 Nickname 정보를 DB와 비교해 중복체크
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
        _idInput.text = "";
        _answerInput.text = "";

        _signInButton.onClick.RemoveListener(SignIn);
        _passwordDoubleCheckButton.onClick.RemoveListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.RemoveListener(NicknameDoubleCheck);
        _idDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
    }
}
