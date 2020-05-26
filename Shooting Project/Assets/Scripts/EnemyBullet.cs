using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 11;
    Vector2 Dir;
    Vector2 NorDir;

    void Start()
    {

    }


    void Update()
    {
        gameObject.transform.Translate(-NorDir * speed * Time.deltaTime);
    }

    public void BulletMove(Vector3 PlayerPosition)
    {
        NorDir = Vector3.Normalize(gameObject.transform.position - PlayerPosition);


        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.y < -0.2f || pos.y > 1.2f || pos.x < -0.2f || pos.x > 1.2f)
        {
            gameObject.SetActive(false);
        }
    }
}
