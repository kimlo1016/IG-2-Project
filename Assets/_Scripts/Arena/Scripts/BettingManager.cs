using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingManager : MonoBehaviour
{
    public Dictionary<string, double> BettingOneList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingTwoList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingThreeList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingFourList = new Dictionary<string, double>();

    public double BetAmount;
    public double[] BetRates;
    public double[] ChampionBetAmounts;

    //�й�(DB�� ���), ���� ����, ���� �˸�, �ݾ� ������, Dictionary���� ���� ã�Ƽ� ��ȯ.

    private void Awake()
    {
        
    }

    private void DistributeGold()
    {

    }

    private void BettingStart()
    {

    }

  
    private void BettingEnd()
    {
        ResetAllBetting();
    }


    private void ResetAllBetting()
    {
        BetAmount = 0;
        BettingOneList.Clear();
        BettingTwoList.Clear();
        BettingThreeList.Clear();
        BettingFourList.Clear();

        for(int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
        }
    }
}
