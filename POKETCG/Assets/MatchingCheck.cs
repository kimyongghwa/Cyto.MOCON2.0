using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingCheck : MonoBehaviour
{
    public bool isMatching;
    static MatchingCheck instance;
    public static MatchingCheck Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }

    }
}
