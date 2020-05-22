﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour
{
    [SerializeField] GameObject BulletPool;
    [SerializeField] GameObject LevelUpEffect;
    [SerializeField] GameObject Skill1;
    [SerializeField] GameObject Skill2;
    [SerializeField] Text LevelText;
    [SerializeField] PoolManager Pool;
    [SerializeField] Slider Level;
    [SerializeField] float MoveSpeed;
    [SerializeField] float Damage = 1;
    [SerializeField] float FireDelay = 0.17f;
    [SerializeField] int PlayerLevel = 1;
    float Exp = 0;
    bool isDelayEnd = true;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        LevelManager();


        LevelText.text = "레벨 " + PlayerLevel.ToString() + " [ " + ((Level.value / Level.maxValue) * 100).ToString("N1") + "% ] ";
    }

    void KeyInput()
    {
        ScreenRect();

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, MoveSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, -MoveSpeed * Time.deltaTime, 0);
        }


        if (Input.GetKey(KeyCode.X) && isDelayEnd)
        {
            Exp += 3;
            Fire();
            isDelayEnd = false;
            StartCoroutine(Delay(FireDelay));
        }
    }

    void Fire()
    {
        for(int i = 0; i< Pool.Bullets.Count; i++)
        {
            if (Pool.Bullets[i] != null && !Pool.Bullets[i].activeInHierarchy)//지금 찾는 총알이 켜져있지 않은게 맞을때
            {
                Pool.Bullets[i].SetActive(true);
                Pool.Bullets[i].transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                break;
            }
            else if (Pool.Bullets[i] == null)
            {
                GameObject tempBullet = Instantiate(BulletPool, transform.position, Quaternion.identity, Pool.transform);//생성
                Pool.Bullets[i] = tempBullet;//생성한걸 지금 이 배열에 넣음
                break;
            }
            else continue;
        }

        //if (!Pool.Bullets[i].activeInHierarchy)//지금 찾는 총알이 켜져있지 않은게 맞을때
        //{
        //    Pool.Bullets[i].SetActive(true);
        //    Pool.Bullets[i].transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //    break;
        //}
        //else if (Pool.Bullets[i] == null)//값이 존재하지 않는 구역까지 도달하였을때(총알이 없는 곳)
        //{
        //    GameObject tempBullet = Instantiate(BulletPool, transform.position, Quaternion.identity, Pool.transform);//생성
        //    tempBullet = Pool.Bullets[i];//생성한걸 지금 이 배열에 넣음
        //    break;
        //}
        //else
        //{
        //    i++;//다음 총알 찾으러 ㄱ
        //}
    }

    private void ScreenRect()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0)
            pos.x = 0;
        else if (pos.x > 1)
            pos.x = 1;

        if (pos.y < 0)
            pos.y = 0;
        else if (pos.y > 1)
            pos.y = 1;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void LevelManager()
    {
        Level.value = Mathf.Lerp(Level.value, Exp, 0.1f);

        if (Level.value > Level.maxValue - 0.1f)//MaxValue의 최대값까지 도달하면
        {
            Level.maxValue = Level.maxValue += 5;
            Exp = 0;
            PlayerLevel++;


            LevelUpEffect.SetActive(true);
            LevelUpEffect.GetComponent<Animator>().Play(0);
            LevelUpEffect.transform.position = gameObject.transform.position;


            if(PlayerLevel == 3)
            {
                Skill1.SetActive(true);
            }
            else if (PlayerLevel == 5)
            {
                Skill2.SetActive(true);
            }

            if (PlayerLevel > 5)
            {
                Damage = Mathf.Lerp(Damage, 4f, 0.1f);
                FireDelay = Mathf.Lerp(FireDelay, 0.03f, 0.1f);
                MoveSpeed = Mathf.Lerp(MoveSpeed, 8f, 0.1f);
            }
            else
            {
                Damage *= 1.2f;
                FireDelay *= 0.8f;
                MoveSpeed *= 1.2f;
            }


        }
    }

    IEnumerator Delay(float firedelay)
    {
        yield return new WaitForSeconds(firedelay);
        isDelayEnd = true;
    }
}
