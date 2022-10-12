using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FindPasswordUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _findPasswordButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _emailInput;
    [SerializeField] TMP_InputField _answerInput;
    [SerializeField] TMP_InputField _passwordOutput;

    [Header("Popup")]
    [SerializeField] FindPasswordErrorPopupUI _errorPopup;

    public Defines.EFindPasswordErrorType ErrorType { get; private set; }

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
        _logInButton.onClick.AddListener(LoadLogIn);
        _findPasswordButton.onClick.AddListener(FindPassword);
    }

    private void LoadLogIn() => _logInUIManager.LoadUI(Defines.ELogInUIIndex.LOGIN);

    private void FindPassword()
    {
        if (_emailInput.text == _emailInput.text) // DB ���� �ʿ�
        {
            _errorPopup.ErrorPopup(Defines.EFindPasswordErrorType.EMAIL);
            return;
        }
        if (_answerInput.text != _answerInput.text) // DB ���� �ʿ�
        {
            _errorPopup.ErrorPopup(Defines.EFindPasswordErrorType.ANSWER);
            return;
        }

        _passwordOutput.text = "��й�ȣ"; // DB ���� �ʿ�
    }

    private void OnDisable()
    {
        _emailInput.text = "";
        _answerInput.text = "";
        _passwordOutput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
    }
}
