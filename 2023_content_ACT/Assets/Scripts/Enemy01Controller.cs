using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Controller : MonoBehaviour
{
    //SpriteRenderer取得
    [SerializeField] private Renderer renderer;
    //speedを取得
    [SerializeField] private float speed;
    //フラグ宣言
    private bool isMove;//
    private bool isFind;//視認
    private bool isDeath;//死亡

    //列挙型Phaseの宣言
    private enum Phase 
    {
        Default = -1,
        Idle,
        Move,
        Attack,
        Deth,

    };
    Phase phase = Phase.Idle;

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
            case Phase.Deth:
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
    //
    private void move()
    {

        transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);

        isFindHundle();

        //見えなくなったらIdleに
        if (!isFind)
        {
            phase = Phase.Idle;
        }
    }

    //視界に入っているかを判断してフラグ操作をするメソッド
    private void isFindHundle()
    {
        if (renderer.isVisible) { isFind = true; }
        else { isFind = false; }
    }
}
