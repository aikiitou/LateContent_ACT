using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // 定数

    // 移動用
    private const float WALKSPEED = 2.0f; // 歩行時のスピード
    private const float DASHSPEED = 4.0f; // ダッシュ時のスピード

    // 攻撃用
    private const float ATTACKINTERVAL = 0.2f; // 攻撃の間隔

    // 方向変換用
    private const float NORMALANGEL = 0.0f;
    private const float REVERSALANGLE = 180.0f;

    // スタミナ用
    private const float STARTSTAMINA = 100.0f;
    private const float WALKDECSTAMINA = 2.0f;
    private const float DASHDECSTAMINA = 6.0f;
    private const float JUMPDECSTAMINA = 6.0f;
    private const float ATTACKDECSTAMINA = 8.0f;
    private const float ONESECONDS = 1.0f;

    // 変数

    // 基本
    private Rigidbody2D rb2D = null; // 物理演算用コンポーネント格納変数


    // 移動用
    private Vector2 input = Vector3.zero;        // キーパッドの左スティック入力情報取得変数
    private Vector2 clampedInput = Vector3.zero; // input変数の値に制限をかけた値を格納する変数
    private Vector2 velocity = Vector3.zero;     // 移動速度格納変数
    private bool isDash = false;

    // 方向変換用
    Quaternion rot;
    Quaternion currentrot;

    // ジャンプ用
    [SerializeField] private float jump; // ジャンプ時に加える力
    private bool isJump = false;         // ジャンプしているかのチェック用フラグ


    // 攻撃用
    [SerializeField] private GameObject attackEffect = null; // 攻撃エフェクトのゲームオブジェクト格納変数
    private float atkInterbalTimer = 0.0f; // 前回攻撃してから何秒立ったかのタイマー
    private bool isAttack = false; // アタックの入力があったかどうかのフラグ
    private bool isAttacking = false;　// 攻撃中かどうかのフラグ

    // スタミナ用
    [SerializeField]
    private GameObject slider = null;
    private StaminaSliderController sliderController = null;
    private float staminaValue = 0.0f;
    private float moveTime = 0.0f; 

    //　関数
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

    private void Move(float _walk, float _dash, float _dec_walk, float _dec_dash) // 移動
    {
        float speed;
        float decStamina;

        if(isDash)
        {
            speed = _dash;
            decStamina = _dec_dash;
        }
        else
        {
            speed = _walk;
            decStamina = _dec_walk;
        }
        input.x = Gamepad.current.leftStick.x.value; // ゲームパッドの左スティックの横軸の値の読み取り
        clampedInput = Vector2.ClampMagnitude(input, 1.0f); // 読み取った値に制限をかける

        // 引数の値を使った速度計算
        velocity = clampedInput * speed;
        velocity = velocity - rb2D.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -speed, speed), 0.0f);

        if (clampedInput != Vector2.zero)
        {
            staminaValue -= decStamina * Time.deltaTime;
            sliderController.GetStaminaValue(staminaValue);
        }
    }

    private void Jump() // ジャンプ
    {
        Debug.Log("jump"); // デバッグ用
        isJump = true;
        rb2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        staminaValue -= JUMPDECSTAMINA;
        sliderController.GetStaminaValue(staminaValue);
    }

    private void AngleChange()
    {
        if (Gamepad.current.leftStick.value.x == 0)
        {
            transform.rotation = currentrot;
        }
        else if (Gamepad.current.leftStick.value.x < 0)
        {
            rot = Quaternion.Euler(0.0f, REVERSALANGLE, 0.0f);
        }
        else if (Gamepad.current.leftStick.value.x > 0)
        {
            rot = Quaternion.Euler(0.0f, NORMALANGEL, 0.0f);
        }
        transform.rotation = rot;
        currentrot = rot;
    }

    private void Attack() // 攻撃
    {
        // 前回攻撃してからの時間がインターバルを超えていたら
        if (atkInterbalTimer > ATTACKINTERVAL)
        {
            isAttacking = true;
            attackEffect.SetActive(true); // 攻撃エフェクトの表示
            atkInterbalTimer = 0.0f;
            staminaValue -= ATTACKDECSTAMINA;
            sliderController.GetStaminaValue(staminaValue);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grand") // 着地の判定
        {
            isJump = false;
            Debug.Log("isJump=false"); // デバッグ用
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderController = slider.GetComponent<StaminaSliderController>();
        rb2D = GetComponent<Rigidbody2D>();
        staminaValue = STARTSTAMINA;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null) return;

        // 移動関連

        RunSwiching(); // 移動モードの切り替え

        Move(WALKSPEED, DASHSPEED, WALKDECSTAMINA, DASHDECSTAMINA); // 移動

        AngleChange();　// 方向変換

        // ゲームパッドの左ボタンが押されそれがジャンプ中でない場合
        if (Gamepad.current.buttonWest.isPressed && isJump == false)
        {
            Jump(); // ジャンプ
        }


        // 攻撃関連

        atkInterbalTimer += Time.deltaTime; // 攻撃間隔の計算
        if (Gamepad.current.buttonEast.isPressed && isAttacking == false) // 右ボタが押されたらで攻撃
        {
            isAttack = true;
            Attack();
        }
        if(Gamepad.current.buttonEast.isPressed == false)
        {
            isAttack = false;
            isAttacking = false;
        }

        if(Gamepad.current.leftStick.value == Vector2.zero
            && isJump == false && isAttack == false)
        {

        }
    }

    private void FixedUpdate()
    {
        // 自機に力を加えて動かす
        rb2D.AddForce(rb2D.mass * velocity / Time.fixedDeltaTime, ForceMode2D.Force);
    }

}
