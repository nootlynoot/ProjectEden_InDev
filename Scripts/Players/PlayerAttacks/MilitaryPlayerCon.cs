using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rewired;

public class MilitaryPlayerCon : MonoBehaviour
{
    [Header("For Rewired")]
    Player player;
    WeaponController WC;
    PlayerCon PC;
    PlayerMovement PM;
    MilitaryPool MP;
    float shootStart1 = 0f;
    float shootStart2 = 0f;
    float shootStart3 = 0f;
    [Header("Weapons Selection")]
    public GameObject rocket;
    public GameObject nuke;
    public int currentWeapon = 0;
    public int selectedWeapon = 0;
    bool nextWeapon;
    bool shoot;
    bool ultimate;
    bool prevWeapon;
    bool conWeapon;
    [Header("Ammo")]
    public int AmmoCount = 200;
    public bool invince;
    [Header("Dodge Roll")]
    [Space(10)]
    public bool canDash = true;
    public float dashDuration = 0.2f;
    public float dashSpeed = 15f;
    public float dashJumpIncrease = 0f;
    public float dashCD = 0.3f;
    [Header("Homing Rocket")]
    [Space(10)]
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    Collider2D[] targetInRadius;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask targetMask;
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> storedTargets = new List<Transform>();
    private void Awake()
    {
        WC = GetComponentInChildren<WeaponController>();
        PC = GetComponent<PlayerCon>();
        PM = GetComponent<PlayerMovement>();
        player = ReInput.players.GetPlayer(PM.playerId);
        MP = MilitaryPool.instance;
    }

    private void Update()
    {
        getinputs();
        weaponSelection();
    }

    private void FixedUpdate()
    {
        FindVisiblePlayer();
        if (WC.dirX + WC.dirY != 0)
        {
            WC.RotateAll();
        }

        if (PC.PrimaryAttack != null && currentWeapon == 0 && shoot)
        {
            ShootPri();
        } else if (currentWeapon == 1 && shoot)
        {
            ShootSec();
        }else if (currentWeapon == 2 && visibleTargets.Count > 0 && shoot){
            ShootUlt();
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
        if(conWeapon){
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
        } else if (selectedWeapon < 0)
        {
            selectedWeapon = 2;
        }
        if(ultimate){
            float screenX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, 0)).x;
            float screenY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;
            Instantiate(nuke,new Vector2(screenX,screenY),Quaternion.identity);
        }
    }
    void ShootPri()
    {
        if (Time.time > shootStart1 + PC.PrimaryAttack.Cooldown && AmmoCount > 0)
        {
            MilitaryPool.instance.SpawnFromPool("Lazer Blaster", PC.WP.transform.position, PC.WP.transform.rotation);
            AmmoCount -= PC.PrimaryAttack.Cost;
            shootStart1 = Time.time;
        }
    }
    void ShootSec()
    {
        if (Time.time > shootStart2 + PC.SecondaryAttack.Cooldown && AmmoCount > 0)
        {
            MilitaryPool.instance.SpawnFromPool("Gatling Gun", PC.WP.transform.position, PC.WP.transform.rotation);
            AmmoCount -= PC.SecondaryAttack.Cost;
            shootStart2 = Time.time;
        }
    }

    void ShootUlt()
    {
        if (Time.time > shootStart3 + PC.UltimateAttack.Cooldown && AmmoCount > 0){
            print("shot rockets");
            storedTargets.Clear();
            storedTargets.AddRange(visibleTargets);
            StartCoroutine(ShootRocket());
            shootStart3 = Time.time;
        }
    }

    public void SPM()
    {
        if (canDash)
        {
            StartCoroutine(DashCo());
        }
    }

    void FindVisiblePlayer()
    {
        targetInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        visibleTargets.Clear();

        for (int i = 0; i < targetInRadius.Length; i++)
        {
            Transform target = targetInRadius[i].transform;
            Vector2 dirTarget = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            if (!PM.sprite.flipX)
            {
                if (Vector2.Angle(dirTarget, transform.right) < viewAngle / 2)
                {
                    float distanceTarget = Vector2.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask))
                    {
                        if(visibleTargets.Count < 7)
                        {
                            visibleTargets.Add(target);
                            if (visibleTargets.Contains(gameObject.transform))
                            {
                                visibleTargets.Remove(gameObject.transform);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Vector2.Angle(-dirTarget, transform.right) < viewAngle / 2)
                {
                    float distanceTarget = Vector2.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask))
                    {
                        if (visibleTargets.Count < 7)
                        {
                            visibleTargets.Add(target);
                            if (visibleTargets.Contains(gameObject.transform))
                            {
                                visibleTargets.Remove(gameObject.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, viewRadius);
        Vector3 viewangleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewangleB = DirFromAngle(viewAngle / 2, false);
        Handles.DrawLine(transform.position, transform.position + viewangleA * viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewangleB * viewRadius);
    }

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if (!global)
        {
            angleDeg += transform.eulerAngles.z + 90;
        }
        return new Vector2(Mathf.Sin(angleDeg * Mathf.Deg2Rad), Mathf.Cos(angleDeg * Mathf.Deg2Rad));
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

    IEnumerator ShootRocket(){
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(rocket,PC.WP.transform.position,Quaternion.identity);
            AmmoCount -= PC.UltimateAttack.Cost;
    }
}