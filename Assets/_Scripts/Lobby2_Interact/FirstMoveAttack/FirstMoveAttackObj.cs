using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private void Awake()
    {
        _objSpawnPos = transform.position;
    }


    private void OnTriggerEnter(Collider other) // ��� �Ӹ� ������ ���
    {
        if(other.CompareTag("Player")) // �ٵ� �̷��� �׳� �� �ո� ��Ƶ� ���� �� ������? ������Ʈ ���忡�� �� �ڽ��� ��� ���ܽ��Ѿ��ұ�?
        {
            SoundManager.Instance.PlaySFXSound("¸�׶�!/"); //�ش� ����
            PhotonNetwork.Destroy(gameObject);
            Invoke("Respawn", 2f);
            Stun();
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
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
        PhotonNetwork.Instantiate("bottle", _objSpawnPos, Quaternion.identity);
    }

}
