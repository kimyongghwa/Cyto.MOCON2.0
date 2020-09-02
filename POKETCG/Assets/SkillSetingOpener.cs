using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetingOpener : MonoBehaviour
{
    public GameObject skillPanel;
    public GameObject checkButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (skillPanel.activeSelf && PlayerPrefs.GetInt("WarriorEquipNum") == 5 && PlayerPrefs.GetInt("MageEquipNum") == 5 && PlayerPrefs.GetInt("ArcherEquipNum") == 5)
            skillPanel.SetActive(!skillPanel.activeSelf);
        else if (!skillPanel.activeSelf)
            skillPanel.SetActive(!skillPanel.activeSelf);
        else if (skillPanel.activeSelf)
            checkButton.SetActive(true);
    }
}
