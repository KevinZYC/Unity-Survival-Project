using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunItemControl : MonoBehaviour
{
    public int gun;
    GameObject gunModel;
    GameObject player;
    public Transform checkGround; 
    bool isClosest;
    public int tier;
    public static float closestDist = 9999f;
    public float groundDistance = 200f;
    public LayerMask groundMask;
    int[] common = { 23, 25, 26 };
    int[] uncommon = { 10, 13, 15, 18, 20, 24, 28 };
    int[] rare = { 4, 5, 6, 14, 16 };
    int[] elite = { 0, 1, 2, 7, 8, 9, 12 };
    int[] legendary = { 3, 11, 22, 27 };
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
        int rnd;
        player = GameObject.Find("PlayerPos");
        if (tier == 1)
        {
            rnd = UnityEngine.Random.Range(0, common.Length);
            gun = common[rnd];
        }
        if (tier == 2)
        {
            rnd = UnityEngine.Random.Range(0, uncommon.Length);
            gun = uncommon[rnd];
        }
        if (tier == 3)
        {
            rnd = UnityEngine.Random.Range(0, rare.Length);
            gun = rare[rnd];
        }
        if (tier == 4)
        {
            rnd = UnityEngine.Random.Range(0, elite.Length);
            gun = elite[rnd];
        }
        if (tier == 5)
        {
            rnd = UnityEngine.Random.Range(0, legendary.Length);
            gun = legendary[rnd];
        }
       
        gunModel = Instantiate(GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().modelList[gun], gameObject.transform);
        HideChildWithName(gunModel, "bo2_fbi_short");
        HideChildWithName(gunModel, "bo2_fbi_short.001");
        HideChildWithName(gunModel, "bo2_fbi_short.002");
        Debug.Log(gun);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(checkGround.transform.position, groundDistance, groundMask);
        if (!isGrounded)
        {
            gameObject.transform.position -= new Vector3(0.0f, Time.deltaTime*15, 0.0f);
        }

        //gameObject.GetComponent<Rigidbody>().useGravity = !isGrounded;
        float dist = getDistance(player.transform.position, gunModel.transform.position);
        gunModel.transform.position = gameObject.transform.position;
        if (dist < 15 && Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }

    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }

    public void pickup()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, GameObject.Find("Main Camera").transform.forward, out hit, 25))
        {
            //Debug.Log("yes");
            if (hit.transform.tag == "weapon")
            {
                Debug.Log("canPick");

            }
        }
        */
        if (GameObject.Find("PlayerPos").GetComponent<WeaponStats>().isPistol[gun] == 1)
        {
            GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().setWeapon(2, gun);
            Destroy(gunModel);
            Destroy(gameObject);
            closestDist = 9999f;
            GameObject.Find("itemSpawner").GetComponent<itemSpawning>().itemList.Remove(gameObject.transform);

        }
        else if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().isPistol != 1)
        {
            GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().setWeapon(gun);
            Destroy(gunModel);
            Destroy(gameObject);
            closestDist = 9999f;
            GameObject.Find("itemSpawner").GetComponent<itemSpawning>().itemList.Remove(gameObject.transform);
        }
        else if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().gunIDs[0] == -1)
        {
            GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().setWeapon(0, gun);
            Destroy(gunModel);
            Destroy(gameObject);
            closestDist = 9999f;
            GameObject.Find("itemSpawner").GetComponent<itemSpawning>().itemList.Remove(gameObject.transform);
        }
        else if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().gunIDs[1] == -1)
        {
            GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().setWeapon(1, gun);
            Destroy(gunModel);
            Destroy(gameObject);
            closestDist = 9999f;
            GameObject.Find("itemSpawner").GetComponent<itemSpawning>().itemList.Remove(gameObject.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    void HideChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            childTrans.GetComponent<Renderer>().enabled=false;
        }
        
    }
}
