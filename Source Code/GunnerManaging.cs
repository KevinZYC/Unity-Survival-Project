using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerManaging : MonoBehaviour
{
    public float lifetime;
    public float interval;
    public bool isSpawned;
    public GameObject gunner;
    public float countdown;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(gunner, new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-8f, -8f), UnityEngine.Random.Range(-100f, 100f)), Quaternion.identity);
        //Instantiate(gunner, new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-8f, -8f), UnityEngine.Random.Range(-100f, 100f)), Quaternion.identity);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!isSpawned && countdown >= 0)
        {
            countdown -= Time.deltaTime;
        }
        if (!isSpawned && countdown <= 0)
        {
            Instantiate(gunner, new Vector3(-7.5f, UnityEngine.Random.Range(-8f, -8f), UnityEngine.Random.Range(-6.5f, 6.5f)), Quaternion.identity);
            countdown = interval;
        }
        */
    }
    public void spawn()
    {
        Instantiate(gunner, new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-8f, -8f), UnityEngine.Random.Range(-100f, 100f)), Quaternion.identity);
    }
}
