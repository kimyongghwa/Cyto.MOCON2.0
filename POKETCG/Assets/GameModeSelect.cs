using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelect : MonoBehaviour
{
   public void ClickT()
    {
        PlayerPrefs.SetInt("GM", 0);
    }
    public void Click()
    {
        PlayerPrefs.SetInt("GM", 1);
    }
}
