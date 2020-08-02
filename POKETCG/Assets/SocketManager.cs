﻿using System.Collections;
using UnityEngine;
using SocketIO;
using System; //using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{
    public GameObject socketIO;
    public int keyidx;
    Dictionary<string, string> key = new Dictionary<string, string>();
    Dictionary<string, string> sid = new Dictionary<string, string>();
    public GameObject tx;

    private SocketIOComponent socket;

    public void Start()
    {
        //if(Application.internetReachability != NetworkReachability.NotReachable) {
            Instantiate(socketIO);

            GameObject go = GameObject.Find("SocketIO(Clone)");
            socket = go.GetComponent<SocketIOComponent>();

            socket.On("open", OnSocketOpen);

            socket.On("joinRoom", joinRoom);
            socket.On("StartCyto", StartCyto);
            socket.On("error", Error);
            socket.On("close", Close);
            PlayerPrefs.SetInt("GM", 1);
            StartCoroutine("BeepBoop");
        //}
        if (PlayerPrefs.GetInt("Gone") == 1)
            StartCoroutine("TextCoroutine");

    }
    public void StartCyto(SocketIOEvent e)
    {
        SceneManager.LoadScene(1);
    }
    public void OnSocketOpen(SocketIOEvent ev)
    {
    }
    public void GameStart()
    {
        if (PlayerPrefs.GetInt("GM") == 1)
        {
            MatchingCheck.Instance.isMatching = true;
            sid["sid"] = socket.sid;
            socket.Emit("joinRoom", new JSONObject(sid));
        }
        else if(PlayerPrefs.GetInt("GM") == 0)
            SceneManager.LoadScene(2);
        else if (PlayerPrefs.GetInt("GM") == 2)
            SceneManager.LoadScene(3);
    }
    public void joinRoom(SocketIOEvent e)
    {
        String a = string.Format(e.data[0].ToString());
        a = a.Replace('"', ' ');
        a = a.Trim();
        keyidx = int.Parse(a);
        key["key"] = keyidx.ToString();
        PlayerPrefs.SetString("key", key["key"]);
        Debug.Log("joinRoom " + "||" + e.data);
    }
    public void CancleJoin() // 취소버튼 누르면 실행
    {
        if (MatchingCheck.Instance.isMatching == true)
        {
            MatchingCheck.Instance.isMatching = false;
            socket.Emit("CanceljoinRoom", new JSONObject(key));
        }
    }
    private void OnApplicationQuit()
    {
        CancleJoin();
    }


    public void Error(SocketIOEvent e)
    {
        Debug.Log("SocketIO Error received: " + e.name + "||" + e.data);
    }

    public void Close(SocketIOEvent e)
    {
        key["id"] = socket.sid;
        socket.Emit("leaveRoom", new JSONObject(key));
        Debug.Log("SocketIO Close received: " + e.name + " " + e.data);
    }
    IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    IEnumerator TextCoroutine()
    {
        tx.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        tx.SetActive(false);
        PlayerPrefs.SetInt("Gone", 0);
    }
    IEnumerator BeepBoop()
    {
        yield return new WaitForSeconds(0.1f);
        MatchingCheck.Instance.isMatching = true;
        sid["sid"] = socket.sid;
        socket.Emit("joinRoom", new JSONObject(sid));
    }
}
