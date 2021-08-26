using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSpawning : MonoBehaviour
{
    public GameObject item;
    public List<Transform> itemList = new List<Transform>();
    GameObject player;
    public int closestGun;
    GameObject closest;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerPos");
        int[] common = { 23, 25, 26 };
        int[] uncommon = { 10, 13, 15, 18, 20, 24, 28};
        int[] rare = {4, 5, 6 ,14, 16 };
        int[] elite = {0, 1, 2, 7, 8, 9, 12 };
        int[] legendary = {3, 11 ,22 ,27 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject curItem = Instantiate(item) as GameObject;
            curItem.transform.position = GameObject.Find("PlayerPos").transform.position + new Vector3(0.0f, 10f, 0.0f);
            curItem.GetComponent<gunItemControl>().tier = 1;
            itemList.Add(curItem.transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject curItem = Instantiate(item) as GameObject;
            curItem.transform.position = GameObject.Find("PlayerPos").transform.position + new Vector3(0.0f, 10f, 0.0f);
            curItem.GetComponent<gunItemControl>().tier = 2;
            itemList.Add(curItem.transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject curItem = Instantiate(item) as GameObject;
            curItem.transform.position = GameObject.Find("PlayerPos").transform.position + new Vector3(0.0f, 10f, 0.0f);
            curItem.GetComponent<gunItemControl>().tier = 3;
            itemList.Add(curItem.transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject curItem = Instantiate(item) as GameObject;
            curItem.transform.position = GameObject.Find("PlayerPos").transform.position + new Vector3(0.0f, 10f, 0.0f);
            curItem.GetComponent<gunItemControl>().tier = 4;
            itemList.Add(curItem.transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject curItem = Instantiate(item) as GameObject;
            curItem.transform.position = GameObject.Find("PlayerPos").transform.position + new Vector3(0.0f, 10f, 0.0f);
            curItem.GetComponent<gunItemControl>().tier = 5;
            itemList.Add(curItem.transform);
        }

        if(itemList.Count>0){
            closest = GetClosest(itemList).gameObject;
            if (getDistance(closest.transform.position, player.transform.position) <= 15f)
            {
                closestGun = closest.GetComponent<gunItemControl>().gun;
            }
            else
            {
                closestGun = -1;
            }
        }
        else
        {
            closestGun = -1;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            //itemList.Remove(closest.transform);
            if (getDistance(closest.transform.position, player.transform.position)<=15f)
            {
                closest.GetComponent<gunItemControl>().pickup();
            }
            
        }
    }

    Transform GetClosest(List<Transform> items)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        foreach (Transform potentialTarget in items)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        
        return bestTarget;
    }

    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }
}
