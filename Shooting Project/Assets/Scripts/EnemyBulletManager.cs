using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    List<GameObject> EnemyBullets;
    [SerializeField] GameObject EnemyBulletObj;
    [SerializeField] GameObject Player;
    bool CreateOn = false;
    Vector3 EnemyPosition;
    void Start()
    {
        EnemyBullets = new List<GameObject>();
        for (int i = 0; i < 48; i++)
        {
            EnemyBullets.Add(null);
        }

    }


    public void Create(Vector3 pos)
    {
        EnemyPosition = pos;
        CreateOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CreateOn)
        {
            for (int i = 0; i < EnemyBullets.Count; i++)
            {
                if (EnemyBullets[i] != null && !EnemyBullets[i].activeInHierarchy)//지금 찾는 적이 켜져있지 않은게 맞을때
                {
                    EnemyBullets[i].SetActive(true);
                    EnemyBullets[i].GetComponent<EnemyBullet>().BulletMove(Player.transform.position);
                    break;
                }
                else if (EnemyBullets[i] == null)
                {
                    GameObject tempBullet = Instantiate(EnemyBulletObj, EnemyPosition, Quaternion.identity, gameObject.transform);//생성
                    EnemyBullets[i] = tempBullet;//생성한걸 지금 이 배열에 넣음
                    EnemyBullets[i].GetComponent<EnemyBullet>().BulletMove(Player.transform.position);
                    break;
                }
                else continue;
            }
            CreateOn = false;
        }
    }
}
