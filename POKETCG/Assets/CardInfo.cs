using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour
{
    public int chNum;
    public int cardNum;
    public bool guardAttack;
    public Image thisImage;
    public Color color;
    public Color color2;
    public bool enemeCard;
    CardManager cm;
    CardManager csm; // socket 처리
    public int[] cost = new int[5];
    public int damage;
    public int getArmor;
    public int getMiss;
    public int hill;
    public bool armorBreak;
    public GameObject effect;
    public GameObject effectSelf;

    public void Click()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            for (int i = 1; i < 5; i++)
            {
                if (cost[i] > cm.CardNum[i])
                    return;
            }
            if (!enemeCard)
            {
                BattleManager.Instance.Card = this;
                if (cm.isMulti)
                    csm.SendSkill(this.name);
            }
            else
                BattleManager.Instance.EnemeCard = this;
        }
        else
        {
            switch (chNum)
            {
                case 1:
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myWarriorSkill" + i) == cardNum)
                        {
                            PlayerPrefs.SetInt("myWarriorSkill" + i, 0);
                            PlayerPrefs.SetInt("WarriorEquipNum", PlayerPrefs.GetInt("WarriorEquipNum", 5)-1);
                            Debug.Log(PlayerPrefs.GetInt("WarriorEquipNum" ));
                            thisImage.color = color;
                            return;
                        }
                    }
                    for (int i =0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myWarriorSkill" + i) == 0)
                        {
                            PlayerPrefs.SetInt("myWarriorSkill" + i, cardNum);
                            PlayerPrefs.SetInt("WarriorEquipNum", PlayerPrefs.GetInt("WarriorEquipNum", 5) + 1);
                            Debug.Log(PlayerPrefs.GetInt("WarriorEquipNum"));
                            break;
                        }
                    }
                    break;
                case 2:
                    PlayerPrefs.SetInt("WarriorEquipNum", 5);
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myMageSkill" + i) == cardNum)
                        {
                            PlayerPrefs.SetInt("myMageSkill" + i, 0);
                            PlayerPrefs.SetInt("MageEquipNum", PlayerPrefs.GetInt("MageEquipNum", 5) - 1);
                            Debug.Log(PlayerPrefs.GetInt("MageEquipNum"));
                            thisImage.color = color;
                            return;
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myMageSkill" + i) == 0)
                        {
                            PlayerPrefs.SetInt("myMageSkill" + i, cardNum);
                            PlayerPrefs.SetInt("MageEquipNum", PlayerPrefs.GetInt("MageEquipNum", 5) + 1);
                            Debug.Log(PlayerPrefs.GetInt("MageEquipNum"));
                            break;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myArcherSkill" + i) == cardNum)
                        {
                            PlayerPrefs.SetInt("myArcherSkill" + i, 0);
                            PlayerPrefs.SetInt("ArcherEquipNum", PlayerPrefs.GetInt("ArcherEquipNum", 5) - 1);
                            thisImage.color = color;
                            return;
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myArcherSkill" + i) == 0)
                        {
                            PlayerPrefs.SetInt("myArcherSkill" + i, cardNum);
                            PlayerPrefs.SetInt("ArcherEquipNum", PlayerPrefs.GetInt("ArcherEquipNum", 5) + 1);
                            break;
                        }
                    }
                    break;
            }
        }
    }

    public void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!enemeCard)
                cm = GameObject.Find("Manager").GetComponent<CardManager>();
            else
                cm = GameObject.Find("EnemeCardManager").GetComponent<CardManager>();
            csm = GameObject.Find("Manager").GetComponent<CardManager>();
        }
      
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (guardAttack && enemeCard)
                damage = BattleManager.Instance.otherInfo.guard;
            else if (guardAttack && !enemeCard && BattleManager.Instance.myInfo != null)
                damage = BattleManager.Instance.myInfo.guard;
            if (BattleManager.Instance.Card == this)
                thisImage.color = new Color(255, 0, 0, 255);
            else if (!enemeCard)
            {
                for (int i = 1; i < 5; i++)
                {
                    if (cost[i] > cm.CardNum[i])
                    {
                        thisImage.color = color;
                        return;
                    }
                }
                thisImage.color = color2;
            }

        }
        else
        {
            switch (chNum)
            {
                case 1:
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myWarriorSkill" + i) == cardNum)
                        {
                            thisImage.color = new Color(255, 0, 0, 255);
                            break;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myMageSkill" + i) == cardNum)
                        {
                            thisImage.color = new Color(255, 0, 0, 255);
                            break;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < 5; i++)
                    {
                        if (PlayerPrefs.GetInt("myArcherSkill" + i) == cardNum)
                        {
                            thisImage.color = new Color(255, 0, 0, 255);
                            break;
                        }
                    }
                    break;
            }
        }
    }
}

