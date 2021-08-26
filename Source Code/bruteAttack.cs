using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class bruteAttack : MonoBehaviour
{
    public float damage;
    public float attackCooldown = 1f;
    public float elapsedTime;
    public bool touching;
    bool isAttacking;
    public Animator attackAnimator;
    public GameObject brute;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = attackCooldown/1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (touching || isAttacking)
        {
            isAttacking = true;
            brute.GetComponent<AIPath>().enabled = false;
            brute.transform.Find("bruteattack").transform.Find("Cube").GetComponent<MeshRenderer>().enabled = true;
            brute.transform.Find("bruteattack").transform.Find("Cube").transform.Find("Cube.001").GetComponent<MeshRenderer>().enabled = true;
            brute.transform.Find("bruteattack").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = true;
            brute.transform.Find("brutewalk1").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = false;
            brute.transform.Find("brutewalk1").transform.Find("Cube").GetComponent<MeshRenderer>().enabled = false;
            brute.transform.Find("brutewalk1").transform.Find("Cube").transform.Find("Cube.001").GetComponent<MeshRenderer>().enabled = false;
            if (elapsedTime > 0)
            {

                brute.transform.Find("bruteattack").GetComponent<Animator>().SetTrigger("attacking");
                brute.transform.Find("bruteattack").GetComponent<Animator>().SetTrigger("stopAttacking");
                elapsedTime -= Time.deltaTime;
            }
            
            if (elapsedTime <= 0)
            {
                isAttacking = false;
                elapsedTime = attackCooldown;
                if (touching)
                {
                    GameObject.Find("PlayerPos").GetComponent<PlayerMovement>().health -= damage;
                    Debug.Log("damaged");
                }
                
            }
        }
        else
        {
            brute.GetComponent<AIPath>().enabled = true;
            elapsedTime = attackCooldown/1.5f;

            brute.transform.Find("bruteattack").transform.Find("Cube").GetComponent<MeshRenderer>().enabled = false;
            brute.transform.Find("bruteattack").transform.Find("Cube").transform.Find("Cube.001").GetComponent<MeshRenderer>().enabled = false;
            brute.transform.Find("bruteattack").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = false;
            brute.transform.Find("brutewalk1").transform.Find("char.001").GetComponent<SkinnedMeshRenderer>().enabled = true;
            brute.transform.Find("brutewalk1").transform.Find("Cube").GetComponent<MeshRenderer>().enabled = true;
            brute.transform.Find("brutewalk1").transform.Find("Cube").transform.Find("Cube.001").GetComponent<MeshRenderer>().enabled = true;
            brute.transform.Find("brutewalk1").GetComponent<Animator>().SetTrigger("startWalk");
            brute.transform.Find("brutewalk1").GetComponent<Animator>().SetTrigger("endWalk");
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
