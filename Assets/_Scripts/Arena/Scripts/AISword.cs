using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField]
    private Collider _swordHitBox;

    public void OnHitBox()
    {
        Debug.Log("����");
        _swordHitBox.enabled = true;
    }

    public void OffHitBox()
    {
        Debug.Log("����");
        _swordHitBox.enabled = false;
    }
}
