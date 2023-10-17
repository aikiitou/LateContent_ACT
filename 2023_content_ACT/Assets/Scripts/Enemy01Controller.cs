using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Controller : MonoBehaviour
{
    //SpriteRenderer取得
    [SerializeField] private Renderer renderer;
    //speedを取得
    [SerializeField] private float speed;
    //
    [SerializeField] private int hitPoint;
    //
    [SerializeField] private GameObject player;
    //
    private float distance;

    //フラグ宣言
    private bool isMove;    //
    private bool isFind;    //視認
    private bool isDeath;   //死亡

    //列挙型Phaseの宣言
    private enum Phase 
    {
        Default = -1,
        Idle,
        Move,
        Attack,
        Death,

    };
    Phase phase = Phase.Idle;

    //カプセル化処理
    public int HitPoint { get => hitPoint; /*set => hitPoint = value;*/ }

    void Update()
    {
        //
        hundlePhase();

    }

    //phase切り替え処理
    private void hundlePhase()
    {
        switch (phase)
        {
            case Phase.Idle:
                idle();
                break;
            case Phase.Move:
                move();
                break;
            case Phase.Attack:
                break; 
            case Phase.Death:
                break;
            default: 
                break;
        
        }
    }

    //
    private void idle() 
    {
        isFindHundle();

        if (isFind) { phase = Phase.Move; }
    }

    //--------------------　ここから動作処理　--------------------//
    private void move()
    {
        // ガゼル
        //transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);

        isFindHundle();

        isPress();

        //見えなくなったらIdleに
        if (!isFind)
        {
            phase = Phase.Idle;
        }
    }

    //--------------------　ここまで動作処理　--------------------//

    private void death()
    {

    }

    //視界に入っているかを判断してフラグ操作をするメソッド
    private void isFindHundle()
    {
        if (renderer.isVisible) { isFind = true; }
        else { isFind = false; }
    }

    private void isPress()
    {
        Debug.Log(distance);

        if (distance < 5) 
        {



        }

    }
}
