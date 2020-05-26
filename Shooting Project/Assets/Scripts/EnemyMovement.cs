using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MOVESTATE
    {
        Straight,
        Curve,
    }

    public PoolManager Pool;
    [SerializeField] GameObject ExplosionObj;

    SpriteRenderer EnemySprite;
    private int MaxHP;
    private float CurrentHP;
    private float speed = 11;
    private Vector2 Dir;//Default값
    public MOVESTATE ThisState;
    private Vector3 PointVec;
    private Vector3 EndVec;
    private bool isGoal = false;
    bool Once = true;

    public void SettingValue(Vector3 m_position,float m_speed, Vector2 m_Dir, MOVESTATE m_ThisState, Vector3 m_PointVec, Vector3 m_EndVec)
    {
        transform.position = m_position;
        speed = m_speed;
        Dir = m_Dir;
        ThisState = m_ThisState;
        PointVec = m_PointVec;
        EndVec = m_EndVec;

        switch ((int)m_ThisState)
        {
            case 0:
                MaxHP = 8;
                break;
            case 1:
                MaxHP = 15;
                break;
        }
                CurrentHP = MaxHP;
    }

    void Awake()
    {
        EnemySprite = gameObject.GetComponent<SpriteRenderer>();
        Pool = GameObject.Find("BulletPool").GetComponent<PoolManager>();
    }

    private void OnEnable()
    {
        isGoal = false;
        EnemySprite.color = new Color(255, 255, 255, 255);
        CurrentHP = MaxHP;
        Once = true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < Pool.Explosions2.Count; i++)
        {

            if (Pool.Explosions2[i] != null && !Pool.Explosions2[i].activeInHierarchy)//지금 찾는 총알이 켜져있지 않은게 맞을때
            {
                Pool.Explosions2[i].SetActive(true);
                Pool.Explosions2[i].transform.position = gameObject.transform.position;
                Pool.Explosions2[i].GetComponent<Animator>().Play(0);
                break;
            }
            else if (Pool.Explosions2[i] == null)
            {
                GameObject tempExplosion = Instantiate(ExplosionObj, transform.position, Quaternion.identity, Pool.transform);//생성
                Pool.Explosions2[i] = tempExplosion;//생성한걸 지금 이 배열에 넣음
                Pool.Explosions2[i].transform.position = gameObject.transform.position;
                Pool.Explosions2[i].GetComponent<Animator>().Play(0);
                break;
            }
            else continue;
        }
    }

    private void Move()
    {
        switch (ThisState)
        {
            case MOVESTATE.Straight:
                transform.Translate(Vector3.Normalize(Dir) * speed * Time.deltaTime);
                break;
            case MOVESTATE.Curve:
                if (!isGoal)
                    transform.position = Vector3.LerpUnclamped(transform.position, PointVec, 1.4f * Time.deltaTime);
                else
                    transform.position = Vector3.LerpUnclamped(transform.position, EndVec, 1.4f * Time.deltaTime);

                if (Vector3.Distance(transform.position, PointVec) < 1)
                {
                    if (Once)
                    {
                        isGoal = true;
                        GameObject.Find("EnemyBulletPool").GetComponent<EnemyBulletManager>().Create(gameObject.transform.position);
                        Once = false;
                    }
                }
                break;
        }

    }

    void Update()
    {
        Move();
        isOutMap();
    }

    void isOutMap()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.y > 1.2f || pos.x > 1.2f || pos.x < -0.2f || pos.y < -0.2f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            CurrentHP -= PlaneMovement.Damage;
            StartCoroutine(Hitted());
            collision.gameObject.SetActive(false);

            if (CurrentHP <= 0)//죽음
            {
                PlaneMovement.Exp += 1;
                gameObject.SetActive(false);

            }
            
        }
    }

    IEnumerator Hitted()
    {
        EnemySprite.color = new Color(255, 0, 0,255);
        yield return new WaitForSeconds(0.08f);
        EnemySprite.color = new Color(255, 255, 255, 255);
    }
}
