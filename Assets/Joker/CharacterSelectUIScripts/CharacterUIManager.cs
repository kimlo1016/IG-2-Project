using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.ECharacterUIIndex;

public class CharacterUIManager : MonoBehaviour
{
    [SerializeField] GameObject[] UI;

    private void Awake()
    {
        LoadUI(_UI.SELECT);
    }

    private void ShutUI()
    {
        foreach (GameObject ui in UI)
        {
            ui.SetActive(false);
        }
    }

    // ECharacterUIIndex�� �Ű������� �޾�, ui ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� ui ������Ʈ�� Ȱ��ȭ�Ѵ�.
    public void LoadUI(_UI ui)
    {
        ShutUI();
        UI[(int)ui].SetActive(true);
    }
}
