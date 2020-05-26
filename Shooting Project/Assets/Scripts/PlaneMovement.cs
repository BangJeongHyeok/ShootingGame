using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour
{
    float timer = 0;
    [SerializeField] GameObject BulletPool;
    [SerializeField] GameObject ShieldObject;
    SpriteRenderer ShieldColor;
    [SerializeField] GameObject LevelUpEffect;
    [SerializeField] GameObject Skill1;
    [SerializeField] GameObject Skill2;
    [SerializeField] Text LevelText;
    [SerializeField] PoolManager Pool;
    [SerializeField] Slider Level;
    [SerializeField] float MoveSpeed;
    public static float Damage = 5;
    SpriteRenderer PlayerSprite;
    [SerializeField] float SingleFireDelay = 0.17f;
    [SerializeField] float MultipleFireDelay = 0.4f;
    [SerializeField] int PlayerLevel = 1;

    float shieldalpha = 1f;
    public static float Exp = 0;
    bool isDelayEnd = true;
    bool Shield = false;

    bool SingleShot = true;


    void Start()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
        ShieldColor = ShieldObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        LevelManager();
        ShieldAnimation();

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
            if (SingleShot && isDelayEnd)
            {
                Fire(new Vector2(0,1));
                isDelayEnd = false;
                StartCoroutine(Delay(SingleFireDelay));
                Exp += 0.2f;
            }
            else if(!SingleShot && isDelayEnd)
            {
                for (int i = -3; i < 3; i++)
                {
                    Fire(new Vector2(i * 0.1f, 1));
                }
                isDelayEnd = false;
                StartCoroutine(Delay(MultipleFireDelay));
                Exp += 0.8f;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SingleShot = !SingleShot;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shield = true;
            shieldalpha = 1f;
            ShieldObject.SetActive(true);
            
        }
    }

    void Fire(Vector2 DirVec)
    {
        for(int i = 0; i< Pool.Bullets.Count; i++)
        {
            if (Pool.Bullets[i] != null && !Pool.Bullets[i].activeInHierarchy)//지금 찾는 총알이 켜져있지 않은게 맞을때
            {
                Pool.Bullets[i].SetActive(true);
                Pool.Bullets[i].GetComponent<Bullet>().Dir = DirVec;
                Pool.Bullets[i].transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                break;
            }
            else if (Pool.Bullets[i] == null)
            {
                GameObject tempBullet = Instantiate(BulletPool, transform.position, Quaternion.identity, Pool.transform);//생성
                Pool.Bullets[i] = tempBullet;//생성한걸 지금 이 배열에 넣음
                Pool.Bullets[i].GetComponent<Bullet>().Dir = DirVec;
                break;
            }
            else continue;
        }
    }

    void ShieldAnimation()
    {
        ShieldObject.transform.Rotate(0, 0, 10 * Time.deltaTime);
        ShieldColor.color = new Color(1f, 1f, 1f, shieldalpha);
        if (shieldalpha < 0.01f)
        {
            shieldalpha = 0;
            Shield = false;
            ShieldObject.SetActive(false);
        }
        else
            shieldalpha -= 0.33f * Time.deltaTime;
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
            Level.maxValue = Level.maxValue += 25;
            Exp = 0;
            PlayerLevel++;


            LevelUpEffect.SetActive(true);
            LevelUpEffect.transform.position = gameObject.transform.position;
            LevelUpEffect.GetComponent<Animator>().Play(0);


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
                SingleFireDelay = Mathf.Lerp(SingleFireDelay, 0.03f, 0.1f);
                MultipleFireDelay = Mathf.Lerp(MultipleFireDelay, 0.1f, 0.1f);
                MoveSpeed = Mathf.Lerp(MoveSpeed, 8f, 0.1f);
            }
            else
            {
                Damage *= 1.2f;
                SingleFireDelay *= 0.8f;
                MultipleFireDelay *= 0.8f;
                MoveSpeed *= 1.2f;
            }


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Shield)
        {
            if (collision.tag == "Enemy" || collision.tag == "EnemyBullet")
            {
                StartCoroutine(Hitted());
                StartCoroutine(CameraShake(0.1f));
            }
        }
        
    }

    IEnumerator Delay(float firedelay)
    {
        yield return new WaitForSeconds(firedelay);
        isDelayEnd = true;
    }


    IEnumerator CameraShake(float time)
    {
        timer = 0;
        while (time > timer)
        {
            timer += Time.deltaTime;
            Vector2 ShakeAmount;
            ShakeAmount = Random.insideUnitCircle * 0.07f;

                Camera.main.transform.position += new Vector3(ShakeAmount.x, ShakeAmount.y, 0);
                yield return new WaitForSeconds(0.03f);
                Camera.main.transform.position -= new Vector3(ShakeAmount.x, ShakeAmount.y, 0);
        }
    }

    IEnumerator Hitted()
    {
        PlayerSprite.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.08f);
        PlayerSprite.color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.08f);
        PlayerSprite.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.08f);
        PlayerSprite.color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.08f);
        PlayerSprite.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.08f);
        PlayerSprite.color = new Color(255, 255, 255, 255);
    }
}
