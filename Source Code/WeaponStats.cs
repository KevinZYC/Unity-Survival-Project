using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float[] bodyDamage = {32f, 21f, 87f, 31f, 25f, 140f, 27f, 27f, 20f, 182f, 32f, 21f, 27f, 14f, 32f, 9f, 21f, 101f, 88f, 205f, 45f, 20f, 24f, 16f, 55f, 21f, 24f, 155f, 18f, 32f, 80f};
    public float[] headDamage = {132f, 61f, 191f, 110f, 52f, 220f, 96f, 96f, 39f, 240f, 75f, 67f, 96f, 48f, 99f, 18f, 82f, 202f, 175f, 240f, 140f, 60f, 99f, 55f, 140f, 60f, 99f, 210f, 36f, 132f, 99f};
    public int[] burstType    = {1, 1, 1, 1, 12, 1, 3, 1, 7, 0, 1, 1, 1, 1, 1, 24, 1, 0, 0, 0, 0, 0, 1, 1, 1, 2, 0, 1, 9, 1, 1};
    public float[] ROF        = {0.1f, 0.067f, 0.22f, 0.085f, 0.6f, 0.35f, 0.05f, 0.075f, 0.2f, 1.2f, 0.11f, 0.045f, 0.085f, 0.055f, 0.11f, 0.3f, 0.09f, 0.75f, 0.45f, 1.2f, 0.25f, 0.125f, 0.06f, 0.085f, 0.55f, 0.05f, 0.125f, 0.3f, 0.9f, 0.085f, 1f};
    public int[] mag          = {30, 40, 12, 30, 6, 2, 30, 100, 8, 5, 25, 32, 30, 24, 47, 2, 30, 6, 6, 3, 7, 15, 120, 18, 6, 36, 21, 8, 4, 30, 999};
    public int[] ammo         = {60, 120, 24, 60, 12, 10, 60, 100, 24, 10, 75, 128, 72, 80, 141, 10, 60, 18, 24, 6, 28, 300, 120, 54, 18, 72, 63, 16, 12, 60, 999};
    public float[] recoil     = {4f, 2f, 5f, 2.5f, 8f, 11f, 2f, 2f, 0.3f, 3f, 0.5f, 1.5f, 2.25f, 2.2f, 3.5f, 0.5f, 2.5f, 2.5f, 2.5f, 7.5f, 7f, 3.5f, 1.75f, 2.75f, 7f, 1.5f, 4f, 5.5f, 2f, 3f, 0f};
    public float[] speed      = {0.75f, 0.9f, 0.75f, 0.8f, 0.75f, 0.75f, 0.8f, 0.7f, 0.7f, 0.75f, 0.75f, 0.95f, 0.8f, 0.95f, 0.7f, 0.8f, 0.8f, 0.85f, 0.9f, 0.7f, 0.9f, 1f, 0.7f, 0.95f, 0.9f, 0.95f, 1f, 0.6f, 0.8f, 0.75f, 1.2f};
    public float[] spread     = {0.005f, 0.01f, 0.02f, 0f, 0.05f, 0.01f, 0.004f, 0.0125f, 0.06f, 0.1f, 0.0125f, 0.0125f, 0.004f, 0.02f, 0.04f, 0.1f, 0.007f, 0.01f, 0.01f, 0.1f, 0.007f, 0.01f, 0.02f, 0.01f, 0.003f, 0.008f, 0.01f, 0.07f, 0.07f, 0.005f, 0f};
    public bool[]      canADS = {false, false, true, true, true, true, false, true, false, true, false, true, false, false, false, false, false, true, false, true, false, false, false, false, false, false, false, true, false, false, false};
    public float[] ADSSpread  = {0.0025f, 0.007f, 0f, 0f, 0.05f, 0.006f, 0.003f, 0.003f, 0.06f, 0f, 0.01f, 0.005f, 0.001f, 0.015f, 0.03f, 0.1f, 0.007f, 0f, 0f, 0f, 0.007f, 0.004f, 0.01f, 0.01f, 0.05f, 0.008f, 0.01f, 0f, 0.07f, 0.001f, 0f};
    public float[] scopeZoom  = {1f, 1f, 5f, 3f, 4f, 2f, 2.5f, 2.5f, 1f, 8f, 1f, 2f, 1f, 1f, 1f, 1f, 1f, 5f, 5f, 8f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 8f, 1f, 1f, 1f}; //do NOT use 0f for any of them
    public int[] rewardBasic  = {4, 7, 2, 5, 2, 2, 6, 6, 2, 1, 5, 9, 6, 9, 5, 2, 5, 2, 3, 1, 3, 10, 6, 6, 3, 8, 6, 2, 2, 4, 10};
    public int[] rewardArmed  = {5, 9, 3, 7, 2, 2, 8, 8, 3, 1, 6, 12, 8, 12, 6, 3, 6, 3, 3, 1, 4, 15, 8, 8, 4, 11, 8, 2, 3, 5, 15};
    public int[] rewardBrute  = {7, 11, 4, 9, 3, 2, 10, 10, 4, 2, 8, 15, 10, 15, 8, 4, 8, 3, 4, 2, 5, 20, 10, 10, 5, 14, 10, 3, 4, 7, 20};
    public string[] name = { "AK-47", "MP7", "MK-20", "AUG A3", "Buckshot", "Slugshot", "M16", "M249", "Auto-Shotgun", "Arctic 300", "UMP-45", "Vector .45", "M4A1", "MAC-10", "DP-28", "Birdshot", "T95 PARA", "M24", "Hunting Rifle", "Barrett .50", "Desert Eagle", "Glock", "MG3-H", "CZ75-Auto", ".44 Revolver", "Dual Assassins", "Tec-9", "Arctic .500 Auto", "Sawed-Off", "AK-109", "Combat Knife" };
    public float[] yOffset = { 0, 0.5f, 0, 0.25f, 0, 0, -0.25f, 0, 0, 0f, 0, 0, 0f, 0, 0.15f, 0, 0, 0, 0, 0, -0.1f, 0, 0f, 0, 0, 0, 0, 0, 0, 0, 0};
    public float[] hRecoil = {3f, 1.5f, 3f, 3f, 3f, 3f, 3f, 1.2f, 3f, 3f, 3f, 1.5f, 3f, 3f, 0.5f, 3f, 3f, 3f, 3f, 3f, 3f, 2f, 0.8f, 0.8f, 1f, 1.75f, 4f, 3f, 3f, 2f, 0f};
    public float[] sprayAccuracy = {2f, 2f, 0.5f, 1.5f, 0.4f, 1f, 1.75f, 2.5f, 0.4f, 1f, 0.15f, 2f, 1f, 1.5f, -1f, 1f, 1f, 1f, 1f, 1f, 0.75f, 1f, -2f, 0.8f, 0.8f, 1f, 0.7f, 0.5f, 1f, 3f, 2f};
    public int[] hasBolt = { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1};
    public float[] recoilRecovery = { 1f, 1f, 0.9f, 1f, 0.5f, 1f, 1f, 1f, 0.8f, 1f, 0.3f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1.2f, 1f, 0.6f, 0.75f, 1f, 1f, 0.5f, 1.2f, 1f, 1f };
    public float[] swapSpeed = { 0.75f, 0.75f, 1f, 1f, 1f, 0.75f, 0.75f, 1.2f, 1.2f, 1f, 0.75f, 0.75f, 0.75f, 0.75f, 1.2f, 0.75f, 0.75f, 1f, 0.75f, 1.2f, 0.3f, 0.2f, 1.2f, 0.3f, 0.6f, 0.35f, 0.3f, 1.2f, 0.5f, 0.75f, 0.3f };
    public int[] slamfire = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0};
    public int[] isPistol = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0};
    public float[] optimalRange = {20f, 12f, 120f, 40f, 5f, 12f, 12f, 15f, 5f, 240f, 12f, 12f, 20f, 12f, 15f, 5f, 20f, 180f, 180f, 240f, 20f, 5f, 15f, 5f, 5f, 30f, 20f, 240f, 8f, 25f, 3f };
    public float[] damageFalloff = {0.3f, 0.5f, 0f, 0.2f, 0.7f, 0.3f, 0.2f, 0.25f, 0.5f, 0f, 0.5f, 0.4f, 0.3f, 0.5f, 0.325f, 0.5f, 0.3f, 0f, 0f, 0f, 0.4f, 0.4f, 0.35f, 0.4f, 0.4f, 0.4f, 0.3f, 0f, 0.7f, 0.3f, 100f };
    public float[] minimumDamage = { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0f };
    public float[] noise = {2f, 1f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 1f, 1f, 0f, 1f, 2f, 2f, 2f, 0f, 2f, 2f, 2f, 1f, 2f, 1f, 2f, 0f, 1f, 2f, 2f, 1f, 0f};
    public float[] recoilHeight = {20f, 12f, 12f, 12f, 12f, 12f, 18f, 16f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 17f, 12f, 20f, 13f, 10f, 16f, 12f, 24f, 24f};
    public float[] reloadSpeed = { 3f, 2f, 3f, 3f, 3.5f, 2f, 2.5f, 5f, 3f, 3.5f, 2f, 2f, 2.5f, 2f, 4f, 2f, 2.5f, 2.5f, 2.5f, 2.5f, 3f, 1.5f, 5f, 3f, 2.5f, 3.5f, 2f, 4f, 2.5f, 3f, 0.1f };
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
