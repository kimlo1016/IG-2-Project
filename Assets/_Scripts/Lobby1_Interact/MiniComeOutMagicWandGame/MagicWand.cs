﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MagicWand : MonoBehaviourPun
{
    [Header("확률에 해당하는 숫자를 누적시켜 적어주세요")]
    [SerializeField] private int[] _useMagicChance;

    private int _totalProbability = 100;

    [Header("쿨타임을 골라주세요")]
    [SerializeField] private Defines.CoolTime _coolTime;

    [Header("VRUI의 MagicWandPanel을 넣어주세요")]
    [SerializeField]
    private GameObject _magicWandPanel;

    private TextMeshProUGUI _magicNameText;
    private TextMeshProUGUI _magicCoolTimeText;

    [SerializeField]
    private ParticleSystem[] _magic;
    private float _currentTime;
    private bool _checkCoolTime;

    void Awake()
    {
        _magicNameText = _magicWandPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _magicCoolTimeText = _magicWandPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        _magicWandPanel.SetActive(false);

        _magic = new ParticleSystem[transform.childCount];

        for (int i = 0; i < transform.childCount -1; ++i)
        {
            _magic[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two) && !_checkCoolTime)
            {
                int RandomNumber = Random.Range(0, _totalProbability + 1);

                photonView.RPC("GetMagic", RpcTarget.All, RandomNumber);

                _magicWandPanel.SetActive(true);
                _magicNameText.text = gameObject.name;

                _checkCoolTime = true;
            }

            if (_checkCoolTime)
            {
                _currentTime += Time.deltaTime;

                int _coolTimeText = (int)_coolTime;

                _coolTimeText -= (int)_currentTime;
                _magicCoolTimeText.text = _coolTimeText.ToString();

                if (_currentTime > (float)_coolTime)
                {
                    _currentTime -= _currentTime;
                    _checkCoolTime = false;
                    _magicWandPanel.SetActive(false);
                }
            }
        }
    }

    [PunRPC]
    public void GetMagic(int num)
    {
        // int _getMagic = 0;

        for (int i = 0; i < transform.childCount; ++i)
        {
            //if (num - _useMagicChance[i] >= 0)
            //{
            //    _getMagic += _useMagicChance[i];
            //}
            //else
            //{
            //    _magic[i].gameObject.SetActive(true);
            //    break;
            //}

            if (num < _useMagicChance[i])
            {
                _magic[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
