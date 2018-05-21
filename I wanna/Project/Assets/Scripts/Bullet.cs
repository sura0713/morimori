using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //Vector2 コンポーネント
    Vector2 vector2;

    //Rigidbody2D コンポーネント
    Rigidbody2D rigidbody2d;

    //左右どっちに飛んでいくかのトリガー
    public bool trg;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (trg == true)
        {
            //プレイヤーが右向きだったら右に発射
            Move(0f, 16f);
        }
        else
        {
            //プレイヤーが左向きだったら左に発射
            Move(180f, 16f);
        }
    }

    void Move(float direction, float speed)
    {
        vector2.x = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
        vector2.y = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;
        rigidbody2d.velocity = vector2;
    }

}
