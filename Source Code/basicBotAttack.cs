using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class basicBotAttack : MonoBehaviour
{
    public float damage = 15f;
    public float attackCooldown = 1f;
    public float elapsedTime;
    public bool touching;
    public Animator attackAnimator;
    public GameObject bot;
    bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = attackCooldown/3;
    }

    // Update is called once per frame
    void Update()
    {
        if (touching || isAttacking)
        {
            bot.GetComponent<AIPath>().enabled = false;
            isAttacking = true;
            bot.transform.Find("humanoid_attack1").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = true;
            bot.transform.Find("humanwalk2").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = false;
            if (elapsedTime > 0)
            {
                bot.transform.Find("humanoid_attack1").GetComponent<Animator>().SetTrigger("attacking");
                bot.transform.Find("humanoid_attack1").GetComponent<Animator>().SetTrigger("stopAttacking");
                elapsedTime -= Time.deltaTime;
            }
            
            if (elapsedTime <= 0)
            {
                isAttacking = false;
                elapsedTime = attackCooldown;
                if(touching){
                    GameObject.Find("PlayerPos").GetComponent<PlayerMovement>().health -= damage;
                    Debug.Log("damaged");
                }
                
            }
        }
        else
        {
            bot.GetComponent<AIPath>().enabled = true;
            
            elapsedTime = attackCooldown/3;
            bot.transform.Find("humanoid_attack1").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = false;
            bot.transform.Find("humanwalk2").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = true;
            bot.transform.Find("humanwalk2").GetComponent<Animator>().SetTrigger("startWalk");
            bot.transform.Find("humanwalk2").GetComponent<Animator>().SetTrigger("endWalk");
            
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
}
