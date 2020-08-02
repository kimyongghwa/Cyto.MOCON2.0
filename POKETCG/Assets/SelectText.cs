using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectText : MonoBehaviour
{
    public Text texxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("GM") == 0)
            texxt.text = "튜토리얼";
        if (PlayerPrefs.GetInt("GM") == 1)
            texxt.text = "빠른대전";
        if (PlayerPrefs.GetInt("GM") == 2)
            texxt.text = "싱글플레이";
    }
}
