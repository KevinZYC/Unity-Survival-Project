using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GunnerAI : MonoBehaviour
{
    public GunnerManaging manager;
    public float health;
    public float maxHealth;
    public Transform player;
    Component AI;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("gunnerManager").GetComponent<GunnerManaging>();
        Destroy(gameObject, manager.lifetime);
        manager.isSpawned = true;
        player = GameObject.Find("PlayerPos").transform;
        maxHealth = health;
    }

    void Update()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        if (health<=0)
        {
            death();
            GameObject.Find("gunnerManager").GetComponent<GunnerManaging>().spawn();
        }
        
    }
    // Update is called once per frame
    void OnDestroy()
    {
        manager.isSpawned = false;
    }
    public void death()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding");
    }
}
