using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemShower : MonoBehaviour
{
    public Text text;
    public BankDataScript bankDataScript;
    // Start is called before the first frame update
    void Start()
    {
        bankDataScript = BankDataScript.Instance;
        text.text = bankDataScript.gems.ToString();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
