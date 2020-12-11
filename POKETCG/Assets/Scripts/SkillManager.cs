using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    static SkillManager instance;
    public static SkillManager Instance{
        get{
            return instance;
        }
    }
    public bool battleScene;
    public int equipWarrior;
    public int equipMage;
    public int equipArcher;
    public Text[] chText = new Text[3];
    public GameObject[] warriorGameObjects = new GameObject[5];
    public GameObject[] mageGameObjects = new GameObject[5];
    public GameObject[] archerGameObjects = new GameObject[5];
    public GameObject[] ewarriorGameObjects = new GameObject[5];
    public GameObject[] emageGameObjects = new GameObject[5];
    public GameObject[] earcherGameObjects = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        if(!battleScene)
        for(int i =0; i<warriorGameObjects.Length; i++)
        {

            if ( PlayerPrefs.GetInt("HaveWarriorSkill" + i, 0) == 1)
            {
                warriorGameObjects[i].SetActive(true);
            }
            if (PlayerPrefs.GetInt("HaveMageSkill" + i, 0) == 1)
            {
                mageGameObjects[i].SetActive(true);
            }
            if (PlayerPrefs.GetInt("HaveArcherSkill" + i, 0) == 1)
            {
                archerGameObjects[i].SetActive(true);
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Update()
    {
        if (!battleScene)
        {
            chText[0].text = "전사 (" + PlayerPrefs.GetInt("WarriorEquipNum") + " / 5)";
            chText[1].text = "마법사 (" + PlayerPrefs.GetInt("MageEquipNum") + " / 5)";
            chText[2].text = "궁수 (" + PlayerPrefs.GetInt("ArcherEquipNum") + " / 5)";
        }
    }
}
