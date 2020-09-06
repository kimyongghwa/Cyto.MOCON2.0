using System.Collections;
using UnityEngine;
using SocketIO;
using System; //using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{
    public GameObject socketIO;
    public bool noInternetMatching;
    public int keyidx;
    public GameObject firstEyes;
    Dictionary<string, string> key = new Dictionary<string, string>();
    Dictionary<string, string> sid = new Dictionary<string, string>();
    Dictionary<string, string> flagcheck = new Dictionary<string, string>();
    public GameObject tx;

    private SocketIOComponent socket;

    public void Start()
    {
        PlayerPrefs.SetInt("First", 0);
        //if(Application.internetReachability != NetworkReachability.NotReachable) {
        Instantiate(socketIO);

            GameObject go = GameObject.Find("SocketIO(Clone)");
            socket = go.GetComponent<SocketIOComponent>();

            socket.On("open", OnSocketOpen);
            socket.On("testRoom",testRoom);
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
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                MatchingCheck.Instance.isMatching = true;
                sid["sid"] = socket.sid;
                socket.Emit("joinRoom", new JSONObject(sid));
            }
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
        CancleJoin();
    }
    public void testRoom(SocketIOEvent e)
    {
        Debug.Log("testRoomReceived");
    }

    public void Close(SocketIOEvent e)
    {
        key["id"] = socket.sid;
        CancleJoin();
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
        yield return new WaitForSeconds(0.01f);
        Debug.Log("BeepBoop");
        Debug.Log(PlayerPrefs.GetInt("First"));
        if (PlayerPrefs.GetInt("First", 0) == 0)
        {
            Debug.Log("JoinRoom BeepBoop");
            MatchingCheck.Instance.isMatching = true;
            sid["sid"] = socket.sid;
            socket.Emit("joinRoom", new JSONObject(sid));
            firstEyes.SetActive(true);
        }
    }
}