using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // 定数

    // 移動用
    private const float WALKSPEED = 2.0f; // 歩行時のスピード
    private const float DASHSPEED = 4.0f; // ダッシュ時のスピード

    // 攻撃用
    private const float ATTACKINTERVAL = 1.0f; // 攻撃の間隔

    // 変数

    // 基本
    private Rigidbody2D rb2D = null; // 物理演算用コンポーネント格納変数


    // 移動用
    private Vector2 input = Vector3.zero;        // キーパッドの左スティック入力情報取得変数
    private Vector2 clampedInput = Vector3.zero; // input変数の値に制限をかけた値を格納する変数
    private Vector2 velocity = Vector3.zero;     // 移動速度格納変数
    private bool isDash = false;

    // ジャンプ用
    [SerializeField] private float jump; // ジャンプ時に加える力
    private bool isJump = false;         // ジャンプしているかのチェック用フラグ


    // 攻撃用
    [SerializeField] private GameObject attackEffect = null; // 攻撃エフェクトのゲームオブジェクト格納変数
    private float atkTimer = 0.0f;
    private bool isAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null) return;

        // 移動

        RunSwiching(); // 移動モードの切り替え

        if (isDash == true)
        {
            Move(DASHSPEED); // 引数にダッシュ時のを入れる
        }
        else
        {
            Move(WALKSPEED); // 引数に通常時のを入れる
        }

        // ジャンプ

        // ゲームパッドの左ボタンが押されそれがジャンプ中でない場合
        if (Gamepad.current.buttonWest.isPressed && isJump == false)
        {
            Jump();
        }

        // 攻撃
        atkTimer += Time.deltaTime;
        if (Gamepad.current.buttonEast.isPressed)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        rb2D.AddForce(rb2D.mass * velocity / Time.fixedDeltaTime, ForceMode2D.Force); // 自機に力を加えて動かす
    }

    private void RunSwiching() // 移動モードの切り替え
    {
        // ジャンプ中でない場合と攻撃中でない場合のみダッシュと歩きの切り替えができる
        if (isJump == false && isAttack == false)
        {
            if (Gamepad.current.buttonSouth.isPressed) // ゲームパッドの下ボタンが押されていた場合
            {
                isDash = true; // ダッシュ
            }
            else // ゲームパッドの下ボタンが押されていない場合
            {
                isDash = false; // 歩く
            }
        }
    }
    private void Move(float _speed) // 移動
    {
        input.x = Gamepad.current.leftStick.x.value; // ゲームパッドの左スティックの横軸の値の読み取り
        clampedInput = Vector2.ClampMagnitude(input, 1.0f); // 読み取った値に制限をかける

        // 引数の値を使った速度計算
        velocity = clampedInput * _speed;
        velocity = velocity - rb2D.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -_speed, _speed), 0.0f);

    }

    private void Jump() // ジャンプ
    {
        Debug.Log("jump"); // デバッグ用
        isJump = true;
        rb2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if (atkTimer > ATTACKINTERVAL)
        {
            attackEffect.SetActive(true);
            atkTimer = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.tag == "Grand") // 着地の判定
        {
            isJump = false;
            Debug.Log("isJump=false"); // デバッグ用
        }
    }
}
