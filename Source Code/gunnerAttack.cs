using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class gunnerAttack : MonoBehaviour
{
    public float damage;
    public float attackCooldown = 1f;
    public float elapsedTime;
    public bool touching;
    bool isAttacking;
    public Animator attackAnimator;
    public GameObject gunner;
    public GameObject bullet_prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = attackCooldown/1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (getDistance(gunner.transform.position, GameObject.Find("PlayerPos").transform.position)<=35f)
        {
            gunner.GetComponent<AIPath>().enabled = false;
            if (elapsedTime > 0)
            {

                //gunner.transform.Find("gunnerattack").GetComponent<Animator>().SetTrigger("attacking");
                //gunner.transform.Find("gunnerattack").GetComponent<Animator>().SetTrigger("stopAttacking");
                elapsedTime -= Time.deltaTime;
            }
            
            if (elapsedTime <= 0)
            {
                shoot();
                elapsedTime = attackCooldown;
                
            }
        }
        else
        {
            gunner.GetComponent<AIPath>().enabled = true;
            elapsedTime = attackCooldown/1.5f;

            
            gunner.transform.Find("gunnerwalk1").GetComponent<Animator>().SetTrigger("startWalk");
            gunner.transform.Find("gunnerwalk1").GetComponent<Animator>().SetTrigger("endWalk");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touching = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touching = false;
        }
    }
    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }

    void shoot()
    {
        GameObject bullet = Instantiate(bullet_prefab, gunner.transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(40f*transform.forward, ForceMode.Impulse);
    }
}
