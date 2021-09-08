using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldSystem : MonoBehaviour
{
    public Text goldText;
    public int gold;

    private void Start() {
        if(PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
            goldText.text = gold+" GOLD";
        }
    }

    public void UpdateGold(int value)
    {
        gold += value;
        goldText.text = gold+" GOLD";

        PlayerPrefs.SetInt("Gold",gold);
    }
}
