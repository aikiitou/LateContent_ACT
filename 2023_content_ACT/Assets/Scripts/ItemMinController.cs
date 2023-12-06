using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMinController : MonoBehaviour
{
    //　定数
    [SerializeField]
    private int CHANGEANGLE = 180;

    // 変数

    // 関数

    private void Rotation()
    {
        transform.Rotate(Vector3.up, CHANGEANGLE * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().AddStamina(gameObject);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Rotation();
    }
}
