using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class targetAI : MonoBehaviour
{
    public TargetManaging manager;
    public float health;
    public float maxHealth;
    public Transform player;
    public GameObject bot;
    Component AI;
    public float pathfindSpeed;
    public float raycastTimer = 0.5f;
    bool canSee;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("TargetManager").GetComponent<TargetManaging>();
        Destroy(gameObject, manager.lifetime);
        manager.isSpawned = true;
        player = GameObject.Find("PlayerPos").transform;
        maxHealth = health;
    }

    void Update()
    {
        bot.GetComponent<AIPath>().maxSpeed = 4f * Mathf.Max(pathfindSpeed,GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().noiseLevel+1) -1f;
        //raycast timer
        if (raycastTimer >= 0)
        {
            raycastTimer -= Time.deltaTime;
            if (canSee)
            {
                pathfindSpeed = 2f;
            }
            else
            {
                pathfindSpeed -= Time.deltaTime;
                if (pathfindSpeed < 1)
                {
                    pathfindSpeed = 1;
                }
            }
        }
        else
        {
            raycastTimer = 0.5f;
            canSee = false;
            if (raycast())
            {
                canSee = true;
            }
        }




        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        if (health<=0)
        {
            death();
            GameObject.Find("TargetManager").GetComponent<TargetManaging>().spawn();
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
    bool raycast()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position,transform.forward, out hit, 100))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
}
