using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using System;

public class PlayerCustomize : MonoBehaviour
{
    public static int IsFemale = 0;

    [SerializeField] UserCustomizeData _femaleData;
    [SerializeField] UserCustomizeData _maleData;
    [SerializeField] UserCustomizeData _userData;
    private int _equipNum;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        AvatarInit();

        //if(bool.Parse(MySqlSetting.GetValueByBase(Asset.EaccountdbColumns.Nickname,name,Asset.EaccountdbColumns.HaveCharacter)))
        //{
        //    AvatarSetting();
        //}
        //else
        //{

        //    AvatarInit();
        //}
    }


    public void AvatarInit()
    {

        if(IsFemale == 0)
        {
            _userData = _maleData;
           
        }
        else
        {
            _userData = _femaleData;
            
        }
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[0];

    }

    private void AvatarSetting()
    {
        
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.Gender));

        // ������ �´� �����͸� �ҷ���
        if (_isFemale)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarData).Split(',');

        // �ҷ��� �����͸� ��ũ���ͺ� ������Ʈ�� �־���
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ�� Material�� �ҷ���
        _userData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));

        // �ƹ�Ÿ�� ������ ���鼭 �������̴� �ƹ�Ÿ�� ã�Ƴ�.
        for (int i = 0; i < _userData.AvatarState.Length - 1; ++i)
        {
            if (_userData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _equipNum = i;
                break;
            }
        }

        // �������̴� �����۰� Material�� �����Ŵ.
        float _setMaterialNum = _userData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_equipNum];
    }
    


}