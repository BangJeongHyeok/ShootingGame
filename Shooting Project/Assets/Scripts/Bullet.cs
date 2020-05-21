using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 11;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(0,speed * Time.deltaTime, 0);


        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.y > 1)
        {
            gameObject.SetActive(false);
        }
    }
}
