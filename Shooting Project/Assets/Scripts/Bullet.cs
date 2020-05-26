using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PoolManager Pool;
    [SerializeField] GameObject ExplosionObj;

    float speed = 11;
    public Vector2 Dir;//Default값
    
    // Start is called before the first frame update
    void Start()
    {
        Pool = GameObject.Find("BulletPool").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove(Dir);
    }

    public void OnEnable()
    {
        //for (int i = 0; i < Pool.Explosions.Count; i++)
        //{
        //    if (!Pool.Explosions[i].GetComponent<Animation>().isPlaying)
        //    {
        //        Pool.Explosions[i].SetActive(false);
        //    }
        //}
    }

    public void OnDisable()
    {
        for (int i = 0; i < Pool.Explosions.Count; i++)
        {

            if (Pool.Explosions[i] != null && !Pool.Explosions[i].activeInHierarchy)//지금 찾는 총알이 켜져있지 않은게 맞을때
            {
                Pool.Explosions[i].SetActive(true);
                Pool.Explosions[i].transform.position = gameObject.transform.position;
                Pool.Explosions[i].GetComponent<Animator>().Play(0);
                break;
            }
            else if (Pool.Explosions[i] == null)
            {
                GameObject tempExplosion = Instantiate(ExplosionObj, transform.position, Quaternion.identity, Pool.transform);//생성
                Pool.Explosions[i] = tempExplosion;//생성한걸 지금 이 배열에 넣음
                Pool.Explosions[i].transform.position = gameObject.transform.position;
                Pool.Explosions[i].GetComponent<Animator>().Play(0);
                break;
            }
            else continue;
        }
    }

    public void BulletMove(Vector2 DirVec2)
    {
        gameObject.transform.Translate(Vector3.Normalize(DirVec2) * speed * Time.deltaTime);


        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.y > 1.2f)
        {
            gameObject.SetActive(false);
        }
    }
}
