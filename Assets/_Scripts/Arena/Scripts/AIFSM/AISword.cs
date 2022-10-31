using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField] private Collider _swordHitBox;

    [Header("�⺻���� ����Ʈ�� �־��ּ���")]
    [SerializeField] private GameObject _attackEffect;

    public void OnHitBox()
    {
        _swordHitBox.enabled = true;

        if (_attackEffect != null)
        {
            _attackEffect.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void OffHitBox()
    {
        _swordHitBox.enabled = false;

        if (_attackEffect != null)
        {
            _attackEffect.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
