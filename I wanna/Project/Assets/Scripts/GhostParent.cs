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

    //初期出現座標
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
    private GhostData[] ghostData = new GhostData[100];

    //動きを保存するフラグ
    private bool isReplay;

    //再生中かどうか
    private bool isPlay;

    //プレイヤーの死亡数
    private int death;

    //経過時間
    private float elpsedTime = 0f;

    //ゴーストデータクラス
    [System.Serializable]
    private class GhostData
    {
        //位置のリスト
        public List<Vector3> posLists = new List<Vector3>();
    }
    [System.Serializable]
    private class Ghost
    {
        //ゴーストオブジェクト
        public GameObject[] ghostObj = new GameObject[100];
        int i = 0;
    }
    // Use this for initialization
    void Start ()
    {
        Init();
    }

    // Update is called once per frame
    void Update ()
    {
        ReplaySave();
    }

    void Init()
    {
        animator = player.GetComponent<Animator>();

        death = 0;

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
        Debug.Log("ゴースト始動");

        //ゴーストをプレイヤーの初期位置に生成
        ghostObj[death - 1] = Instantiate(ghost, startPoint.transform.position, Quaternion.identity);

        if (ghostData[death - 1] == null)
        {
            Debug.Log("データがないよ");
        }
        else
        {
            isPlay = true;
            for (int j = 0; j < death; j++)
            {
                ghostObj[j].transform.position = ghostData[j].posLists[0];
            }
            StartCoroutine(GhostPlay());
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
        ghostObj[death - 1].SetActive(false);
    }

    IEnumerator GhostPlay()
    {
       
        var i = 0;

        Debug.Log("データ数: " + ghostData[death - 1].posLists.Count);

        while (isPlay)
        {
            yield return new WaitForSeconds(duration);

            for (int j = 0; j < death; j++)
            {
                ghostObj[j].transform.position = ghostData[j].posLists[i];
            }

            i++;

            for (int j = 0; j < death; j++)
            {
                //保存データ数を超えたら再生をやめる
                if (i >= ghostData[j].posLists.Count)
                {
                    //ghostObj[j].GhostStop();
                    ghostObj[j].SetActive(false);
                    i = 0;
                }
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

