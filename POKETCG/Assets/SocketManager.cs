using System.Collections;
using UnityEngine;
using SocketIO;
using System; //using System;
using System.Collections.Generic;

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

        socket.On("error", Error);
        socket.On("close", Close);

        //StartCoroutine("BeepBoop");
    }

    public void OnSocketOpen(SocketIOEvent ev)
    {
        Debug.Log("updated socket id " + socket.sid);
        sid["sid"] = socket.sid;
        socket.Emit("joinRoom", new JSONObject(sid));
    }

    public void joinRoom(SocketIOEvent e)
    {
        Debug.Log("adsf");
        key["key"] = keyidx.ToString();
        Debug.Log(e.name + "||||" + e.data);
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
