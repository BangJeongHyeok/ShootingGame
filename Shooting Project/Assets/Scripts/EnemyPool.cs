using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> Enemys1;
    public List<GameObject> Enemys2;
    [SerializeField] float Delay;
    [SerializeField] GameObject []EnemyObj;
    int SpawnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        Enemys1 = new List<GameObject>();
        Enemys2 = new List<GameObject>();
        for (int i = 0; i < 48; i++)
        {
            Enemys1.Add(null);
            Enemys2.Add(null);
        }

        StartCoroutine(SpawnDelay(Delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemySpawn(Vector2 StartPosition, float m_speed, Vector2 m_Dir, EnemyMovement.MOVESTATE m_ThisState, Vector3 m_PointVec, Vector3 m_EndVec)
    {
        if ((int)m_ThisState == 0)
        {
            for (int i = 0; i < Enemys1.Count; i++)
            {
                if (Enemys1[i] != null && !Enemys1[i].activeInHierarchy)//지금 찾는 적이 켜져있지 않은게 맞을때
                {
                    Enemys1[i].SetActive(true);
                    Enemys1[i].GetComponent<EnemyMovement>().SettingValue(StartPosition, m_speed, m_Dir, m_ThisState, m_PointVec, m_EndVec);
                    break;
                }
                else if (Enemys1[i] == null)
                {
                    GameObject tempBullet = Instantiate(EnemyObj[(int)m_ThisState], transform.position, Quaternion.identity, gameObject.transform);//생성
                    Enemys1[i] = tempBullet;//생성한걸 지금 이 배열에 넣음
                    Enemys1[i].GetComponent<EnemyMovement>().SettingValue(StartPosition, m_speed, m_Dir, m_ThisState, m_PointVec, m_EndVec);
                    break;
                }
                else continue;
            }
        }
        else if ((int)m_ThisState == 1)
        {
            for (int i = 0; i < Enemys2.Count; i++)
            {
                if (Enemys2[i] != null && !Enemys2[i].activeInHierarchy)//지금 찾는 적이 켜져있지 않은게 맞을때
                {
                    Enemys2[i].SetActive(true);
                    Enemys2[i].GetComponent<EnemyMovement>().SettingValue(StartPosition, m_speed, m_Dir, m_ThisState, m_PointVec, m_EndVec);
                    break;
                }
                else if (Enemys2[i] == null)
                {
                    GameObject tempBullet = Instantiate(EnemyObj[(int)m_ThisState], transform.position, Quaternion.identity, gameObject.transform);//생성
                    Enemys2[i] = tempBullet;//생성한걸 지금 이 배열에 넣음
                    Enemys2[i].GetComponent<EnemyMovement>().SettingValue(StartPosition, m_speed, m_Dir, m_ThisState, m_PointVec, m_EndVec);
                    break;
                }
                else continue;
            }
        }
        
    }

    IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if( SpawnCount > 30)
            EnemySpawn(new Vector2(Random.Range(0, 8) - 4, 6), 7, new Vector2(Random.Range(0,0.4f)-0.2f, -1), EnemyMovement.MOVESTATE.Straight,
                new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            else
            EnemySpawn(new Vector2(Random.Range(0,8)-4, 6), 11, new Vector2(0, 0), EnemyMovement.MOVESTATE.Curve,
                new Vector3(Random.Range(0, 4)-2, 0, 0), new Vector3(Random.Range(0, 8) -4, -8, 0));
        SpawnCount++;
        StartCoroutine(SpawnDelay(Delay));
    }

}
