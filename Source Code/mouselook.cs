using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouselook : MonoBehaviour
{
    public float sens;
    public float hipFireSens;
    public float zoomSens;
    public Transform playerRotation;
    public float xrotation;
    public float lastStartedShots; // the last vertical coord where totalRecoil = 0
    public bool shouldCompensate;
    public bool vertRecoil = true;
    public float recoilAdd;
    public Text sensText;
    public int hRecoilDirection;
    public float hRecoil;
    // Start is called before the first frame update
    void Start()
    {
        sens = PlayerPrefs.GetFloat("sens");
        hipFireSens = sens;
        zoomSens = hipFireSens / GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().zoom;
        sensText.text = sens.ToString();
        Cursor.lockState = CursorLockMode.Locked;
        zoomSens = hipFireSens / GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().zoom;
    }

    // Update is called once per frame
    void Update()
    {
        hRecoil= GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().hRecoil;
        zoomSens = hipFireSens / GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().zoom;
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(sens>2)
            {
                sens -= 1;
                hipFireSens = sens;
                zoomSens = hipFireSens / GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().zoom;
                sensText.text = sens.ToString();
                PlayerPrefs.SetFloat("sens", sens);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (sens < 100)
            {
                sens += 1;
                hipFireSens = sens;
                zoomSens = hipFireSens / GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().zoom;
                sensText.text = sens.ToString();
                PlayerPrefs.SetFloat("sens", sens);
            }
        }

        if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().isADS)
        {
            sens = zoomSens;
        }
        else
        {
            sens = hipFireSens;
        }
        float mousex = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mousey = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().totalRecoil<= GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().recoilHeight+2)
        {
            recoilAdd = GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().totalRecoil;
        }
        //Debug.Log(recoilAdd);
        xrotation -= mousey;
        xrotation = Mathf.Clamp(xrotation, -90f - recoilAdd, 90f - recoilAdd);
        if (recoilAdd<=0)
        {
            hRecoilDirection = 0;
            recoilAdd = 0;
        }
        if (recoilAdd<GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().recoilHeight)
        {
            transform.localRotation = Quaternion.Euler(xrotation - recoilAdd, 0f, 0f);
            playerRotation.Rotate(Vector3.up * mousex);
        }
        if (recoilAdd >= GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().recoilHeight)
        {
            Debug.Log("horizontal");
            if (hRecoilDirection < 2)
            {
                transform.localRotation = Quaternion.Euler(xrotation - recoilAdd + UnityEngine.Random.Range(-1.5f, -1f), 0f, 0f);
                playerRotation.Rotate(Vector3.up * (mousex + UnityEngine.Random.Range(hRecoil * -1, 0)));
                hRecoilDirection += 1;

            }
            else
            {
                if (((hRecoilDirection)) % 10 <= 4)
                {
                    transform.localRotation = Quaternion.Euler(xrotation - recoilAdd + UnityEngine.Random.Range(1f, 1.5f), 0f, 0f);
                    playerRotation.Rotate(Vector3.up * (mousex + UnityEngine.Random.Range(0, hRecoil)));
                    hRecoilDirection += 1;


                }
                else if (((hRecoilDirection)) % 10 >= 5)
                {
                    transform.localRotation = Quaternion.Euler(xrotation - recoilAdd + UnityEngine.Random.Range(-1.5f, -1f), 0f, 0f);
                    playerRotation.Rotate(Vector3.up * (mousex + UnityEngine.Random.Range(hRecoil * -1, 0)));
                    hRecoilDirection += 1;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(xrotation - recoilAdd + UnityEngine.Random.Range(1, 1.5f), 0f, 0f);
                    playerRotation.Rotate(Vector3.up * (mousex + UnityEngine.Random.Range(0, hRecoil)));
                    hRecoilDirection = -4;
                }
            }
        }
        
        
        /*if (GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().totalRecoil <= 0)
        {
            lastStartedShots = xrotation;
        }
        Debug.Log(xrotation);
        Debug.Log(lastStartedShots);
        if (xrotation <= lastStartedShots)
        {
            GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().totalRecoil = 0;
        }*/
    }
}
