using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerDamage : AIState
{
    [Header("�� AI~~�� �־��ּ���")]
    [SerializeField] private AIInfo _aIInfo;

    [Header("�� AI~~�� �־��ּ���")]
    [SerializeField] private AIInfo[] _enemyAIInfo;

    private Animator _animator;
    private int _curHP;
    private int _damage;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _curHP = _aIInfo.HP;
        _damage = _enemyAIInfo[0].Damage;

        Debug.Log(_curHP);
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerDamage, true);

        _curHP -= _damage;

        Debug.Log(_curHP);
    }

    public override void OnUpdate()
    {
        if (_aIInfo.HP <= 0)
        {
            aiFSM.ChangeState(EAIState.Death);
        }
        else
        {
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerDamage, false);
    }
}
