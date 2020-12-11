using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_Select : MonoBehaviour
{
    public int chNum;

    public void Click()
    {
        PlayerPrefs.SetInt("PC", chNum);
    }
}
