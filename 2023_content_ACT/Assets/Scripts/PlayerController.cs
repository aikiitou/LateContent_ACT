using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // �萔

    // �ړ��p
    private const float WALKSPEED = 2.0f; // ���s���̃X�s�[�h
    private const float DASHSPEED = 4.0f; // �_�b�V�����̃X�s�[�h

    // �U���p
    private const float ATTACKINTERVAL = 0.2f; // �U���̊Ԋu

    // �����ϊ��p
    private const float NORMALANGEL = 0.0f;
    private const float REVERSALANGLE = 180.0f;

    // �X�^�~�i�p
    private const float STARTSTAMINA = 100.0f;
    private const float WALKDECSTAMINA = 2.0f;
    private const float DASHDECSTAMINA = 6.0f;
    private const float JUMPDECSTAMINA = 6.0f;
    private const float ATTACKDECSTAMINA = 8.0f;
    private const float ONESECONDS = 1.0f;

    // �ϐ�

    // ��{
    private Rigidbody2D rb2D = null; // �������Z�p�R���|�[�l���g�i�[�ϐ�


    // �ړ��p
    private Vector2 input = Vector3.zero;        // �L�[�p�b�h�̍��X�e�B�b�N���͏��擾�ϐ�
    private Vector2 clampedInput = Vector3.zero; // input�ϐ��̒l�ɐ������������l���i�[����ϐ�
    private Vector2 velocity = Vector3.zero;     // �ړ����x�i�[�ϐ�
    private bool isDash = false;

    // �����ϊ��p
    Quaternion rot;
    Quaternion currentrot;

    // �W�����v�p
    [SerializeField] private float jump; // �W�����v���ɉ������
    private bool isJump = false;         // �W�����v���Ă��邩�̃`�F�b�N�p�t���O


    // �U���p
    [SerializeField] private GameObject attackEffect = null; // �U���G�t�F�N�g�̃Q�[���I�u�W�F�N�g�i�[�ϐ�
    private float atkInterbalTimer = 0.0f; // �O��U�����Ă��牽�b���������̃^�C�}�[
    private bool isAttack = false; // �A�^�b�N�̓��͂����������ǂ����̃t���O
    private bool isAttacking = false;�@// �U�������ǂ����̃t���O

    // �X�^�~�i�p
    [SerializeField]
    private GameObject slider = null;
    private StaminaSliderController sliderController = null;
    private float staminaValue = 0.0f;
    private float moveTime = 0.0f; 

    //�@�֐�
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

    private void Move(float _walk, float _dash, float _dec_walk, float _dec_dash) // �ړ�
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
        input.x = Gamepad.current.leftStick.x.value; // �Q�[���p�b�h�̍��X�e�B�b�N�̉����̒l�̓ǂݎ��
        clampedInput = Vector2.ClampMagnitude(input, 1.0f); // �ǂݎ�����l�ɐ�����������

        // �����̒l���g�������x�v�Z
        velocity = clampedInput * speed;
        velocity = velocity - rb2D.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -speed, speed), 0.0f);

        if (clampedInput != Vector2.zero)
        {
            staminaValue -= decStamina * Time.deltaTime;
            sliderController.GetStaminaValue(staminaValue);
        }
    }

    private void Jump() // �W�����v
    {
        Debug.Log("jump"); // �f�o�b�O�p
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

    private void Attack() // �U��
    {
        // �O��U�����Ă���̎��Ԃ��C���^�[�o���𒴂��Ă�����
        if (atkInterbalTimer > ATTACKINTERVAL)
        {
            isAttacking = true;
            attackEffect.SetActive(true); // �U���G�t�F�N�g�̕\��
            atkInterbalTimer = 0.0f;
            staminaValue -= ATTACKDECSTAMINA;
            sliderController.GetStaminaValue(staminaValue);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grand") // ���n�̔���
        {
            isJump = false;
            Debug.Log("isJump=false"); // �f�o�b�O�p
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

        // �ړ��֘A

        RunSwiching(); // �ړ����[�h�̐؂�ւ�

        Move(WALKSPEED, DASHSPEED, WALKDECSTAMINA, DASHDECSTAMINA); // �ړ�

        AngleChange();�@// �����ϊ�

        // �Q�[���p�b�h�̍��{�^���������ꂻ�ꂪ�W�����v���łȂ��ꍇ
        if (Gamepad.current.buttonWest.isPressed && isJump == false)
        {
            Jump(); // �W�����v
        }


        // �U���֘A

        atkInterbalTimer += Time.deltaTime; // �U���Ԋu�̌v�Z
        if (Gamepad.current.buttonEast.isPressed && isAttacking == false) // �E�{�^�������ꂽ��ōU��
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
        // ���@�ɗ͂������ē�����
        rb2D.AddForce(rb2D.mass * velocity / Time.fixedDeltaTime, ForceMode2D.Force);
    }

}
