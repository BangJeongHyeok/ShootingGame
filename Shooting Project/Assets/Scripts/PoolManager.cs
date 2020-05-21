using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> Bullets;

    // Start is called before the first frame update
    void Start()
    {
        Bullets = new List<GameObject>();
        for (int i = 0; i < 64; i++)
        {
            Bullets.Add(null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Check()
    {

    }
    
}
