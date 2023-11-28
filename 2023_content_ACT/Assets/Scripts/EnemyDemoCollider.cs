using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyDemoCollider : MonoBehaviour
{
    // 
    private Enemy01Controller enemy01ControllerScript;


    void Start()
    {
        // 
        enemy01ControllerScript = GetComponent<Enemy01Controller>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy01ControllerScript.SetEnemy01Hp(enemy01ControllerScript.GetEnemy01Hp() - 1);

            // 
            if (enemy01ControllerScript.GetEnemy01Hp() <= 0)
            {
                death();
            }

        }
    }

    private void death()
    {
        gameObject.SetActive(false);
    }
}
