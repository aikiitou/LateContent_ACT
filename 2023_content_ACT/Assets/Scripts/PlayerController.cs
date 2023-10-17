using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // �萔

    // �ړ��p
    private const float WALKSPEED = 2.0f; // ���s���̃X�s�[�h
    private const float DASHSPEED = 4.0f; // �_�b�V�����̃X�s�[�h

    // �U���p
    private const float ATTACKINTERVAL = 1.0f; // �U���̊Ԋu

    // �ϐ�

    // ��{
    private Rigidbody2D rb2D = null; // �������Z�p�R���|�[�l���g�i�[�ϐ�


    // �ړ��p
    private Vector2 input = Vector3.zero;        // �L�[�p�b�h�̍��X�e�B�b�N���͏��擾�ϐ�
    private Vector2 clampedInput = Vector3.zero; // input�ϐ��̒l�ɐ������������l���i�[����ϐ�
    private Vector2 velocity = Vector3.zero;     // �ړ����x�i�[�ϐ�
    private bool isDash = false;

    // �W�����v�p
    [SerializeField] private float jump; // �W�����v���ɉ������
    private bool isJump = false;         // �W�����v���Ă��邩�̃`�F�b�N�p�t���O


    // �U���p
    [SerializeField] private GameObject attackEffect = null; // �U���G�t�F�N�g�̃Q�[���I�u�W�F�N�g�i�[�ϐ�
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

        // �ړ�

        RunSwiching(); // �ړ����[�h�̐؂�ւ�

        if (isDash == true)
        {
            Move(DASHSPEED); // �����Ƀ_�b�V�����̂�����
        }
        else
        {
            Move(WALKSPEED); // �����ɒʏ펞�̂�����
        }

        // �W�����v

        // �Q�[���p�b�h�̍��{�^���������ꂻ�ꂪ�W�����v���łȂ��ꍇ
        if (Gamepad.current.buttonWest.isPressed && isJump == false)
        {
            Jump();
        }

        // �U��
        atkTimer += Time.deltaTime;
        if (Gamepad.current.buttonEast.isPressed)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        rb2D.AddForce(rb2D.mass * velocity / Time.fixedDeltaTime, ForceMode2D.Force); // ���@�ɗ͂������ē�����
    }

    private void RunSwiching() // �ړ����[�h�̐؂�ւ�
    {
        // �W�����v���łȂ��ꍇ�ƍU�����łȂ��ꍇ�̂݃_�b�V���ƕ����̐؂�ւ����ł���
        if (isJump == false && isAttack == false)
        {
            if (Gamepad.current.buttonSouth.isPressed) // �Q�[���p�b�h�̉��{�^����������Ă����ꍇ
            {
                isDash = true; // �_�b�V��
            }
            else // �Q�[���p�b�h�̉��{�^����������Ă��Ȃ��ꍇ
            {
                isDash = false; // ����
            }
        }
    }
    private void Move(float _speed) // �ړ�
    {
        input.x = Gamepad.current.leftStick.x.value; // �Q�[���p�b�h�̍��X�e�B�b�N�̉����̒l�̓ǂݎ��
        clampedInput = Vector2.ClampMagnitude(input, 1.0f); // �ǂݎ�����l�ɐ�����������

        // �����̒l���g�������x�v�Z
        velocity = clampedInput * _speed;
        velocity = velocity - rb2D.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -_speed, _speed), 0.0f);

    }

    private void Jump() // �W�����v
    {
        Debug.Log("jump"); // �f�o�b�O�p
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
        // if (collision.gameObject.tag == "Grand") // ���n�̔���
        {
            isJump = false;
            Debug.Log("isJump=false"); // �f�o�b�O�p
        }
    }
}
