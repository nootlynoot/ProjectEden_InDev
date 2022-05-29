using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteCon : MonoBehaviour
{
    public PlayerCon PC;
    public SpriteRenderer CurrentArtwork;
    [Space(10)]
    public GameObject originalPos;
    public GameObject flippedPos;

    private void Update()
    {
        UpdatePrimarySprite();
        
    }

    void UpdatePrimarySprite()
    {
        switch (PC.PSC.Playertype)
        {
            #region("Military Weapon Sprite")
            case PlayerStatsClass.PlayerType.Military:
                if(GetComponentInParent<MilitaryPlayerCon>() != null)
                {
                    MilitaryPlayerCon MC = GetComponentInParent<MilitaryPlayerCon>();
                    if (MC.currentWeapon == 0)
                    {
                        CurrentArtwork.sprite = PC.PrimaryAttack.WeaponArtwork;
                    }
                    else if (MC.currentWeapon == 1)
                    {
                        CurrentArtwork.sprite = PC.SecondaryAttack.WeaponArtwork;
                    }
                    else if (MC.currentWeapon == 2)
                    {
                        CurrentArtwork.sprite = PC.UltimateAttack.WeaponArtwork;
                    }
                }
                break;
            #endregion
            #region("Religious Weapon Sprite")
            case PlayerStatsClass.PlayerType.Religious:
                ReligiousPlayerCon RC = GetComponentInParent<ReligiousPlayerCon>();
                if (RC.currentWeapon == 0)
                {
                    CurrentArtwork.sprite = PC.PrimaryAttack.WeaponArtwork;
                }
                else if (RC.currentWeapon == 1)
                {
                    CurrentArtwork.sprite = PC.SecondaryAttack.WeaponArtwork;
                }
                break;
            #endregion
            #region("outrider Weapon Sprite")
            case PlayerStatsClass.PlayerType.Outrider:
                OutriderPlayerCon OC = GetComponentInParent<OutriderPlayerCon>();
                    if (OC.currentWeapon == 0)
                    {
                        CurrentArtwork.sprite = PC.PrimaryAttack.WeaponArtwork;
                    }
                    if (OC.currentWeapon == 1)
                    {
                        CurrentArtwork.sprite = PC.SecondaryAttack.WeaponArtwork;
                    }
                    if (OC.currentWeapon == 2)
                    {
                        CurrentArtwork.sprite = PC.UltimateAttack.WeaponArtwork;
                    }
                    break;
            #endregion
        }

        //if (PC.PrimaryAttack == null)
        //{
        //    return;
        //}
        //else if()
        //{
        //    //CurrentArtwork.sprite = PC.PrimaryAttack.WeaponArtwork;
        //}
    }
}
