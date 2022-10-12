using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField] protected GameObject[] _ui;

    protected void ShutUI()
    {
        foreach (GameObject ui in _ui)
        {
            ui.SetActive(false);
        }
    }

    /// <summary>
    /// int ui �ε����� �Ű������� �޾�, UI ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� UI ������Ʈ�� Ȱ��ȭ�Ѵ�.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(int ui)
    {
        ShutUI();
        _ui[ui].SetActive(true);
    }
}
