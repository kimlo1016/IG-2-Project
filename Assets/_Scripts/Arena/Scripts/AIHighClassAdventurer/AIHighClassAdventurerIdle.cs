using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerIdle : AIState
{
    private Animator _animator;

    [Header("�� �νĹ��� �ݶ��̴��� �־��ּ���")]
    [SerializeField] private Collider _myCollider;

    private float _curTime;
    private bool _isRunTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        if (_myCollider.enabled == false)
        {
            _myCollider.enabled = true;
        }

        _isRunTime = true;
        _curTime -= _curTime;
    }

    public override void OnUpdate()
    {
        if (_isRunTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 2f)
        {
            aiFSM.ChangeState(EAIState.Run);
            Debug.Log("���ּ���");
            _curTime -= _curTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerIdle, false);
        _isRunTime = false;
    }
}
