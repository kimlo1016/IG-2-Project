using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInUIManager : MonoBehaviour
{
    public enum ELogInUIIndex
    {
        LOGIN,
        SIGNIN,
        FINDPASSWORD,
        MAX
    }
    private void Awake()
    {
        LoadUI(ELogInUIIndex.LOGIN);
    }

    [SerializeField] GameObject[] UI;

    private void ShutUI()
    {
        foreach (GameObject ui in UI)
        {
            ui.SetActive(false);
        }
    }

    // ELogInUIIndex�� �Ű������� �޾�, ui ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� ui ������Ʈ�� Ȱ��ȭ�Ѵ�.
    public void LoadUI(ELogInUIIndex ui)
    {
        ShutUI();
        UI[(int)ui].SetActive(true);
    }
}
