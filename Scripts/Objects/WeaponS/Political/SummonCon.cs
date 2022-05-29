using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SummonCon : MonoBehaviour
{
    public string PoliticalPlayerName;
    PoliticalPlayerCon PC;
    PlayerMovement PM;
    public SpriteRenderer Mainsprite;
    SpriteRenderer Summonsprite;
    public GameObject WeaponHeld;
    WeaponController WC;
    WeaponSpriteCon WSC;
    [Space(10)]
    public OffensiveStatsClass RiotPistol;
    public OffensiveStatsClass SFRifle;
    public OffensiveStatsClass BOSniper;
    [Space(10)]
    float shootStart;
    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find(PoliticalPlayerName).GetComponent<PlayerMovement>();
        PC = GameObject.Find(PoliticalPlayerName).GetComponent <PoliticalPlayerCon>();
        WC = GetComponentInChildren<WeaponController>();
        WSC = GetComponentInChildren<WeaponSpriteCon>();
        Summonsprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Shooting();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
        changeWeaponSprite();
    }

    void UpdateSprite()
    {
        if (Mainsprite.flipX)
        {
            Summonsprite.flipX = true;
            WeaponHeld.transform.position = WSC.flippedPos.transform.position;
        }
        else
        {
            Summonsprite.flipX = false;
            WeaponHeld.transform.position = WSC.originalPos.transform.position;
        }
        //not the reason why summon gun doesnt switch poses
        if (PM.dirX > 0f)
        {
            if (WC.dirX + WC.dirY == 0)
            {
                WC.RotateL();
            }
        }
        else if (PM.dirX < 0f)
        {
            if (WC.dirX + WC.dirY == 0)
            {
                WC.RotateR();
            }
        }
    }

    void Shooting()
    {
        switch (PC.currentWeapon)
        {
            case 0:
                shootPri();
                break;
            case 1:
                shootSec();
                break;
            case 2:
                shootUlti();
                break;
        }
    }

    public void changeWeaponSprite()
    {
        switch (PC.currentWeapon)
        {
            case 0:
                WSC.CurrentArtwork.sprite = RiotPistol.WeaponArtwork;
                break;
            case 1:
                WSC.CurrentArtwork.sprite = SFRifle.WeaponArtwork;
                break;
            case 2:
                WSC.CurrentArtwork.sprite = BOSniper.WeaponArtwork;
                break;
        }
    }

    void shootPri()
    {
        if (Time.time > shootStart + RiotPistol.Cooldown)
        {
            if (PC.shoot)
            {
                MilitaryPool.instance.SpawnFromPool("Riot Pistol", WeaponHeld.transform.position, WeaponHeld.transform.rotation);
                shootStart = Time.time;
            }
        }
    }

    void shootSec()
    {
        if (Time.time > shootStart + SFRifle.Cooldown)
        {
            if (PC.shoot)
            {
                MilitaryPool.instance.SpawnFromPool("SF Rifle", WeaponHeld.transform.position, WeaponHeld.transform.rotation);
                shootStart = Time.time;
            }
        }
    }

    void shootUlti()
    {
        if (Time.time > shootStart + BOSniper.Cooldown)
        {
            if (PC.shoot)
            {
                MilitaryPool.instance.SpawnFromPool("BO Sniper", WeaponHeld.transform.position, WeaponHeld.transform.rotation);
                shootStart = Time.time;
            }
        }
    }
}
