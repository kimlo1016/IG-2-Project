using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private float _fadeTime = 2f;
    private void Awake()
    {
        _objSpawnPos = transform.position;
    }


    private void OnTriggerEnter(Collider other) // ��븦 ������ ���
    {
        if(other.CompareTag("Player")) 
            // �ٵ� �̷��� �׳� �� �ո� ��Ƶ� ���� �� ������? ������Ʈ ���忡�� �� �ڽ��� ��� ���ܽ��Ѿ��ұ�? ����ó�� �ʿ���
        {
            SoundManager.Instance.PlaySFXSound("¸�׶�!"); //�ش� ���� //�ٵ� �̰͵� ����ȭ �ؾ� �� ��
            PhotonNetwork.Destroy(gameObject);
            // other�� ȭ�� ��İ� �ϱ�
            Stun();
            StartCoroutine(FadeIn(0, 1));
            Invoke("Respawn", _fadeTime);
        }
    }

    public void OnTriggerExit(Collider other) // ���Ĺ��� ���
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
            Respawn();
        }
    }

    public void Stun()
    {
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate("bottle", _objSpawnPos, Quaternion.identity);
    }

    IEnumerator FadeIn(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            // ���� ȭ�� = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / _fadeTime);
        }

        // ���� ȭ�� = Mathf.Lerp(startAlpha, endAlpha, 1);
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        yield return null;
    }
}
