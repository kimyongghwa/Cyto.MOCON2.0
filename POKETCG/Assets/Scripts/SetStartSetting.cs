using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartSetting : MonoBehaviour
{
    public void Start()
    {
        if (PlayerPrefs.GetInt("skillSet") == 0)
        {

            {
                for (int i = 0; i < 5; i++)
                {
                    PlayerPrefs.SetInt("myWarriorSkill" + i, i + 1);
                    PlayerPrefs.SetInt("myArcherSkill" + i, i + 1);
                    PlayerPrefs.SetInt("myMageSkill" + i, i + 1);
                }
                PlayerPrefs.SetInt("WarriorEquipNum", 5);
                PlayerPrefs.SetInt("MageEquipNum", 5);
                PlayerPrefs.SetInt("ArcherEquipNum", 5);
            }
            PlayerPrefs.SetInt("skillSet", 1);
        }
    }
}
