using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BankData", menuName = "Data/BankData")]
public class BankData : ScriptableObject
{
    public int gems;
    public int level;
    public int xp;
}
