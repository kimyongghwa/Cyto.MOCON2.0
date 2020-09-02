using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel;
    public bool tutorial;

    public void OnMouseEnter()
    {
        panel.SetActive(true);
    }
    public void OnMouseExit()
    {
        panel.SetActive(false);
    }
    public void Click()
    {
        if (!tutorial)
            panel.SetActive(!panel.activeSelf);
        else if (tutorial && PlayerPrefs.GetInt("GM") == 1)
            panel.SetActive(!panel.activeSelf);
    }
    public void OgClick()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
