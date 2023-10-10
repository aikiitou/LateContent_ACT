using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Controller : MonoBehaviour
{
    //SpriteRenderer�擾
    [SerializeField] private Renderer renderer;
    //speed���擾
    [SerializeField] private float speed;
    //�t���O�錾
    private bool isMove;//
    private bool isFind;//���F
    private bool isDeath;//���S

    //�񋓌^Phase�̐錾
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

    //phase�؂�ւ�����
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

        //�����Ȃ��Ȃ�����Idle��
        if (!isFind)
        {
            phase = Phase.Idle;
        }
    }

    //���E�ɓ����Ă��邩�𔻒f���ăt���O��������郁�\�b�h
    private void isFindHundle()
    {
        if (renderer.isVisible) { isFind = true; }
        else { isFind = false; }
    }
}
