using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShootRaycast : MonoBehaviour
{
    //public Animator anim;
    public int WeaponSelected;
    public Camera mainCamera;
    public Transform muzzle;
    public GameObject scopeUI;
    public GameObject acogUI;
    public GameObject weaponModel;
    public GameObject[] modelList = new GameObject[4];
    public GameObject[] animList = new GameObject[20];
    public int animCreated = 40;
    public Transform camera;
    public Transform playerRotation;
    public LayerMask canBeShot;
    public int score;
    public Text scoreText;
    public GameObject tracerObject;
    public float recoilUp;
    public float totalRecoil = 0;
    public float countdown;
    public float firingRate = 0.2f;
    public float fireCooldown;
    public float recoilPower;
    public bool canShoot = true;
    public int burstShot;
    public float zoom;
    public float spread;
    public float ADSSpread;
    public bool canADS;
    public float recoilSpread;
    public float damage;
    public float HSdamage;
    public int hasAction;
    public GameObject curGun;
    public Transform hands;
    public int magCount;
    public int ammoCount;
    public bool isReloading;
    public float reloadTime;
    public float reloadSpeed;
    public GameObject splatter;
    public float splatterDelCount;
    public GameObject[] splatterPrefabs = new GameObject[12];
    public GameObject hitmarkBody;
    public GameObject hitmarkHead;
    public float bodyMarkCD;
    public float headMarkCD;
    public bool bodyMark;
    public bool headMark;
    public bool isBursting;
    public GameObject crossTop;
    public GameObject crossLeft;
    public GameObject crossDown;
    public GameObject crossRight;
    public GameObject bulletholePrefab;
    public GameObject EnemyHealthBar;
    public GameObject crossDot;
    public GameObject EnemyHBAnchor;
    public Text WeaponText;
    public GameObject lerpAnchor;
    public string gunName;
    public float lerpTime;
    public float unlerpTime;
    public float endLerpTime;
    public float hRecoil;
    public float recoilRecovery;
    public GameObject curGunBolt;
    public float lerpTimeR;
    public float unlerpTimeR;
    public float endLerpTimeR;
    public float adsProcessSpread;
    public int burst; //0=single,1=auto,2-5=burst,6+=shotgun
                      // Start is called before the first frame update
    public Vector3 origPos;
    public bool isLerping;
    public bool isLerpingR;
    public GameObject GunAnchor;
    public GameObject ReloadAnchor;
    public Animator curAnim;
    public bool inBoltAction;
    public AudioClip gunshotGeneric;
    public AudioClip gunshotSuppressed;
    public AudioClip gunshotHeavy;
    public float lerpStep;
    public float curSpread;
    Quaternion basicRotation;
    bool inSwapTime;
    float swapTime;
    float swapSpeed;
    int canSlamfire;
    public bool isADS;
    float minimumDamage;
    float damageFalloff;
    float optimalRange;
    float damageReduced;
    public float noiseLevel;
    public float recoilHeight;
    public int isPistol;
    float noise;
    int[] mags = new int[3];
    int[] reserves = new int[3];
    public int[] gunIDs = new int[3];
    int curSlot;
    public Vector3 sightPosDiff;
    public bool canIronSight;
    public GameObject GunAnchor2; //iron sight anchor
    public bool inIronSight;
    
    void Start()
    {
        //curGun = Instantiate(modelList[WeaponSelected], camera.gameObject.transform) as GameObject;
        //curGun.transform.position = hands.position;

        setWeapon(0, 0);
        setWeapon(1, 9);
        setWeapon(2, 21);
        changeWeapon(21, mags[2], reserves[2]);
        curSlot = 2;
        scoreText = GameObject.Find("score").GetComponent<Text>();
        fireCooldown = firingRate;
        burstShot = burst;
        hitmarkBody.SetActive(false);
        hitmarkHead.SetActive(false);
        origPos = curGun.transform.position;
        curAnim = curGun.GetComponent<Animator>();
        splatterDelCount = 0.45f;
        if (!canADS)
        {
            ADSSpread = spread;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (inBoltAction && curGunBolt!=null)
        {
            curGunBolt.transform.position = curGun.transform.position;// - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0);
            curGunBolt.transform.rotation = curGun.transform.rotation;
        }

        Transform sightTrans = FindChildWithName(curGun, "sight");
        if (sightTrans != null)
        {
            sightPosDiff = sightTrans.position - curGun.transform.position;
            canIronSight = true;
        }
        else
        {
            canIronSight = false;
        }
        

        //Debug.Log(gameObject.GetComponent<PlayerMovement>().isGrounded && !(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")));
        if (inIronSight || (isADS && gameObject.GetComponent<PlayerMovement>().isGrounded && !(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))))
        {
            if(curSpread > ADSSpread)
            {
                curSpread -= Time.deltaTime*(zoom*0.1f)*0.75f;
            }
            if (curSpread <= ADSSpread)
            {
                curSpread = ADSSpread;
            }
            
        }
        else
        {
            if (curSpread < spread)
            {
                curSpread += Time.deltaTime * 1.2f;
            }
            
            if (curSpread >= ADSSpread)
            {
                curSpread = spread;
            }
        }

        WeaponText.text = "Equipped: "+gunName;

        if (!(canSlamfire == 1&&Input.GetMouseButton(1)))
        {
            spread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().spread[WeaponSelected];
            firingRate = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected];

           
        }

        if (noiseLevel > 0)
        {
            noiseLevel -= Time.deltaTime/5;
        }
        else
        {
            noiseLevel = 0;
        }
        
        //Debug.Log(hasAction);
        recoilSpread = totalRecoil / (400 * GameObject.Find("PlayerPos").GetComponent<WeaponStats>().sprayAccuracy[WeaponSelected]);
        if (isLerping && !isReloading)
        {
            
            origPos = curGun.transform.position;
            //Debug.Log("origPos " + origPos);
            if (!(canSlamfire == 1 && Input.GetMouseButton(1)))
            {
                lerpStep = 5f * Time.deltaTime;
                
                if (!inIronSight)
                {
                    curGun.transform.position = Vector3.Lerp(curGun.transform.position, lerpAnchor.transform.position - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0), lerpStep);
                }
                else if(totalRecoil>0)
                {
                    curGun.transform.position = Vector3.Lerp(curGun.transform.position, lerpAnchor.transform.position - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0)+sightPosDiff, lerpStep);
                }
                Quaternion wantedRotation = lerpAnchor.transform.rotation;
                curGun.transform.rotation = Quaternion.RotateTowards(curGun.transform.rotation, wantedRotation, Time.deltaTime * 25f);
            }
            if (endLerpTime > 0)
            {
                endLerpTime -= Time.deltaTime;
            }
            else
            {
                isLerping = false;
            }


            /*
            if (lerpTime < endLerpTime/2)
            {
                curGun.transform.position = Vector3.Lerp(curGun.transform.position, lerpAnchor.transform.position, step);
                lerpTime += Time.deltaTime;
                unlerpTime = 0.01f;
            }
            if (lerpTime >= endLerpTime/2)
            {
                unlerpTime = 0;
                //isLerping = false;
            }


            if (unlerpTime< endLerpTime/2)
            {
                curGun.transform.position = Vector3.Lerp(curGun.transform.position, GunAnchor.transform.position, step);
                unlerpTime += Time.deltaTime;
            }
            if (unlerpTime >= endLerpTime/2)
            {
                lerpTime = 0;
                unlerpTime = 0;
                Debug.Log("end lerp");
                isLerping = false;
            }
            */
        }
        else
        {
            
            if (inSwapTime)
            {
                lerpStep = 3f/swapSpeed * Time.deltaTime;
            }
            else
            {
                lerpStep = 4f * Time.deltaTime;
            }
            if (!inIronSight)
            {
                curGun.transform.position = Vector3.Lerp(curGun.transform.position, GunAnchor.transform.position - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0), lerpStep);
            }
            else
            {
                curGun.transform.position = Vector3.Lerp(curGun.transform.position, GunAnchor.transform.position - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0)+sightPosDiff, lerpStep);
            }
            
            curGun.transform.rotation = Quaternion.RotateTowards(curGun.transform.rotation, GunAnchor.transform.rotation, Time.deltaTime * 6f);
            lerpTime = 0;
            unlerpTime = 0;
            isLerping = false;
        }
       
        /*
        if (isReloading && !isLerpingR)
        {
            lerpTime = 0;
            unlerpTime = 0;
            //curGun.transform.position = Vector3.Lerp(curGun.transform.position, hands.transform.position, unlerpTime);
        }
        */
        
        if (isLerpingR)
        {
            endLerpTimeR = reloadSpeed;
            
            if (lerpTimeR < endLerpTimeR / 2)
            {
                //curGun.transform.position = Vector3.Lerp(curGun.transform.position, ReloadAnchor.transform.position, lerpTimeR/2);
                lerpTimeR += Time.deltaTime;
                unlerpTimeR = -0.0001f;
            }
            if (lerpTimeR <= endLerpTimeR/2)
            {
                unlerpTimeR = -0.0001f;
                lerpTimeR += Time.deltaTime;
                //isLerping = false;
            }

            if (unlerpTimeR < endLerpTimeR / 2.5)
            {
                unlerpTimeR += Time.deltaTime;
            }

            if (unlerpTimeR < endLerpTimeR/2 && unlerpTimeR >=endLerpTimeR/2.5)
            {
                //curGun.transform.position = Vector3.Lerp(curGun.transform.position, GunAnchor.transform.position, unlerpTimeR/2);
                unlerpTimeR += Time.deltaTime;
            }
            if (unlerpTimeR >= endLerpTimeR/2)
            {
                lerpTimeR = 0;
                unlerpTimeR = 0;
                isReloading = false;
                isLerpingR = false;
                reload();
            }
        }
        else
        {
            lerpTimeR = 0;
            unlerpTimeR = 0;
        }

        if (!isADS && zoom>=4.5f)
        {
            
            crossTop.SetActive(false);
            crossLeft.SetActive(false);
            crossDown.SetActive(false);
            crossRight.SetActive(false);
            crossDot.SetActive(false);
            

        }
        else
        {
            crossTop.SetActive(true);
            crossLeft.SetActive(true);
            crossDown.SetActive(true);
            crossRight.SetActive(true);
            crossDot.SetActive(true);
        }

        if (isADS)
        {
            /*
            crossTop.SetActive(false);
            crossLeft.SetActive(false);
            crossDown.SetActive(false);
            crossRight.SetActive(false);
            */
            
        }
        else
        {
            
            if (canSlamfire != 1 && Input.GetMouseButton(1) && canIronSight &&!isADS &&!isReloading)
            {
                inIronSight = true;
                curGun.transform.position = Vector3.Lerp(curGun.transform.position, GunAnchor.transform.position - new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0)+ sightPosDiff, Time.deltaTime*10f);
                //curGun.transform.rotation = Quaternion.RotateTowards(curGun.transform.rotation, GunAnchor2.transform.rotation, Time.deltaTime * 10f);
                curGun.transform.rotation = GunAnchor2.transform.rotation;
                /*
                curGun.transform.position = GunAnchor.transform.position;
                curGun.transform.position -= new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0);
                curGun.transform.position += sightPosDiff;
                curGun.transform.rotation = GunAnchor2.transform.rotation;
                */
            }
            else
            {
                inIronSight = false;
                //curGun.transform.localRotation = new Quaternion(0, 0, 0, 0);
                
                //curGun.transform.position = GunAnchor.transform.position;
                //curGun.transform.position -= new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0);
                //curGun.transform.rotation = new Quaternion(curGun.transform.rotation[0], gameObject.transform.rotation[1] - 90f, curGun.transform.rotation[2], curGun.transform.rotation[3]);
            }
            
        }
        /*
        crossTop.SetActive(true);
        crossLeft.SetActive(true);
        crossDown.SetActive(true);
        crossRight.SetActive(true);
        */
        if (isADS)
        {
            crossTop.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (curSpread*zoom + recoilSpread / 2) * 600);
            crossLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2((curSpread * zoom + recoilSpread / 2) * -600, 0);
            crossDown.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (curSpread * zoom + recoilSpread / 2) * -600);
            crossRight.GetComponent<RectTransform>().anchoredPosition = new Vector2((curSpread * zoom + recoilSpread / 2) * 600, 0);

        }
        else
        {
            crossTop.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (curSpread + recoilSpread) * 600);
            crossLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2((curSpread + recoilSpread) * -600, 0);
            crossDown.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (curSpread + recoilSpread) * -600);
            crossRight.GetComponent<RectTransform>().anchoredPosition = new Vector2((curSpread + recoilSpread) * 600, 0);
        }


        //Debug.Log(crossRight.transform.position.y);

        if (!inBoltAction &&!isReloading)
        {
            if (Input.GetKeyDown("t"))
            {
                if (WeaponSelected < modelList.Length - 1)
                {
                    WeaponSelected += 1;
                    changeWeapon(WeaponSelected, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected], 0);
                }
                else
                {
                    WeaponSelected = 0;
                    changeWeapon(WeaponSelected, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected], 0);
                }
            }

            if (Input.GetKeyDown("y"))
            {
                if (WeaponSelected <= 0)
                {
                    WeaponSelected = modelList.Length - 1;
                    changeWeapon(WeaponSelected, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected], 0);
                }
                else
                {
                    WeaponSelected -= 1;
                    changeWeapon(WeaponSelected, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected], 0);
                }
            }

            if (Input.GetKeyDown("q"))
            {
                
                mags[curSlot] = magCount;
                reserves[curSlot] = ammoCount;
                changeWeapon(gunIDs[0], mags[0], reserves[0]);
                curSlot = 0;
            }
            if (Input.GetKeyDown("e"))
            {
                
                mags[curSlot] = magCount;
                reserves[curSlot] = ammoCount;
                changeWeapon(gunIDs[1], mags[1], reserves[1]);
                curSlot = 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                
                mags[curSlot] = magCount;
                reserves[curSlot] = ammoCount;
                changeWeapon(gunIDs[2], mags[2], reserves[2]);
                curSlot = 2;
            }
        }

        

        scoreText.text = (magCount.ToString() + "/" + ammoCount.ToString());
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            curAnim.SetTrigger("startReload");
            //Debug.Log("reloading");
            curAnim.SetTrigger("endReload");
            isLerpingR = true;
            
            AnimatorStateInfo info = curAnim.GetCurrentAnimatorStateInfo(0);
            reloadTime = info.length;
            Debug.Log(info.length);
            isReloading = true;
            
        }
        if (inBoltAction)
        {
            hideGun();
        }
        else
        {
            showGun();
        }
        if ((Input.GetMouseButton(1) && canADS && !isReloading && !inBoltAction && !(isLerping && lerpTime>=0.2)))
        {
            isADS = true;
            if (zoom <= 4)
            {
                acogUI.SetActive(true);
                scopeUI.SetActive(false);
            }
            else
            {
                scopeUI.SetActive(true);
                acogUI.SetActive(false);
            }
            mainCamera.fieldOfView = 80f / zoom;

            hideGun();
            

        }
        else 
        {
            isADS = false;
            mainCamera.fieldOfView = 80f;
            scopeUI.SetActive(false);
            acogUI.SetActive(false);
            if (!inBoltAction)
            {
                showGun();
            }
            
            


        }
        if (totalRecoil > 0)
        {
            totalRecoil -= Time.deltaTime * 20 * recoilRecovery;
        }
        if (totalRecoil < 0)
        {
            totalRecoil = 0;
        }
        //Debug.Log(canShoot);
        //Debug.Log(fireCooldown);
        if (magCount>0 && !isReloading)
        {
            if (Input.GetMouseButton(1) && canSlamfire == 1)
            {
                if (canShoot)
                {
                    curSpread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ADSSpread[WeaponSelected];
                    firingRate = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected] / 3;
                    fireCooldown = firingRate;
                    runBolt();
                    inBoltAction = true;
                    shoot();
                    magCount -= 1;

                }
            }

            if (burst == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (canShoot)
                    {

                        if (hasAction == 1)
                        {
                            Debug.Log(hasAction);
                            runBolt();
                            inBoltAction = true;
                        }
                        shoot();
                        magCount -= 1;
                        
                    }
                }
            }
            else if (burst == 1)
            {
                if (Input.GetMouseButton(0))
                {
                    if (canShoot)
                    {
                        if (hasAction == 1)
                        {
                            runBolt();
                            inBoltAction = true;
                        }
                        shoot();
                        magCount -= 1;
                    }
                }
            }
            else if (burst <= 5)
            {


                

                if ((Input.GetMouseButton(0)||isBursting) && burstShot > 0 && magCount > 0)
                {
                    if (canShoot)
                    {
                        if (hasAction == 1 && burstShot==burst)
                        {
                            Debug.Log(hasAction);
                            runBolt();
                            inBoltAction = true;
                        }
                        shoot();
                        isBursting = true;
                        burstShot -= 1;
                        magCount -= 1;
                        if(burstShot <= 0)
                        {
                            fireCooldown += firingRate * 4;
                            burstShot = burst;
                            isBursting = false;
                            //inBoltAction = false;
                            recoilUp = UnityEngine.Random.Range(recoilPower, recoilPower + 0.5f);
                            totalRecoil += recoilUp;
                            if (totalRecoil > recoilHeight)
                            {
                                totalRecoil = recoilHeight+0.5f;
                            }
                        }
                    }
                }
                /*
                if (!Input.GetMouseButton(0) && burstShot <= 0)
                {
                    burstShot = burst;
                    fireCooldown = firingRate * 4;
                }
                */
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (canShoot)
                    {
                        if (hasAction == 1)
                        {
                            runBolt();
                            inBoltAction = true;
                        }
                        for (int i = 0; i < burst; i++)
                        {
                            shoot();
                        }
                        magCount -= 1;
                        
                    }
                }
            }
            
        }
        if (!canShoot && !inSwapTime && fireCooldown >= 0)
        {
            fireCooldown -= Time.deltaTime;

        }
        if (!canShoot && !inSwapTime && fireCooldown <= 0)
        {
            canShoot = true;
            fireCooldown = firingRate;
            if (!(burst > 0 && burst < 5) || !isBursting)
            {
                inBoltAction = false;
            }
            
        }



        /*
        if (isReloading && reloadTime >= 0)
        {
            reloadTime -= Time.deltaTime;
        }
        if (isReloading && reloadTime<=0)
        {
            isReloading = false;
            
            
        }
        */
        /*
        if (splatterDelCount>=0)
        {
            splatterDelCount -= Time.deltaTime;
        }
        if (splatterDelCount<=0)
        {
            int counting = 0;
            foreach (GameObject i in splatterPrefabs)
            {
                Destroy(i);
                splatterPrefabs[counting] = null;
                counting += 1;
            }
            //Array.Clear(splatterPrefabs, 0, splatterPrefabs.Length);
            splatterDelCount = 0.45f;
        }
        */
        if (hitmarkBody.active && bodyMarkCD >= 0)
        {
            bodyMarkCD -= Time.deltaTime;

        }
        if (hitmarkBody.active && bodyMarkCD <= 0)
        {
            hitmarkBody.SetActive(false);
        }

        if (hitmarkHead.active && headMarkCD >= 0)
        {
            headMarkCD -= Time.deltaTime;

        }
        if (hitmarkHead.active && headMarkCD <= 0)
        {
            hitmarkHead.SetActive(false);
        }

        if (swapTime >= 0)
        {
            inSwapTime = true;
            canShoot = false;
            swapTime -= Time.deltaTime;
        }
        else
        {
            inSwapTime = false;
        }
    }
    public void shoot()
    {
        noiseLevel = noise/2;

        if (hasAction != 1)
        {
            runLerp();
        }
        if (noise == 0f)
        {
            AudioSource.PlayClipAtPoint(gunshotSuppressed, gameObject.transform.position, 0.5f);
        }
        else
        {
            if (burst >= 5)
            {
                AudioSource.PlayClipAtPoint(gunshotHeavy, gameObject.transform.position, 2f/burst);
            }
            else if (damage >= 60)
            {
                AudioSource.PlayClipAtPoint(gunshotGeneric, gameObject.transform.position, 0.8f);
                AudioSource.PlayClipAtPoint(gunshotHeavy, gameObject.transform.position, 1.1f);
                
            }
            else if (damage >= 40)
            {
                AudioSource.PlayClipAtPoint(gunshotGeneric, gameObject.transform.position, 0.8f);
                
            }
            else if (damage >= 30)
            {
                AudioSource.PlayClipAtPoint(gunshotGeneric, gameObject.transform.position, 0.45f);
            }
            else
            {
                AudioSource.PlayClipAtPoint(gunshotGeneric, gameObject.transform.position, 0.25f);
            }
        }
        
        RaycastHit hit = new RaycastHit();
        Vector3 finalShot = new Vector3(0f, 0f, 0f);
        finalShot = camera.forward;
        
        //Debug.Log(gameObject.GetComponent<CharacterController>().velocity.x);
        if (isADS && gameObject.GetComponent<PlayerMovement>().isGrounded && !(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")))
        {
            finalShot[0] += UnityEngine.Random.Range(-curSpread - recoilSpread / 2, curSpread + recoilSpread / 2);
            finalShot[1] += UnityEngine.Random.Range(-curSpread - recoilSpread / 2, curSpread + recoilSpread / 2);
            finalShot[2] += UnityEngine.Random.Range(-curSpread - recoilSpread / 2, curSpread + recoilSpread / 2);
        }
        else
        {
            finalShot[0] += UnityEngine.Random.Range((curSpread + recoilSpread) * -1, (curSpread + recoilSpread));
            finalShot[1] += UnityEngine.Random.Range((curSpread + recoilSpread) * -1, (curSpread + recoilSpread));
            finalShot[2] += UnityEngine.Random.Range((curSpread + recoilSpread) * -1, (curSpread + recoilSpread));
        }
        //Debug.Log(finalShot);
        if (Physics.Raycast(camera.position, finalShot, out hit, 10000, canBeShot))
        {
            //Debug.Log("hit");
            /*
            Vector3 finalShot = new Vector3(0f,0f,0f);
            finalShot= hit.point;
            if (Input.GetMouseButton(1))
            {
                finalShot[0] += UnityEngine.Random.Range(ADSSpread * -1, ADSSpread);
                finalShot[1] += UnityEngine.Random.Range(ADSSpread * -1, ADSSpread);
                finalShot[2] += UnityEngine.Random.Range(ADSSpread * -1, ADSSpread);
            }
            else
            {
                finalShot[0] += UnityEngine.Random.Range(spread * -1, spread);
                finalShot[1] += UnityEngine.Random.Range(spread * -1, spread);
                finalShot[2] += UnityEngine.Random.Range(spread * -1, spread);
            }
            */
            if (isADS)
            {
                LineRenderer tracer = Instantiate(tracerObject, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<LineRenderer>();
                tracer.transform.parent = GameObject.Find("PlayerPos").transform;
                tracer.useWorldSpace = true;
                tracer.SetPosition(0, muzzle.position);
                //Debug.Log(camera.forward);
                tracer.SetPosition(1, hit.point);
                tracer.enabled = true;
                Destroy(tracer.gameObject, 0.1f);
                
            }
            else
            {
                LineRenderer tracer = Instantiate(tracerObject, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<LineRenderer>();
                tracer.transform.parent = GameObject.Find("PlayerPos").transform;
                tracer.useWorldSpace = true;
                tracer.SetPosition(0, curGun.gameObject.transform.Find("muzzle").position);
                //Debug.Log(camera.forward);
                tracer.SetPosition(1, hit.point);
                tracer.enabled = true;
                Destroy(tracer.gameObject, 0.1f);
            }

            //anim.SetTrigger("shoot");
            //anim.SetTrigger("finishShoot");
            //transform.RotateAround(camera.position, Vector3.forward, -10);
            if (hit.transform.gameObject.layer == 9)
            {
                GameObject t_newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(hit.point + hit.normal);
                t_newHole.transform.parent = hit.collider.gameObject.transform;
                Destroy(t_newHole, 8f);
            }
            

            /*bulletDistance = Vector3.Distance(t_newHole.transform.position, barrel.transform.position);
            Debug.Log("Distance " + bulletDistance.ToString());
            Debug.Log(Mathf.Max(loadout[currentIndex].damage - Mathf.Max(0, bulletDistance - loadout[currentIndex].optimaldistance) * loadout[currentIndex].damagedropoff, loadout[currentIndex].damage * loadout[currentIndex].mindamage));*/
            


            
            if (hit.collider.gameObject.layer == 8)
            {
                damageReduced = calcDamageFalloff(getDistance(camera.position, hit.collider.gameObject.transform.position), false);
                Debug.Log(Mathf.RoundToInt(getDistance(camera.position, hit.collider.gameObject.transform.position))+" "+(damage-damageReduced)+" "+damageFalloff);
                
                /*
                int nextEmpty = findNextEmpty();
                if (nextEmpty<11)
                {
                    splatterPrefabs[nextEmpty] = Instantiate(splatter, hit.point, Quaternion.identity);
                }
                */
                Instantiate(splatter, hit.point, Quaternion.identity);

                //Destroy(blood, 0.1f);

                //splatterPrefabs[nextEmpty].transform.parent = hit.collider.gameObject.transform;
                //Destroy(splatterPrefabs[nextEmpty], 0.5f);
                //score += 1;
                //scoreText.text = score.ToString();
                if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "bot")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<targetAI>().health -= (damage-damageReduced);
                }
                if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "gunner")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<GunnerAI>().health -= (damage - damageReduced);
                }
                else if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "brute")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<bruteAI>().health -= (damage-damageReduced);
                }
                //Debug.Log(hit.collider.gameObject);
                //Destroy(hit.collider.gameObject);
                hitmarkBody.SetActive(true);
                bodyMarkCD = 0.2f;
            }
            else if (hit.collider.gameObject.layer == 11)
            {
                damageReduced = calcDamageFalloff(getDistance(camera.position, hit.collider.gameObject.transform.position), true);
                Debug.Log(Mathf.RoundToInt(getDistance(camera.position, hit.collider.gameObject.transform.position)) + " " + (HSdamage - damageReduced) + " " + damageFalloff);
                /*
                int nextEmpty = findNextEmpty();
                if (nextEmpty < 11)
                {
                    splatterPrefabs[nextEmpty] = Instantiate(splatter, hit.point, Quaternion.identity);
                }
                */
                //splatterPrefabs[nextEmpty].transform.parent = hit.collider.gameObject.transform;
                //Destroy(splatterPrefabs[nextEmpty], 0.5f);
                //score += 1;
                //scoreText.text = score.ToString();
                Instantiate(splatter, hit.point, Quaternion.identity);

                //Destroy(blood, 0.5f);

                if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "bot")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<targetAI>().health -= (HSdamage-damageReduced);
                }
                if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "gunner")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<GunnerAI>().health -= (HSdamage - damageReduced);
                }
                else if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "brute")
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<bruteAI>().health -= (HSdamage-damageReduced);
                }

                //Debug.Log(hit.collider.gameObject);
                //Destroy(hit.collider.gameObject);
                hitmarkHead.SetActive(true);
                headMarkCD = 0.2f;
            }

            if (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 11)
            {
                GameObject curHB = Instantiate(EnemyHealthBar, EnemyHBAnchor.transform) as GameObject;
                if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "brute")
                {
                    curHB.GetComponent<Slider>().value = (hit.collider.gameObject.transform.parent.parent.GetComponent<bruteAI>().health) / (hit.collider.gameObject.transform.parent.parent.GetComponent<bruteAI>().maxHealth);
                }
                else if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "bot")
                {
                    curHB.GetComponent<Slider>().value = (hit.collider.gameObject.transform.parent.parent.GetComponent<targetAI>().health) / (hit.collider.gameObject.transform.parent.parent.GetComponent<targetAI>().maxHealth);
                }
                else if (hit.collider.gameObject.transform.parent.parent.gameObject.tag == "gunner")
                {
                    curHB.GetComponent<Slider>().value = (hit.collider.gameObject.transform.parent.parent.GetComponent<GunnerAI>().health) / (hit.collider.gameObject.transform.parent.parent.GetComponent<GunnerAI>().maxHealth);
                }

                Destroy(curHB, 0.5f);
            }

            canShoot = false;
            
            
            recoilUp = UnityEngine.Random.Range(recoilPower, recoilPower + 0.5f);
            totalRecoil += recoilUp;
            if (totalRecoil > recoilHeight)
            {
                totalRecoil = recoilHeight+0.5f;
            }
        }
    }
    public int findNextEmpty()
    {
        int counter = 0;
        while (true)
        {
            if (splatterPrefabs[counter]==null || counter>=11)
            {
                return counter;
            }
            else
            {
                counter += 1;
            }
        }
    }

    public void changeWeapon(int ID, int mag, int reserve)
    {

        if (!inBoltAction && ID>=0)
        {
            WeaponSelected = ID;
            Destroy(curGun);
            firingRate = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[ID];
            fireCooldown = firingRate;
            recoilPower = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoil[ID];
            zoom = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().scopeZoom[ID];
            spread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().spread[ID];
            ADSSpread = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ADSSpread[ID];
            canADS = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().canADS[ID];
            damage = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().bodyDamage[ID];
            HSdamage = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().headDamage[ID];
            burst = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().burstType[ID];
            hRecoil = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().hRecoil[ID];
            burstShot = burst;
            hasAction = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().hasBolt[ID];
            gunName = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().name[ID];
            magCount = mag;
            ammoCount = reserve;
            recoilRecovery = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoilRecovery[ID];
            swapSpeed = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().swapSpeed[ID];
            canSlamfire = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().slamfire[ID];
            optimalRange = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().optimalRange[ID];
            damageFalloff = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().damageFalloff[ID];
            minimumDamage = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().minimumDamage[ID];
            reloadSpeed = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().reloadSpeed[ID];
            noise = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().noise[ID];
            recoilHeight = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().recoilHeight[ID];
            isPistol = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().isPistol[ID];
            curSpread = spread;
            fireCooldown = 0;

            


            GameObject.Find("PlayerPos").GetComponent<PlayerMovement>().speed = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().speed[ID] * 12f;
            weaponModel = modelList[ID];
            curGun = Instantiate(modelList[ID], GunAnchor.transform) as GameObject;
            //curGun.transform.position = GunAnchor.transform.position;
            curGun.transform.position = GunAnchor.transform.position;
            //origPos = curGun.transform.position;
            curAnim = curGun.GetComponent<Animator>();
            //curGun.transform.position -= new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[ID], 0);
            curGun.transform.position -= new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[ID] + 1.5f, 0);
            showGun();
            curGun.transform.rotation = GunAnchor.transform.rotation;
            Quaternion basicRotation = curGun.transform.rotation;
            swapTime = swapSpeed;

            
        }
        
    }

    public void setWeapon(int ID)
    {
        
        if (!inBoltAction)
        {
            int slot = curSlot;
            bool switchTo = true;
            
            if (gunIDs[0] == -1)
            {
                slot = 0;
                //switchTo = false;
            }
            if (gunIDs[1] == -1)
            {
                slot = 1;
                //switchTo = false;
            }
            mags[slot] = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[ID];
            reserves[slot] = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ammo[ID];
            gunIDs[slot] = ID;
            Debug.Log("slot");
            changeWeapon(ID, mags[slot], reserves[slot]);
            if (switchTo)
            {
                
            }

        }
        

    }
    public void setWeapon(int slot, int ID)
    {
        
        if (!inBoltAction)
        {
            mags[slot] = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[ID];
            reserves[slot] = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ammo[ID];
            gunIDs[slot] = ID;
            //changeWeapon(ID, mags[slot], reserves[slot]);

        }

    }
    public void reload()
    {
        
        int magSize = GameObject.Find("PlayerPos").GetComponent<WeaponStats>().mag[WeaponSelected];
        int ammoLeft = magCount;
        //ammoCount += ammoLeft;
        //magCount = 0;
        
        
        if (ammoCount >= magSize - ammoLeft)
        {
            magCount += magSize - ammoLeft;
            ammoCount -= magSize - ammoLeft;
        }
        else
        {
            magCount += ammoCount;
            ammoCount = 0;
        }
        
        
    }
    public void runBolt()
    {
        curGunBolt = Instantiate(animList[WeaponSelected], GunAnchor.transform) as GameObject;
        curGunBolt.transform.position = curGun.transform.position;
        curGunBolt.transform.rotation = curGun.transform.rotation;

        curGunBolt.transform.position -= new Vector3(0, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().yOffset[WeaponSelected], 0);
        curGunBolt.GetComponent<Animator>().SetTrigger("startAction");
        curGunBolt.GetComponent<Animator>().SetTrigger("endAction");
        if (canSlamfire==1)
        {
            Destroy(curGunBolt, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected]/3 + 0.02f);
        }
        else if(burst>0 && burst<5)
        {
            Destroy(curGunBolt, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected]*(burst+4) + 0.02f);
        }
        else
        {
            Destroy(curGunBolt, GameObject.Find("PlayerPos").GetComponent<WeaponStats>().ROF[WeaponSelected] + 0.02f);
        }
        
    }
    public void runLerp()
    {
        endLerpTime = firingRate*0.4f;
        isLerping = true;
    }

    public void hideGun()
    {
        //Debug.Log("hide");
        curGun.SetActive(false);
        /*
        foreach (Transform child in curGun.transform)
        {
            if (child.name != "muzzle" && child.name != "sight"  && child.name != "Armature" && child.name != "Armature.001" && child.name != "Armature.002")
            {
                child.gameObject.GetComponent<Renderer>().enabled = false;
                foreach (Transform c in child.transform)
                {
                    if (c.name != "muzzle" && c.name != "sight"  && c.name != "Armature" && c.name != "Armature.001" && child.name != "Armature.002")
                    {
                        c.gameObject.GetComponent<Renderer>().enabled = false;
                        foreach (Transform c2 in c.transform)
                        {
                            if (c2.name != "muzzle" && c2.name != "sight"  && c2.name != "Armature" && c2.name != "Armature.001" && child.name != "Armature.002")
                            {
                                c2.gameObject.GetComponent<Renderer>().enabled = false;
                            }
                        }
                    }
                }
            }
        }
        if (WeaponSelected == 21)
        {
            Transform t = curGun.transform.Find("Armature.001").transform.Find("Cube");
            foreach (Transform c2 in t.transform)
            {
                if (c2.name != "muzzle" && c2.name != "sight"  && c2.name != "Armature" && c2.name != "Armature.001")
                {
                    c2.gameObject.GetComponent<Renderer>().enabled = false;
                }
            }
            t.gameObject.GetComponent<Renderer>().enabled = false;
        }
        */
    }
    public void showGun()
    {
        curGun.SetActive(true);
        /*

        foreach (Transform child in curGun.transform)
        {
            if (child.name != "muzzle" && child.name != "sight"  && child.name != "Armature" && child.name != "Armature.001" && child.name != "Armature.002")
            {
                child.gameObject.GetComponent<Renderer>().enabled = true;
                foreach (Transform c in child.transform)
                {
                    if (c.name != "muzzle" && c.name != "sight"  && c.name != "Armature" && c.name != "Armature.001" && child.name != "Armature.002")
                    {
                        c.gameObject.GetComponent<Renderer>().enabled = true;
                        foreach (Transform c2 in c.transform)
                        {
                            if (c2.name != "muzzle" && c2.name != "sight"  && c2.name != "Armature" && c2.name != "Armature.001" && child.name != "Armature.002")
                            {
                                c2.gameObject.GetComponent<Renderer>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
        if (WeaponSelected == 21)
        {
            Transform t = curGun.transform.Find("Armature.001").transform.Find("Cube");
            foreach (Transform c2 in t.transform)
            {
                if (c2.name != "muzzle" && c2.name != "sight"  && c2.name != "Armature" && c2.name != "Armature.001")
                {
                    c2.gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
            t.gameObject.GetComponent<Renderer>().enabled = true;
        }
        */
    }
    public float getDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt((start[0] - end[0]) * (start[0] - end[0]) + (start[1] - end[1]) * (start[1] - end[1]) + (start[2] - end[2]) * (start[2] - end[2]));
    }
    float calcDamageFalloff(float d, bool isHS)
    {
        if (isHS)
        {
            return Mathf.Min(damageFalloff *(HSdamage/damage) * Mathf.Max(0, d - optimalRange), HSdamage*(1f-minimumDamage));
        }
        else
        {
            return Mathf.Min(damageFalloff * Mathf.Max(0, d - optimalRange), damage * (1f - minimumDamage));
        }
    }

    Transform FindChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans;
        }
        return null;

    }
}
