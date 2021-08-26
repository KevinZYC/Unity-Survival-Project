using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class statControl : MonoBehaviour
{
    public GameObject player;
    int WeaponSelected;
    float dmg;
    float dmgHead;
    float rof;
    float mag;
    float optRange;
    float dmgDrop;
    float spread;
    float ADSSpread;
    float recoil;
    float recoilRec;
    float hRecoil;
    float speed;
    float zoom;
    float sprayAccuracy;
    int burst;

    public GameObject damageBar;
    public GameObject firepowerBar;
    public GameObject rangeBar;
    public GameObject recoilBar;
    public GameObject mobilityBar;
    public GameObject damageBar1;
    public GameObject firepowerBar1;
    public GameObject rangeBar1;
    public GameObject recoilBar1;
    public GameObject mobilityBar1;
    public Text weaponText2;
    public Text text2;
    public int nearbyGun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nearbyGun = GameObject.Find("itemSpawner").GetComponent<itemSpawning>().closestGun;

        WeaponSelected = GameObject.Find("PlayerPos").GetComponent<ShootRaycast>().WeaponSelected;
        rof = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected];
        recoil = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoil[WeaponSelected];
        zoom = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().scopeZoom[WeaponSelected];
        spread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().spread[WeaponSelected];
        ADSSpread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ADSSpread[WeaponSelected];
        dmg = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().bodyDamage[WeaponSelected];
        dmgHead = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().headDamage[WeaponSelected];
        hRecoil = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().hRecoil[WeaponSelected];
        mag = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected];
        recoilRec = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoilRecovery[WeaponSelected];
        optRange = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().optimalRange[WeaponSelected];
        dmgDrop = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().damageFalloff[WeaponSelected];
        burst = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().burstType[WeaponSelected];
        speed = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().speed[WeaponSelected];
        sprayAccuracy = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().sprayAccuracy[WeaponSelected];

        float dmgV = 2*dmg + dmgHead;
        if (burst >= 5)
        {
            dmgV *= burst / 1.5f;
        }
        if (dmgHead >= 100)
        {
            dmgV += 15;
        }

        damageBar.GetComponent<Slider>().value = Mathf.Min(1f, dmgV / 500f);

        float rofV = rof;
        if (burst > 1 && burst < 5)
        {
            rofV *= 0.5f*(6-burst);
        }
        
        firepowerBar.GetComponent<Slider>().value = Mathf.Min(1f, (30/rofV + mag*1.5f)/700+0.1f);

        rangeBar.GetComponent<Slider>().value = Mathf.Min(1f, (4-Mathf.Min(spread*100,ADSSpread*100,4) + 2.5f-dmgDrop*4f)/7);
        if (sprayAccuracy < 0)
        {
            rangeBar.GetComponent<Slider>().value *= 1.5f;
        }
        recoilBar.GetComponent<Slider>().value = Mathf.Min(1f, (recoil*(3.3f+0.5f/rofV) + hRecoil*2)/65);
        mobilityBar.GetComponent<Slider>().value = Mathf.Min(1f, (speed-0.5f)*2);

        //for pickup gun
        if (nearbyGun >= 0)
        {
            damageBar1.SetActive(true);
            firepowerBar1.SetActive(true);
            rangeBar1.SetActive(true);
            recoilBar1.SetActive(true);
            mobilityBar1.SetActive(true);
            text2.gameObject.SetActive(true);
            

            WeaponSelected = nearbyGun;
            weaponText2.text = "Nearby: " + GameObject.Find("PlayerPos").GetComponent<WeaponStats>().name[WeaponSelected];
            rof = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected];
            recoil = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoil[WeaponSelected];
            zoom = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().scopeZoom[WeaponSelected];
            spread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().spread[WeaponSelected];
            ADSSpread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ADSSpread[WeaponSelected];
            dmg = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().bodyDamage[WeaponSelected];
            dmgHead = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().headDamage[WeaponSelected];
            hRecoil = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().hRecoil[WeaponSelected];
            mag = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected];
            recoilRec = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoilRecovery[WeaponSelected];
            optRange = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().optimalRange[WeaponSelected];
            dmgDrop = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().damageFalloff[WeaponSelected];
            burst = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().burstType[WeaponSelected];
            speed = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().speed[WeaponSelected];
            sprayAccuracy = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().sprayAccuracy[WeaponSelected];

            dmgV = 2*dmg + dmgHead;
            if (burst >= 5)
            {
                dmgV *= burst / 1.5f;
            }
            if (dmgHead >= 100)
            {
                dmgV += 15;
            }

            damageBar1.GetComponent<Slider>().value = Mathf.Min(1f, dmgV / 500f);

            rofV = rof;
            if (burst > 1 && burst < 5)
            {
                rofV *= 0.5f * (6 - burst);
            }

            firepowerBar1.GetComponent<Slider>().value = Mathf.Min(1f, (30 / rofV + mag * 1.5f) / 700 + 0.1f);

            rangeBar1.GetComponent<Slider>().value = Mathf.Min(1f, (4 - Mathf.Min(spread * 100, ADSSpread * 100, 4) + 2.5f - dmgDrop * 4f) / 7);
            if (sprayAccuracy < 0)
            {
                rangeBar1.GetComponent<Slider>().value *= 1.5f;
            }
            recoilBar1.GetComponent<Slider>().value = Mathf.Min(1f, (recoil * (3.3f + 0.5f / rofV) + hRecoil * 2) / 65);
            mobilityBar1.GetComponent<Slider>().value = Mathf.Min(1f, (speed - 0.5f) * 2);
        }
        else
        {
            damageBar1.SetActive(false);
            firepowerBar1.SetActive(false);
            rangeBar1.SetActive(false);
            recoilBar1.SetActive(false);
            mobilityBar1.SetActive(false);
            text2.gameObject.SetActive(false);
            weaponText2.text = "";
        }
        
    }
}
