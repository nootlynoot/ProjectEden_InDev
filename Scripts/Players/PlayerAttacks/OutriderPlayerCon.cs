using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class OutriderPlayerCon : MonoBehaviour
{
    Player player;
    bool nextWeapon;
    bool shoot;
    bool ultimate;
    bool prevWeapon;
    bool conWeapon;
    public PlayerCon PC;
    float shootStart = 0f;
    PlayerMovement PM;
    public WeaponController WC;
    public GameObject weaponPoint;
    MilitaryPool MP;
    [Header("Dodge Roll")]
    [Space(10)]
    public bool canDash = true;
    public float dashDuration = 0.2f;
    public float dashSpeed = 15f;
    public float dashJumpIncrease = 0f;
    public float dashCD = 0.3f;
    public int currentWeapon = 0;
    public int selectedWeapon = 0;
    [Header("Gimmick")]
    public float heatMeter;
    public float heatedAtkIncrease;
    public float heatedSpdIncrease;
    [Header("PulseRifle")]
    public PulseCannon PulseCan;
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<PlayerMovement>();
        PC = GetComponent<PlayerCon>();
        WC = GetComponentInChildren<WeaponController>();
        player = ReInput.players.GetPlayer(PM.playerId);
        MP = MilitaryPool.instance;
    }

    // Update is called once per frame
    void Update()
    {
        heatMeter = Mathf.Clamp(heatMeter, 0, 100);
        getinputs();
        selectWeapon();
        heat();
    }

    private void FixedUpdate()
    {
        if (WC.dirX + WC.dirY != 0)
        {
            heatMeter += 0.1f;
            WC.RotateAll();
            if (PC.PrimaryAttack != null && currentWeapon == 0 && shoot)
            {
                ShootPri();
            }
            else if (currentWeapon == 1 && shoot)
            {
                ShootSec();
            }
            else if (currentWeapon == 2 && shoot)
            {
                ShootUlti();
            }
        }
        else if (heatMeter != 0)
        {
            heatMeter -= 0.2f;
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

    void selectWeapon()
    {
        if (conWeapon)
        {
            currentWeapon = selectedWeapon;
        }

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
    void ShootPri()
    {
        if (Time.time > shootStart + (PC.PrimaryAttack.Cooldown - (heatedAtkIncrease)))
        {
            MilitaryPool.instance.SpawnFromPool("Sword", PC.WP.transform.position, PC.WP.transform.rotation);

            shootStart = Time.time;
        }
    }
    void ShootSec()
    {
        if (Time.time > shootStart + (PC.SecondaryAttack.Cooldown - (heatedAtkIncrease)))
        {
            MilitaryPool.instance.SpawnFromPool("Hand Cannon", PC.WP.transform.position, PC.WP.transform.rotation);

            shootStart = Time.time;
        }
    }

    void ShootUlti()
    {
        if (Time.time > shootStart + (PC.SecondaryAttack.Cooldown - (heatedAtkIncrease)))
        {
            PulseCan.shotpointPos.transform.position = weaponPoint.transform.position;
            PulseCan.shotpointend.transform.position = PulseCan.LaserHit.position;

            StartCoroutine(Beam());

            shootStart = Time.time;
        }
    }

    void heat()
    {
        if(heatMeter >= 20)
        {
            heatedAtkIncrease = PC.PrimaryAttack.Cooldown / 5;
            heatedSpdIncrease = PM.moveSpeed / 5;

            if(heatMeter >= 40)
            {
                heatedAtkIncrease = PC.PrimaryAttack.Cooldown / 4;
                heatedSpdIncrease = PM.moveSpeed / 4;

                if (heatMeter >= 60)
                {
                    heatedAtkIncrease = PC.PrimaryAttack.Cooldown / 3;
                    heatedSpdIncrease = PM.moveSpeed / 3;

                    if (heatMeter >= 80)
                    {
                        heatedAtkIncrease = PC.PrimaryAttack.Cooldown / 2;
                        heatedSpdIncrease = PM.moveSpeed / 2;
                    }
                }
            }
        }
        else
        {
            heatedSpdIncrease = 0;
            heatedAtkIncrease = 0;
        }
    }

    public void SPM()
    {
        if (canDash)
        {
            StartCoroutine(DashCo());
        }
    }
    IEnumerator Beam()
    {
        WC.isBeaming = true;
        yield return new WaitForSeconds(0.5f);
        WC.isBeaming = false;
    }

    IEnumerator DashCo()
    {
        canDash = false;
        gameObject.tag = "Untagged";
        PM.moveSpeed = dashSpeed;
        PM.jumpForce = dashJumpIncrease;
        yield return new WaitForSeconds(dashDuration);
        PM.moveSpeed = PM.originalSpeed;
        PM.jumpForce = PM.originalJump;
        gameObject.tag = "Player";
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
