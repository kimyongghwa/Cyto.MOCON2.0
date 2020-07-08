using System.Collections;
using UnityEngine;
using SocketIO;
using System; //using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{
    public int keyidx;
    Dictionary<string, string> key = new Dictionary<string, string>();
    Dictionary<string, string> sid = new Dictionary<string, string>();

    private SocketIOComponent socket;

    public void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", OnSocketOpen);

        socket.On("joinRoom", joinRoom);
        socket.On("CytoStart", cytoStart);
        socket.On("error", Error);
        socket.On("close", Close);

        //StartCoroutine("BeepBoop");
    }
    public void cytoStart(SocketIOEvent e)
    {
        SceneManager.LoadScene(1);
    }
    public void OnSocketOpen(SocketIOEvent ev)
    {
    }
    public void GameStart()
    {
        sid["sid"] = socket.sid;
        socket.Emit("joinRoom", new JSONObject(sid));
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

}
