using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public Text levelText;
    public int level = 1;

    private void Start() {
        if(PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
            levelText.text = "LEVEL "+level;
        }
        else
        {
            PlayerPrefs.SetInt("level",1);
            level = 1;
        }
    }

    public void LevelUp()
    {
        level++;
        PlayerPrefs.SetInt("level",level);
    }
}
