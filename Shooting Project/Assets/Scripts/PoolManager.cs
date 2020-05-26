using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> Bullets;
    public List<GameObject> Explosions;
    public List<GameObject> Explosions2;

    // Start is called before the first frame update
    void Start()
    {
        Bullets = new List<GameObject>();
        for (int i = 0; i < 48; i++)
        {
            Bullets.Add(null);
        }

        Explosions = new List<GameObject>();
        for (int i = 0; i < 48; i++)
        {
            Explosions.Add(null);
        }

        Explosions2 = new List<GameObject>();
        for (int i = 0; i < 48; i++)
        {
            Explosions2.Add(null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
