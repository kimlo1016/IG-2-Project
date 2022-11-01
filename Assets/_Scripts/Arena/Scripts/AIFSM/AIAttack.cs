using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class AIAttack : AIState
{
    //[Header("������ ��ũ��Ʈ�� �־��ּ���")]
    //[SerializeField] AIInfo[] _aIInfo;

    [Header("�� ��ų ��Ÿ���� �־��ּ���")]
    [SerializeField] int _skillCoolTime;

    public UnityEvent<int> EnemyDamage;

    private Animator _animator;

    private bool _isSkillCoolTime;
    private bool _changeStateAttackToDamage;
    private float _curTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        EnemyDamage = new UnityEvent<int>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isAttack, true);
        _isSkillCoolTime = true;
    }
    
    public override void OnUpdate()
    {
        if (_changeStateAttackToDamage == true)
        {
            aiFSM.ChangeState(EAIState.Damage);   

            _changeStateAttackToDamage = false;
        }

        if (_isSkillCoolTime == true && _skillCoolTime != 0)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= _skillCoolTime)
        {
            _curTime -= _curTime;
            _isSkillCoolTime = false;
            aiFSM.ChangeState(EAIState.Skill);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack, false);
        _isSkillCoolTime = false;      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            int _enemyDamage;
            _enemyDamage = other.gameObject.GetComponentInParent<AIInfo>().Damage;
            EnemyDamage.Invoke(_enemyDamage);

            _changeStateAttackToDamage = true;
            
        }
        if (Input.GetKeyDown(KeyCode.))
        if (other.gameObject.tag == "AIMagic")
        {
            int _enemySkillDamage;
            _enemySkillDamage = other.gameObject.GetComponentInParent<AIInfo>().SkillDamage;
            EnemyDamage.Invoke(_enemySkillDamage);

            _changeStateAttackToDamage = true;
            
        }
    }
}
