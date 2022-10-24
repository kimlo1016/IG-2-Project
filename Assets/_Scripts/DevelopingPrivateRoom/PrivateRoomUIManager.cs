using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using _UI = Defines.EPrivateRoomUIIndex;

public class PrivateRoomUIManager : UIManager
{
    [SerializeField] TMP_InputField[] _inputFields;

    private void Awake()
    {
        LoadUI(_UI.JOIN);

        foreach (TMP_InputField input in _inputFields)
        {
            input.onSelect.AddListener((string temp) =>
            {
                KeyboardManager.OpenKeyboard();
            });
        }
    }

    /// <summary>
    /// EPrivateRoomUIIndex�� ui �Ű������� �޾�, UIManager.LoadUI�� ������ 
    /// UI ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� UI ������Ʈ�� Ȱ��ȭ�Ѵ�.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(_UI ui)
    {
        LoadUI((int)ui);
    }
}
