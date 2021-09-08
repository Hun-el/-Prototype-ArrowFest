using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    GameManager gameManager;
    ArrowsObject arrowsObject;
    GoldSystem goldSystem;

    public Text arrowText,incomeText;

    int arrowCost,incomeCost;

    int arrowLevel = 1,incomeLevel = 1;

    private void Start() {
        goldSystem = FindObjectOfType<GoldSystem>();
        arrowsObject = FindObjectOfType<ArrowsObject>();
        gameManager = FindObjectOfType<GameManager>();

        if(PlayerPrefs.HasKey("arrowLevel"))
        {
            arrowLevel = PlayerPrefs.GetInt("arrowLevel");
            arrowCost = gameManager.startingFee + ((arrowLevel - 1) * gameManager.Increase);

            arrowText.text = "LEVEL "+arrowLevel+" | "+arrowCost;
        }
        else
        {
            PlayerPrefs.SetInt("arrowLevel",1);
            arrowCost = gameManager.startingFee;

            arrowText.text = "LEVEL "+arrowLevel+" | "+arrowCost;
        }


        if(PlayerPrefs.HasKey("incomeLevel"))
        {
            incomeLevel = PlayerPrefs.GetInt("incomeLevel");
            incomeCost = gameManager.startingFee + ((incomeLevel - 1) * gameManager.Increase);

            incomeText.text = "LEVEL "+incomeLevel+" | "+incomeCost;
        }
        else
        {
            PlayerPrefs.SetInt("incomeLevel",1);
            incomeCost = gameManager.startingFee;

            incomeText.text = "LEVEL "+incomeLevel+" | "+incomeCost;
        }
    }

    public void upgradeArrow()
    {
        if(goldSystem.gold >= arrowCost)
        {
            goldSystem.UpdateGold(-arrowCost);

            arrowLevel++;
            arrowCost = gameManager.startingFee + ((arrowLevel - 1) * gameManager.Increase);

            arrowText.text = "LEVEL "+arrowLevel+" | "+arrowCost;

            PlayerPrefs.SetInt("arrowLevel",arrowLevel);
            arrowsObject.AddNewStack();
        }
    }

    public void upgradeIncome()
    {
        if(goldSystem.gold >= incomeCost)
        {
            goldSystem.UpdateGold(-incomeCost);

            incomeLevel++;
            incomeCost = gameManager.startingFee + ((incomeLevel - 1) * gameManager.Increase);

            incomeText.text = "LEVEL "+incomeLevel+" | "+incomeCost;

            PlayerPrefs.SetInt("incomeLevel",incomeLevel);
        }
    }
}
