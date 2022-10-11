using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField] TMP_InputField _emailInput;
    [SerializeField] TMP_InputField _passwordInput;

    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _findPasswordButton.onClick.AddListener(LoadFind);
        _quitButton.onClick.AddListener(Quit);
    }

    // �Էµ� ���� ������ ���� DB�� ���� ��ġ�ϸ� ���� ���� �ε��Ѵ�.
    public void LogIn()
    {
        // DB ���� �ʿ�
        Debug.Log("�α���!");
    }

    public void LoadSignIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SIGNIN);
    public void LoadFind() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.FINDPASSWORD);
    public void Quit() => Application.Quit();

    private void OnDisable()
    {
        _emailInput.text = "";
        _passwordInput.text = "";
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.AddListener(Quit);
    }
}
