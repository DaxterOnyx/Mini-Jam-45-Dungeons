using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankDataScript : MonoBehaviour
{
    [SerializeField]
    private BankData bankData;
    private static BankDataScript _instance;
    public static BankDataScript Instance
    {
        get {
            if (_instance == null)
                _instance = FindObjectOfType<BankDataScript>();
            return _instance;
        }
    }

    public int gems => bankData.gems;
    public int level => bankData.level;
    public int xp => bankData.xp;
    public void addGems(int i)
    {
        bankData.gems += i;
    }

    public bool payGems(int i)
    {
        if (i > bankData.gems)
            return false;
        bankData.gems -=  i;
        return true;
    }
  
    public void addLevel(int i)
    {
        bankData.level += i;
    }

    public bool payLevel(int i)
    {
        if (i > bankData.level)
            return false;
        bankData.level -= i;
        return true;
    }

    public void addXP(int i)
    {
        bankData.xp += i;
    }

}
