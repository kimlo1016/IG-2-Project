using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using EAIState = Defines.Estate;
using EDamage = Defines.EDamage;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private Collider _aiCollider;

    public UnityEvent HiAI = new UnityEvent();
    public UnityEvent<int> AttackAI = new UnityEvent<int>();

    [SerializeField]
    private EDamage _selectDamage;
    
    private int _damage;

    public void Init()
    {
    }

    private void OnEnable()
    {
        _damage = (int)_selectDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            AttackAI.Invoke(_damage);
        }

        if (other.gameObject.tag == "AI")
        {
            HiAI.Invoke();
            transform.LookAt(other.gameObject.transform);
            _aiCollider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}