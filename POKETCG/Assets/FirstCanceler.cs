using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCanceler : MonoBehaviour
{
    public void Click(){
        PlayerPrefs.SetInt("First", 1);
        }

}
