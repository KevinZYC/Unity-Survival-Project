using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bruteManaging : MonoBehaviour
{
    public float lifetime;
    public float interval;
    public bool isSpawned;
    public GameObject target;
    public float countdown;
    Vector3 spawnPlace;
    // Start is called before the first frame update
    
    void Start()
    {
        countdown = interval;
        //Instantiate(target, new Vector3(-7.5f, UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-6.5f, 6.5f)), Quaternion.identity);
        //Instantiate(target, new Vector3(-7.5f, UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-6.5f, 6.5f)), Quaternion.identity);
        Instantiate(target, new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-100f, 100f)), Quaternion.identity);
        //Instantiate(target, new Vector3(-7.5f, UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-6.5f, 6.5f)), Quaternion.identity);
        //Instantiate(target, new Vector3(-7.5f, UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-6.5f, 6.5f)), Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isSpawned && countdown >= 0)
        {
            countdown -= Time.deltaTime;
        }
        if (!isSpawned && countdown <= 0)
        {
            Instantiate(target, new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-100f, 100f)), Quaternion.identity);
            countdown = interval;
        }
        
    }
    public void spawn()
    {
        do
        {
            spawnPlace = new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-7.2f, -7.2f), UnityEngine.Random.Range(-100f, 100f));
        }
        while (getDistance(GameObject.Find("PlayerPos").transform.position, spawnPlace) >= 75);
        Instantiate(target, spawnPlace, Quaternion.identity);
    }

    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }
}
