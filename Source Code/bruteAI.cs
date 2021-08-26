using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class bruteAI : MonoBehaviour
{
    public bruteManaging manager;
    public float health;
    public float maxHealth;
    public GameObject brute;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("bruteManager").GetComponent<bruteManaging>();
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
            //GameObject.Find("bruteManager").GetComponent<bruteManaging>().spawn();
        }
        if (getDistance(brute.transform.position, player.position) <= 15)
        {
            brute.GetComponent<AIPath>().maxSpeed = 17f;
        }
        else
        {
            brute.GetComponent<AIPath>().maxSpeed = 3.5f;
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
    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }
}
