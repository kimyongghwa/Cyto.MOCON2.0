using System.Collections;
using UnityEngine;
using SocketIO;
using System; //using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    
    public bool isChecked;
    public bool isEnemeChecked;
    public CardManager e_cardManager;
    public GameObject[] pc = new GameObject[3];
    public GameObject[] eneme = new GameObject[3];
    public GameObject[] eskill = new GameObject[3];
    public GameObject[] skill = new GameObject[3];
    public GameObject gg;
    public GameObject de;
    GameObject canvas;
    public GameObject battleScene;
    public bool isAi;
    public bool isMulti;
    public bool isMultiEneme;
    public int[] haveCard = new int[4];
    public AISkill ai;
    public int[] CardNum = new int[5];
    public Animator[] animator = new Animator[4];
    public Image timerImage;
    float tCount = 5f;

    private int keyidx; //room 번호를 서버로부터 받아서 저장할 변수
    Dictionary<string, string> key = new Dictionary<string, string>(); //key 딕셔너리

    private SocketIOComponent socket; //소켓 선언




    public void StartServer()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();


        socket.On("joinRoom", joinRoom);

        socket.On("OpponentCard", (SocketIOEvent e) => {
            Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
            Debug.Log("organic");
            for (int i = 1; i < 5; i++)
            {
                e_cardManager.CardNum[i] = 0;
            }
            for (int i = 0; i<4; i++)
            {
                int a = (int)char.GetNumericValue(e.data[i].ToString()[1]);
                e_cardManager.haveCard[i] = a;
                e_cardManager.CardNum[a]++;
                e_cardManager.animator[i].SetInteger("CardType", a);
                Debug.Log("card a :" + a + " num :" + i);
            }
        });

        socket.On("OpponentCharacter", (SocketIOEvent e) => {
            if (!isMultiEneme)
            {
                Debug.Log(this.gameObject.name);
                Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
                Debug.Log((int)char.GetNumericValue(e.data[0].ToString()[1]));
                int a = (int)char.GetNumericValue(e.data[0].ToString()[1]);
                //상대 카드를 딕셔너리로 받아옴
                GameObject b = Instantiate(eneme[a], battleScene.transform);
                BattleManager.Instance.otherInfo = b.GetComponent<PlayerInfo>();
                Instantiate(eskill[a], canvas.transform);
            }
        });

        socket.On("OpponentSkill", (SocketIOEvent e) => {
            Debug.Log(string.Format(e.data[0].ToString()));
            String a = string.Format(e.data[0].ToString()); 
            a = a.Replace('"', ' ');
            a = a.Trim();
            Debug.Log(a);
            Debug.Log("abc");
            Debug.Log(GameObject.Find(a));
            //상대 카드를 딕셔너리로 받아옴
            BattleManager.Instance.EnemeCard = GameObject.Find(a).GetComponent<CardInfo>();
        });

        socket.On("OpponentCheck", (SocketIOEvent e) => {
            Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
            //상대 카드를 딕셔너리로 받아옴
            isEnemeChecked = true;
            if (isEnemeChecked && isChecked)
            {
                isEnemeChecked = false;
                isChecked = false;
                DealGyo();
                Reroll();
            }
        });


        socket.On("error", Error);
        socket.On("close", Close);
        socket.On("OpponentLeft", OpponentLeft);
        socket.On("StartRoop", StartRoop);


        //StartCoroutine("BeepBoop");
    }

    //private IEnumerator BeepBoop()
    //{
    //    // wait 1 seconds and continue
    //    yield return new WaitForSeconds(1);

    //    socket.Emit("beep");

    //    // wait 3 seconds and continue
    //    yield return new WaitForSeconds(3);

    //    socket.Emit("beep");

    //    // wait 2 seconds and continue
    //    yield return new WaitForSeconds(2);

    //    socket.Emit("beep");

    //    // wait ONE FRAME and continue
    //    yield return null;

    //    socket.Emit("beep");
    //    socket.Emit("beep");
    //}

    public void joinRoom(SocketIOEvent e)
    {
        //joinRoom이 발생하면 room 번호를 받고 keyidx에 room번호를 넣는다
        String a = string.Format(e.data[0].ToString());
        a = a.Replace('"', ' ');
        a = a.Trim();
        keyidx = int.Parse(a);
        Debug.Log(keyidx);
        key["key"] = keyidx.ToString();
        Debug.Log("joinRoom " +"||" + e.data);
    }



    public void Error(SocketIOEvent e)
    {
        Debug.Log("SocketIO Error received: " + e.name + "||" + e.data);
    }

    public void Close(SocketIOEvent e)
    {
        Debug.Log("SocketIO Close received: " + e.name + " " + e.data);
    }

    public void OpponentLeft(SocketIOEvent e)
    {
        //상대가 떠났습니다 띄우고
        SceneManager.LoadScene(0);
    }

    public void StartRoop(SocketIOEvent e)
    {
        Debug.Log("Sodsfasdfsadfffffffffffffff");
        key["id"] = socket.sid;
        socket.Emit("Alive", new JSONObject(key));
    }

    public void CloseClick() //메인화면으로 나가기 버튼
    {
        key["id"] = socket.sid;
        socket.Emit("leaveRoom", new JSONObject(key));
        Debug.Log("leaveRoom");
    }

    private void Awake()
    {
        keyidx = int.Parse(PlayerPrefs.GetString("key"));
        Debug.Log(keyidx);
        if (isMulti && !isMultiEneme)
        {
            StartServer();
            key["key"] = PlayerPrefs.GetString("key");
        }
        else if (isMultiEneme)
        {
            GameObject go = GameObject.Find("SocketIO");
            socket = go.GetComponent<SocketIOComponent>();
        }
    }
    private void Start()
    {
        if (isMulti && !isMultiEneme) // 멀티일 경우 보내라.
        {
            Dictionary<string, string> MyCharacter = new Dictionary<string, string>();
            MyCharacter["number"] = PlayerPrefs.GetInt("PC").ToString();
            MyCharacter["key"] = keyidx.ToString();
            socket.Emit("MyCharacter", new JSONObject(MyCharacter));
            StartCoroutine("TimerCoroutine");
            Reroll();
        }
        if (!isAi && !isMultiEneme)
        {
            canvas = GameObject.Find("Canvas");
            Instantiate(skill[PlayerPrefs.GetInt("PC", 1)], canvas.transform);
            GameObject a = Instantiate(pc[PlayerPrefs.GetInt("PC", 1)], battleScene.transform);
        }
        if(!isMulti)
            Reroll();
    }
    public void SendSkill(string a)
    {
        Dictionary<string, string> MySkill = new Dictionary<string, string>();
        MySkill["number"] = a+"E"; 
        MySkill["key"] = keyidx.ToString();
        socket.Emit("MySkill", new JSONObject(MySkill));
    }
    public void SendCheck()
    {
        Dictionary<string, string> MyCheck = new Dictionary<string, string>();
        MyCheck["checking"] = "true";  
        MyCheck["key"] = keyidx.ToString();
        socket.Emit("MyCheck", new JSONObject(MyCheck));
    }
    public void Reroll() // 카드를 교체한다.
    {

        for (int i = 1; i < 5; i++)
        {
            CardNum[i] = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            animator[i].SetInteger("CardType", 0);
            haveCard[i] = UnityEngine.Random.Range(1, 5); //haveCard[i] = Random.Range(1, 5);
            CardNum[haveCard[i]]++;
            StartCoroutine("RerollCoroutine", i);
        }
        if (isMulti && !isMultiEneme) // 멀티인데 내 캐릭터일 경우 정보를 서버로 보냄 (카드 뭐나왔는지 보내야됨 ^^)
        {
            Debug.Log("Reroll");
            Dictionary<string, string> MyCard = new Dictionary<string, string>();
            MyCard["card0"] = haveCard[0].ToString();
            MyCard["card1"] = haveCard[1].ToString();
            MyCard["card2"] = haveCard[2].ToString();
            MyCard["card3"] = haveCard[3].ToString();
            MyCard["key"] = keyidx.ToString();
            socket.Emit("MyCard", new JSONObject(MyCard));
        }
        if (isAi)
        { // ai일 경우 나온 카드에 따라 사용할 기술을 정해준다.
            for (int i = ai.cards.Length - 1; i >= 0; i--)
            {
                AiSelect(i);
            }
        }
    }
    public void AiSelect(int i)
    {
        for (int j = 1; j < 5; j++)
        {
            if (ai.cards[i].cost[j] > CardNum[j])
                return;

        }
        BattleManager.Instance.EnemeCard = ai.cards[i];
    }


    public void Click()
    {
        if (!isMulti)
        {
            DealGyo();
            Reroll();
        }
        else
        {
            if (!isChecked)
            {
                isChecked = true;
                SendCheck();
                if (isEnemeChecked && isChecked)
                {
                    isEnemeChecked = false;
                    isChecked = false;
                    DealGyo();
                    Reroll();

                }
            }


        }
    }
    public void DealGyo() //딜교환부분
    {
        if (BattleManager.Instance.Card != null)
        {
            BattleManager.Instance.myInfo.guard += BattleManager.Instance.Card.getArmor;
            BattleManager.Instance.myInfo.nowHp += BattleManager.Instance.Card.hill;
        }
        if (BattleManager.Instance.EnemeCard != null)
        {
            BattleManager.Instance.otherInfo.guard += BattleManager.Instance.EnemeCard.getArmor;
            BattleManager.Instance.otherInfo.nowHp += BattleManager.Instance.EnemeCard.hill;
        }
        if (BattleManager.Instance.otherInfo.nowHp >= BattleManager.Instance.otherInfo.maxHp)
        {
            BattleManager.Instance.otherInfo.nowHp = BattleManager.Instance.otherInfo.maxHp;
        }
        if (BattleManager.Instance.myInfo.nowHp >= BattleManager.Instance.myInfo.maxHp)
        {
            BattleManager.Instance.myInfo.nowHp = BattleManager.Instance.myInfo.maxHp;
        }
        if (BattleManager.Instance.EnemeCard != null)
        {
            BattleManager.Instance.myInfo.guard -= BattleManager.Instance.EnemeCard.damage;
            if (BattleManager.Instance.myInfo.guard < 0)
            {
                BattleManager.Instance.myInfo.nowHp += BattleManager.Instance.myInfo.guard;
                BattleManager.Instance.myInfo.guard = 0;
            }
            if (BattleManager.Instance.EnemeCard.effect != null)
                Instantiate(BattleManager.Instance.EnemeCard.effect, BattleManager.Instance.myInfo.transform);
            if (BattleManager.Instance.EnemeCard.effectSelf != null)
                Instantiate(BattleManager.Instance.EnemeCard.effectSelf, BattleManager.Instance.otherInfo.transform);
            if (BattleManager.Instance.EnemeCard.damage != 0)
                StartCoroutine("PlayerHit", true);
        }
        if (BattleManager.Instance.Card != null)
        {
            BattleManager.Instance.otherInfo.guard -= BattleManager.Instance.Card.damage;
            if (BattleManager.Instance.otherInfo.guard < 0)
            {
                BattleManager.Instance.otherInfo.nowHp += BattleManager.Instance.otherInfo.guard;
                BattleManager.Instance.otherInfo.guard = 0;
            }
            if (BattleManager.Instance.Card.effect != null)
                Instantiate(BattleManager.Instance.Card.effect, BattleManager.Instance.otherInfo.transform);
            if (BattleManager.Instance.Card.effectSelf != null)
                Instantiate(BattleManager.Instance.Card.effectSelf, BattleManager.Instance.myInfo.transform);
            if (BattleManager.Instance.Card.damage != 0)
                StartCoroutine("PlayerHit", false);
        }
        BattleManager.Instance.Card = null;
        BattleManager.Instance.EnemeCard = null;
        if (BattleManager.Instance.otherInfo.nowHp <= 0) //승
        {
            de.SetActive(true);
            if (isMulti && !isMultiEneme)
            {
                key["id"] = socket.sid;
                socket.Emit("EndCyto", new JSONObject(key));
            }
        }

        if (BattleManager.Instance.myInfo.nowHp <= 0) //패
        {
            gg.SetActive(true);
            if (isMulti && !isMultiEneme)
            {
                key["id"] = socket.sid;
                socket.Emit("EndCyto", new JSONObject(key));
            }
        }
    }

    IEnumerator RerollCoroutine(int num)
    {
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 30, 0);
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 60, 0);
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 120, 0);
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 150, 0);
        yield return new WaitForSeconds(0.015f);
        animator[num].gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        animator[num].SetInteger("CardType", haveCard[num]);
    }

    IEnumerator PlayerHit(bool a)
    {
        GameObject p1 = BattleManager.Instance.otherInfo.gameObject;
        if (a)
            p1 = BattleManager.Instance.myInfo.gameObject;
        yield return new WaitForSeconds(0.03f);
        p1.transform.localRotation = Quaternion.Euler(0, 90, 0);
        yield return new WaitForSeconds(0.03f);
        p1.transform.localRotation = Quaternion.Euler(0, 180, 0);
        yield return new WaitForSeconds(0.03f);
        p1.transform.localRotation = Quaternion.Euler(0, 270, 0);
        yield return new WaitForSeconds(0.03f);
        p1.transform.localRotation = Quaternion.Euler(0, 360, 0);
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        tCount -= 0.1f;
        timerImage.fillAmount =  tCount / 5.0f;
        if(tCount <= 0)
        {
            Click();
            tCount = 5;
        }
        StartCoroutine("TimerCoroutine");
    }
}