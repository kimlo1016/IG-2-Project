using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingManager : MonoBehaviour
{
    public Dictionary<string, float> BettingOneList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingTwoList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingThreeList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingFourList = new Dictionary<string, float>();

    public float BetAmount;
    public float[] BetRate;
    public float[] ChampionBetAmounts;

    //�й�(DB�� ���), ���� ����,���� �˸�, �ݾ� ������, Dictionary���� ���� ã�Ƽ� ��ȯ.

    private void Awake()
    {
        ;
    }

    private void DistributeGold()
    {

    }

    private void BettingStart()
    {

    }

    private void BettingEnd()
    {

    }

    private void FindBetting()
    {

    }

    private void ResetAllBetting()
    {
        
    }
}
