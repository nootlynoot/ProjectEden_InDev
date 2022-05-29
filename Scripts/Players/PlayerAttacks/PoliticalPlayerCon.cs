using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PoliticalPlayerCon : MonoBehaviour
{
    [Header("For Rewired")]
    Player player;
    PlayerCon PC;
    PlayerMovement PM;
    public WeaponController[] WC;
    public SummonCon[] SC;
    [Header("Dodge Roll")]
    [Space(10)]
    public bool canDash = true;
    public float dashDuration = 0.2f;
    public float dashSpeed = 15f;
    public float dashJumpIncrease = 0f;
    public float dashCD = 0.3f;
    [Header("Weapons Selection")]
    [Space(10)]
    int selectedWeapon = 0;
    public int currentWeapon = 0;
    public int costOfRiot;
    public int costOfSF;
    public int costOfBO;
    public GameObject[] summons;
    bool nextWeapon;
    [HideInInspector]
    public bool shoot;
    bool ultimate;
    bool conWeapon;
    bool prevWeapon;
    [Header("Money")]
    [Space(10)]
    [Range(0, 5)] public float money;
    float moneyRegenStart;
    public float moneyRegenRate;
    public float moneyRegenAmt;

    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerCon>();
        PM = GetComponent<PlayerMovement>();
        player = ReInput.players.GetPlayer(PM.playerId);
    }

    // Update is called once per frame
    void Update()
    {
        WC = GetComponentsInChildren<WeaponController>();
        SC = GetComponentsInChildren<SummonCon>();
        getinputs();
        moneyRegen();
        Summon();
        weaponSelection();
        for (int i = 0; i < WC.Length; i++)
        {
            if (WC[i].dirX + WC[i].dirY != 0)
            {
                WC[i].RotateAll();
            }
        }
    }

    void getinputs()
    {
        nextWeapon = player.GetButtonDown("Next Weapon");
        prevWeapon = player.GetButtonDown("Prev Weapon");
        shoot = player.GetButton("Shoot");
        ultimate = player.GetButtonDown("Ultimate");
        conWeapon = player.GetButtonDown("Confirm Selection");
    }

    void weaponSelection()
    {
        if (nextWeapon)
        {
            selectedWeapon += 1;
        }
        if (prevWeapon)
        {
            selectedWeapon -= 1;
        }
        if (selectedWeapon > 2)
        {
            selectedWeapon = 0;
        }
        else if (selectedWeapon < 0)
        {
            selectedWeapon = 2;
        }
    }

    void Summon()
    {
        if (conWeapon)
        {
            currentWeapon = selectedWeapon;
            switch (currentWeapon)
            {
                case 0:
                    print("summon Pistol");
                    if (money >= costOfRiot)
                    {
                        ResetSummons();
                        money -= costOfRiot;
                        PC.HP += 8;
                        summons[0].SetActive(true);
                        summons[1].SetActive(true);
                        summons[2].SetActive(true);
                        summons[3].SetActive(true);
                    }
                    break;
                case 1:
                    print("summon SF");
                    if (money >= costOfSF)
                    {
                        ResetSummons();
                        money -= costOfSF;
                        PC.HP += 6;
                        summons[0].SetActive(true);
                        summons[1].SetActive(true);
                        summons[4].SetActive(true);
                    }
                    break;
                case 2:
                    print("summon BO");
                    if (money >= costOfBO)
                    {
                        ResetSummons();
                        money -= costOfBO;
                        PC.HP += 4;
                        summons[0].SetActive(true);
                        summons[1].SetActive(true);
                    }
                    break;

            }
        }
    }

    void ResetSummons()
    {
        PC.HP = 1;
        for (int i = 0; i < summons.Length; i++)
        {
            summons[i].SetActive(false);
        }
    }

    void moneyRegen()
    {
        if (Time.time > moneyRegenStart + moneyRegenRate && money < 5)
        {
            money += moneyRegenAmt;
            moneyRegenStart = Time.time;
        }else if(money >= 5)
        {
            return;
        }
    }

    public void SPM()
    {
        if (canDash)
        {
            StartCoroutine(DashCo());
        }
    }

    IEnumerator DashCo()
    {
        canDash = false;
        //gameObject.tag = "Untagged";
        PM.moveSpeed = dashSpeed;
        PM.jumpForce = dashJumpIncrease;
        yield return new WaitForSeconds(dashDuration);
        PM.moveSpeed = PM.originalSpeed;
        PM.jumpForce = PM.originalJump;
        //gameObject.tag = "Player";
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}