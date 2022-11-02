using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using UnityEngine.UI;
using TMPro;

public class Tumbleweed : MonoBehaviour
{
    private enum ECoinGrade
    {
        Common,
        Rare,
        Epic,
        Max,
    }

    [Header("�⺻ ����")]
    [SerializeField] private float _lifeTime = 20f;
    [SerializeField] private float _getGoldTime = 3f;
    [Header("��� ����")]
    [SerializeField] private int[] _goldCoinGiveCount = new int[(int)ECoinGrade.Max];
    [SerializeField] private float[] _goldCoinRate = new float[(int)ECoinGrade.Max];
    private float _maxGoldCoinRate;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [Header("�̵� ����")]
    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [Header("UI")]
    [SerializeField] private Transform _UITransform;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _getGoldPanel;
    [SerializeField] private TextMeshProUGUI _goldCountText;

    [Header("����")]
    [SerializeField] private AudioClip[] _goldCoinAudioClips = new AudioClip[(int)ECoinGrade.Max];
    private AudioSource _audioSource;

    // �÷��̾� �ν� ����
    private Transform _playerTransform;
    private PlayerTumbleweedInteraction _playerInteraction;
    private bool _isTherePlayer;
    private bool _isGetCoin;

    // ������
    private TumbleweedSpawner _spawner;

    // ��� ���� ����
    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;
    private WaitForSeconds _waitForLifeTime;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _waitForLifeTime = new WaitForSeconds(_lifeTime);

        _rigidbody = GetComponent<Rigidbody>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;

        _spawner = GetComponentInParent<TumbleweedSpawner>();

        _audioSource = GetComponent<AudioSource>();

        _meshRenderer = GetComponent<MeshRenderer>();

        _maxGoldCoinRate = 0f;
        foreach(float rate in _goldCoinRate)
        {
            _maxGoldCoinRate += rate;
        }
    }

    private void OnEnable()
    {
        ActiveSelf();
        ResetTumbleweed();
    }

    private void ResetTumbleweed()
    {
        _rigidbody.velocity = ZERO_VECTOR;
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);

        _outline.enabled = false;
        _meshRenderer.enabled = true;
        _slider.gameObject.SetActive(false);
        _slider.value = 0f;
        _getGoldPanel.SetActive(false);

        _isTherePlayer = false;
        _isGetCoin = false;

        StartCoroutine(CoDisableSelf());
    }

    private IEnumerator CoDisableSelf()
    {
        yield return _waitForLifeTime;
        DisableSelf();
    }

    private void FixedUpdate()
    {
        if(_isTherePlayer)
        {
            _UITransform.rotation = Quaternion.Euler(0f, _UITransform.rotation.y, _UITransform.rotation.z);
            _UITransform.transform.LookAt(_playerTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        PlayerTumbleweedInteraction playerInteraction = other.transform.root.GetComponentInChildren<PlayerTumbleweedInteraction>();
        if(!playerInteraction || playerInteraction.IsNearTumbleweed)
        {
            return;
        }

        _outline.enabled = true;
        _playerTransform = other.transform.root;
        _playerInteraction = playerInteraction;
        _playerInteraction.IsNearTumbleweed = true;
        _isTherePlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        if(_isTherePlayer && _playerTransform != other.transform.root)
        {
            return;
        }

        _outline.enabled = false;
        _isTherePlayer = false;
        _playerInteraction.IsNearTumbleweed = false;
        _slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_isGetCoin)
        {
            return;
        }

        if(!_isTherePlayer || _playerInteraction.GrabbingTime <= 0f)
        {
            _slider.gameObject.SetActive(false);
            _slider.value = 0f;
            return;
        }

        _slider.gameObject.SetActive(true);
        _slider.value = _playerInteraction.GrabbingTime / _getGoldTime;
        
        if(_slider.value >= 1f)
        {
            _isGetCoin = true;

            _rigidbody.velocity = ZERO_VECTOR;

            StopAllCoroutines();
            
            _playerInteraction.GetGold(GiveRandomGold());
            _playerInteraction.IsNearTumbleweed = false;
            
            _meshRenderer.enabled = false;
            
            Invoke("DisableSelf", 1f);
        }
    }

    private int GiveRandomGold()
    {
        float randomInt = Random.Range(0f, _maxGoldCoinRate);
        Debug.LogError(randomInt);

        float coinRate = 0f;
        for(int i = 0; i < (int) ECoinGrade.Max; ++i)
        {
            coinRate += _goldCoinRate[i];
            if(randomInt < coinRate)
            {
                return GiveCoinEffect(i);
            }
        }

        return -1;
    }
    private int GiveCoinEffect(int grade)
    {
        _audioSource.PlayOneShot(_goldCoinAudioClips[grade]);

        _slider.gameObject.SetActive(false);
        _goldCountText.text = "+" + _goldCoinGiveCount[grade];
        _getGoldPanel.SetActive(true);

        return _goldCoinGiveCount[grade];
    }

    private void OnDisable()
    {
        _spawner.ReturnToTumbleweedStack(gameObject);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    private void ActiveSelf()
    {
        //gameObject.SetActive(true);
    }
}
