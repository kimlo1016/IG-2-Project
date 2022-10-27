using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private Transform _miningUI;
    [SerializeField]
    private GameObject _miningButton;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _miningButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetChildTrans();
        }
    }
    public void GetChildTrans()
    {
        for(int i = 0; i < _miningUI.childCount; i++)
        {
            _miningUI.GetChild(i).gameObject.SetActive(false);
        }
    }
}
