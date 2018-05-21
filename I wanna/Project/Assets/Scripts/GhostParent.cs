using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GhostParent : MonoBehaviour {

    //プレイヤー
    [SerializeField]
    private Player player;

    //ゴースト
    [SerializeField]
    private GameObject ghost;

    [SerializeField]
    private GameObject startPoint;

    //保存できる最大数
    [SerializeField]
    private int maxData = 10000;

    //動きを保存する間隔
    [SerializeField]
    private float duration = 0.005f;

    //Animationコンポーネント
    private Animator animator;

    //ゴーストオブジェクト
    public GameObject[] ghostObj = new GameObject[100];

    //ゴーストのデータ
    public GhostData[] ghostData = new GhostData[100];

    //動きを保存するフラグ
    private bool isReplay;

    //再生中かどうか
    private bool isPlay;

    //プレイヤーの死亡数
    public int death = 0;

    private int i = 0;

    //経過時間
    private float elpsedTime = 0f;

    int[] hoge3;

    //ゴーストデータクラス
    //[SerializeField]
    public class GhostData
    {
        //位置のリスト
        public List<Vector3> posLists = new List<Vector3>();
    }
    // Use this for initialization
    void Start ()
    {
        Init();
    }

    // Update is called once per frame
    void Update ()
    {
        hoge3[0] = death;
        ReplaySave();
    }

    void Init()
    {
        animator = player.GetComponent<Animator>();

        SaveStart();
        
    }

    void ReplaySave()
    {

        if (isReplay)
        {
            elpsedTime += Time.deltaTime;

            if (elpsedTime >= duration)
            {
                ghostData[death].posLists.Add(player.transform.position);

                elpsedTime = 0f;

                //データの保存数が最大数を超えたら保存をやめる
                if (ghostData[death].posLists.Count >= maxData)
                {
                    SaveStop();
                }
            }                
        }
    }

    //記録開始
    public void SaveStart()
    {
        isReplay = true;
        elpsedTime = 0f;
        ghostData[death] = new GhostData();
        
        Debug.Log("保存開始");
    }
    //記録停止
    public void SaveStop()
    {
        isReplay = false;

        Debug.Log("保存停止");
    }
    //ゴースト再生
    public void GhostStart()
    {

        SaveStop();
        Debug.Log("ゴースト始動");

        //ゴーストをプレイヤーの初期位置に生成
        foreach (int hoge in hoge3)
        {
            ghostObj[death - 1] = Instantiate(ghost, startPoint.transform.position, Quaternion.identity);

            if (ghostData[death - 1] == null)
            {
                Debug.Log("データがないよ");
            }
            else
            {
                isPlay = true;
                ghostObj[death - 1].transform.position = ghostData[death - 1].posLists[0];
                StartCoroutine(GhostPlay());
                //ghostObj[death - 1].transform.position = ghostData[death - 1].posLists[0];
            }
        }
        for (int j = 0; j < death; j++)
        {
            ghostObj[j].SetActive(true);

        }

    }
    //ゴースト再生を停止
    public void GhostStop()
    {
        Debug.Log("ゴースト停止");
        isPlay = false;
        Destroy(ghostObj[death - 1]);
        //ghostObj[death - 1].SetActive(false);
    }

    IEnumerator GhostPlay()
    {
        

        Debug.Log("データ数: " + ghostData[death - 1].posLists.Count);

        while (isPlay)
        {
            //var i = 0;

            yield return new WaitForSeconds(duration);

            ghostObj[death - 1].transform.position = ghostData[death - 1].posLists[i];
            
            i++;

            //保存データ数を超えたら再生をやめる
            if (i >= ghostData[death - 1].posLists.Count)
            {
                GhostStop();
                //ghostObj[num].transform.position = ghostData.posLists[0];
                //i = 0;
            }
        }
       
    }

    public void PlayerDeath()
    {

        //記録を停止
        SaveStop();

        //死亡回数を増やす
        death++;

        //ゴーストを生成
        GhostStart();

        //記録を開始
        SaveStart();
    }

    void OnApplicationQuit()
    {
        Debug.Log("アプリケーション終了");
    }
}

