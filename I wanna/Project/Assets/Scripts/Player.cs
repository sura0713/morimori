using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private GhostParent ghost;

    //発射する弾
    [SerializeField]
    private GameObject shot;

    //死亡時に出現するオブジェクト
    [SerializeField]
    private GameObject dead;

    //プレイヤーのスタート位置
    [SerializeField]
    private GameObject startPoint;

    //Rigidbody2D コンポーネント
    private Rigidbody2D rigidbody2d;

    //Animator コンポーネント
    private Animator animator;

    //SpriteRenderer コンポーネント
    private SpriteRenderer spriteRenderer;

    //移動ベクトル
    private Vector3 velocity;

    //Raycastの判定
    private RaycastHit2D[] hit = new RaycastHit2D[3];

    private LayerMask groundLayer;

    //ジャンプできる回数
    private int jumpCount;

    //移動中か
    public bool isMoving;

    //接地しているか
    public bool isGround;

    // Use this for initialization
    void Start () {  

        //初期化
        Init();

    }
	
	// Update is called once per frame
	void Update () {
        
        //移動
        Move();

        //接地判定
        Ground();

        //ジャンプ
        Jump();

        //ショット
        Shot();

        //プレイヤーのアニメーション
        Animation();
	}

    void Init()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //地面のレイヤーを定義
        groundLayer = LayerMask.NameToLayer("Ground");

        //二段ジャンプをできるようにする
        jumpCount = 1;
    }

    void Move()
    {
        
        //キー入力によりベクトルを加算
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //右に進む
            isMoving = true;
            rigidbody2d.velocity = new Vector2(5, rigidbody2d.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //左に進む
            isMoving = true;
            rigidbody2d.velocity = new Vector2(-5, rigidbody2d.velocity.y);
        }
        else
        {
            //左右キーを押していなかったら止まる
            isMoving = false;
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }

        if(GetDirection().x > 0)
        {
            //右向き
            spriteRenderer.flipX = false;
        }
        else if(GetDirection().x < 0)
        {
            //左向き
            spriteRenderer.flipX = true;
        }

    }

    void Ground()
    {
        //プレイヤーの下側の接地判定
        hit[0] = Physics2D.Raycast(transform.position, Vector2.down, 0.5f,1 << groundLayer);
        
        //壁の側面に触れたときに接地判定を出さないようにする
        hit[1] = Physics2D.Raycast(transform.position + new Vector3(0.25f, 0),Vector2.down,0.5f, 1 << groundLayer);
        
        //壁の側面に触れたときに接地判定を出さないようにする
        hit[2] = Physics2D.Raycast(transform.position - new Vector3(0.25f, 0),Vector2.down, 0.5f, 1 << groundLayer);

        if(hit[0].collider == null && hit[1].collider == null && hit[2].collider == null)
        {
            //空中にいるときは接地判定をfalseに
            isGround = false;
            return;
        }
        else
        {
            //地上にいるときは接地判定をtrueにしてjumpCountを戻す
            isGround = true;
            jumpCount = 1;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGround)
        {
            //接地しているときに左シフトを押したら1段目のジャンプ
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 10);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && isGround == false && jumpCount >= 1)
        {
            //もういちど左シフトを押したら2段目のジャンプ
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 7);
            jumpCount--;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift) && rigidbody2d.velocity.y >= 2)
        {
            //左シフトを離したらジャンプを中断
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 2);
        }
    }

    void Shot()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //弾を生成
            GameObject bullet = Instantiate(shot, transform.position, Quaternion.identity);

            //Bulletスクリプトオブジェクトを取得
            Bullet b = bullet.GetComponent<Bullet>();

            if (spriteRenderer.flipX == false)
            {
                //右向きのとき弾を右に発射させる
                b.trg = true;
            }
            else if (spriteRenderer.flipX == true)
            {
                //左向きのとき弾を左に発射させる
                b.trg = false;
            }
        }
    }

    void Animation()
    {
        if (!isGround)
        {
            //接地判定じゃないときはジャンプのアニメに
            animator.Play("PlayerJump");
        }
        else if (isMoving)
        {
            //移動しているときは走るアニメに
            animator.Play("PlayerMove");
        }
        else
        {
            //何もしていないときは立ちアニメに
            animator.Play("PlayerIdle");
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        //死亡判定
        if (other.gameObject.tag == "PlayerKiller")
        {

            //死亡判定時に何か出す
            //Instantiate(dead, transform.position, Quaternion.identity);

            ghost.PlayerDeath();

            //スタート地点にリスポーン
            gameObject.transform.position = startPoint.transform.position;
        }
    }

    public Vector3 GetDirection()
    {
        //ベクトルの向きだけが欲しいから正規化
        return rigidbody2d.velocity.normalized;
    }

    
}
