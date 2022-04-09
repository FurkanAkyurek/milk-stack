using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private Text moneyText;

    void Update()
    {
        moneyText.text = PlayerPrefs.GetInt("Money").ToString();
    }
}
