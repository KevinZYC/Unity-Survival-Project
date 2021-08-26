using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    float damage = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("bulletspawn");
        transform.LookAt(GameObject.Find("PlayerPos").transform);
        transform.localEulerAngles -= new Vector3(92, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hitsomething " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("PlayerPos").GetComponent<PlayerMovement>().health -= damage;
        }
        Destroy(gameObject);
    }
    
}
