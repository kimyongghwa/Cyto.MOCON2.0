using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dondes : MonoBehaviour
{
    static Dondes instance;
    public static Dondes Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
