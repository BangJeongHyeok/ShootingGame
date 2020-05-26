using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPool : MonoBehaviour
{
    [SerializeField] GameObject[] BGPool;
    [SerializeField] SpriteRenderer[] SRPool;
    [SerializeField] float Speed = 8;

    Vector2 ImageSize;

    float CurrentPosY = 0;
    // Start is called before the first frame update
    void Start()
    {

        ImageSize = SRPool[0].sprite.rect.size;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPosY -= Speed * Time.deltaTime;
        //for (int i = 0; i < BGPool.Length; i++)
        //{
        //BGPool[i].transform.Translate(0, -Speed * Time.deltaTime, 0);

        //if (BGPool[i].transform.position.y <= -9.6f)
        //{
        //    BGPool[i].transform.position = new Vector3(0, 9.5f * 2, 0);
        //}

        float renderPos = CurrentPosY % 9.6f + 9.6f / 2;
            //float renderPos = Speed % 9.6f + 9.6f / 2;
            BGPool[2].transform.position = new Vector2(0, renderPos + 9.6f);
            BGPool[1].transform.position = new Vector2(0, renderPos - 9.6f);
            BGPool[0].transform.position = new Vector2(0, renderPos);
        //}
    }
}
