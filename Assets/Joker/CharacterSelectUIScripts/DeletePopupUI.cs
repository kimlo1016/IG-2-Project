using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePopupUI : PopupUI
{
    [SerializeField] Button _deleteButton;

    protected new void OnEnable()
    {
        base.OnEnable();
        _deleteButton.onClick.RemoveListener(DeleteCharacter);
        _deleteButton.onClick.AddListener(DeleteCharacter);
    }

    private void DeleteCharacter()
    {
        // ĳ���� ������ ����
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        _deleteButton.onClick.RemoveListener(DeleteCharacter);
    }
}
