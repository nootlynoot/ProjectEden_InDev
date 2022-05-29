using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rewired;

public class ReligiousPlayerCon : MonoBehaviour
{
    [Header("For Rewired")]
    Player player;
    public PlayerCon PC;
    PlayerMovement PM;
    public WeaponController WC;
    MilitaryPool MP;
    [Header("Dodge Roll")]
    public bool canDash = true;
    public float dashDuration = 0.2f;
    public float dashSpeed = 15f;
    public float dashJumpIncrease = 0f;
    public float dashCD = 0.3f;
    [Header("Weapons Selection")]
    public int currentWeapon = 0;
    bool shoot;
    [Header("CastingInputs")]
    public string upText;
    public string downText;
    public string leftText;
    public string rightText;
    [Space(10)]
    public string firstCast;
    public string secondCast;
    public string thirdCast;
    bool castingUp;
    bool castingDown;
    bool castingLeft;
    bool castingRight;
    bool firstCastCheck;
    bool secondCastCheck;
    bool thirdCastCheck;
    [Header("Chain Lightning")]
    public GameObject lightning;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    Collider2D[] targetInRadius;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask targetMask;
    public List<Transform> visibleTargets = new List<Transform>();
    protected List<Transform> targets = new List<Transform>();
    public Transform lightningTarget;
    [Header("Glacial Shard")]
    bool casted;
    public Transform[] ShardSpawn;
    [SerializeField] protected List<Transform> shards = new List<Transform>();
    int shardshot = 0;
    [Header("Beam")]
    public int healValue;
    public int DamageValue;
    [Header("Gimmick")]
    public float karmaMeter;
    public enum karmaStates { cardinal, halfCardinal, bishop, halfBishop, priest, halfPriest, neutral, halfResearcher, researcher, halfTheorist, theorist, halfScientist, scientist }
    public karmaStates karmaState = karmaStates.neutral;
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
        getinputs();
        CastingInputs();
        KarmaMeter();
        FindVisiblePlayer();
    }
    private void FixedUpdate()
    {
        if (WC.isBeaming)
        {
            if (WC.dirX + WC.dirY != 0)
            {
                WC.RotateAll();
            }
        }

        if (shoot)
        {
            GetClosestEnemy();
            Shoot();
        }
    }

    void getinputs()
    {
        shoot = player.GetButtonDown("Shoot");
        if (!WC.isBeaming)
        {
            castingDown = player.GetNegativeButtonDown("CastingY");
            castingUp = player.GetButtonDown("CastingY");
            castingLeft = player.GetNegativeButtonDown("CastingX");
            castingRight = player.GetButtonDown("CastingX");
        }
    }

    void KarmaMeter()
    {
        karmaMeter = Mathf.Clamp(karmaMeter, -30, 30);
        #region("Good Karma")
        if (karmaMeter >= 0)
        {
            karmaState = karmaStates.neutral;
            
            if (karmaMeter >= 5)
            {
                karmaState = karmaStates.halfPriest;

                if (karmaMeter >= 10)
                {
                    karmaState = karmaStates.priest;

                    if (karmaMeter >= 15)
                    {
                        karmaState = karmaStates.halfBishop;

                        if (karmaMeter >= 20)
                        {
                            karmaState = karmaStates.bishop;

                            if (karmaMeter >= 25)
                            {
                                karmaState = karmaStates.halfCardinal;

                                if (karmaMeter >= 30)
                                {
                                    karmaState = karmaStates.cardinal;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region("Bad Karma")
        if (karmaMeter <= 0)
        {
            karmaState = karmaStates.neutral;

            if (karmaMeter <= -5)
            {
                karmaState = karmaStates.halfResearcher;

                if (karmaMeter <= -10)
                {
                    karmaState = karmaStates.researcher;

                    if (karmaMeter <= -15)
                    {
                        karmaState = karmaStates.halfTheorist;

                        if (karmaMeter <= -20)
                        {
                            karmaState = karmaStates.theorist;

                            if (karmaMeter <= -25)
                            {
                                karmaState = karmaStates.halfScientist;

                                if (karmaMeter <= -30)
                                {
                                    karmaState = karmaStates.scientist;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        switch (karmaState)
        {
            case karmaStates.cardinal:

                break;
            case karmaStates.halfCardinal:

                break;
            case karmaStates.bishop:

                break;
            case karmaStates.halfBishop:

                break;
            case karmaStates.priest:

                break;
            case karmaStates.halfPriest:

                break;
            case karmaStates.neutral:

                break;
            case karmaStates.halfResearcher:

                break;
            case karmaStates.researcher:

                break;
            case karmaStates.halfTheorist:

                break;
            case karmaStates.theorist:

                break;
            case karmaStates.halfScientist:

                break;
            case karmaStates.scientist:

                break;
        }
    }

    void CastingInputs()
    {
        if (castingDown)
        {
            if (!firstCastCheck)
            {
                firstCastCheck = true;
                firstCast = downText;
                return;
            }
            if (firstCastCheck && !secondCastCheck)
            {
                secondCastCheck = true;
                secondCast = downText;
                return;
            }
            if (secondCastCheck && !thirdCastCheck)
            {
                thirdCastCheck = true;
                thirdCast = downText;
                return;
            }
            if (thirdCastCheck)
            {
                firstCastCheck = false;
                firstCast = "";
                secondCastCheck = false;
                secondCast = "";
                thirdCastCheck = false;
                thirdCast = "";
                return;
            }
        }
        if (castingLeft)
        {
            if (!firstCastCheck)
            {
                firstCastCheck = true;
                firstCast = leftText;
                return;
            }
            if (firstCastCheck && !secondCastCheck)
            {
                secondCastCheck = true;
                secondCast = leftText;
                return;
            }
            if (secondCastCheck && !thirdCastCheck)
            {
                thirdCastCheck = true;
                thirdCast = leftText;
                return;
            }
            if (thirdCastCheck)
            {
                firstCastCheck = false;
                firstCast = "";
                secondCastCheck = false;
                secondCast = "";
                thirdCastCheck = false;
                thirdCast = "";
                return;
            }
        }
        if (castingRight)
        {
            if (!firstCastCheck)
            {
                firstCastCheck = true;
                firstCast = rightText;
                return;
            }
            if (firstCastCheck && !secondCastCheck)
            {
                secondCastCheck = true;
                secondCast = rightText;
                return;
            }
            if (secondCastCheck && !thirdCastCheck)
            {
                thirdCastCheck = true;
                thirdCast = rightText;
                return;
            }
            if (thirdCastCheck)
            {
                firstCastCheck = false;
                firstCast = "";
                secondCastCheck = false;
                secondCast = "";
                thirdCastCheck = false;
                thirdCast = "";
                return;
            }
        }
        if (castingUp)
        {
            if (!firstCastCheck)
            {
                firstCastCheck = true;
                firstCast = upText;
                return;
            }
            if (firstCastCheck && !secondCastCheck)
            {
                secondCastCheck = true;
                secondCast = upText;
                return;
            }
            if (secondCastCheck && !thirdCastCheck)
            {
                thirdCastCheck = true;
                thirdCast = upText;
                return;
            }
            if (thirdCastCheck)
            {
                firstCastCheck = false;
                firstCast = "";
                secondCastCheck = false;
                secondCast = "";
                thirdCastCheck = false;
                thirdCast = "";
                return;
            }
        }
    }

    void Shoot()
    {
        if (visibleTargets.Count > 0)
        {
            if (firstCast == upText && secondCast == downText && thirdCast == leftText)
            {
                Instantiate(lightning, gameObject.transform.position, Quaternion.identity);
            }

            if(shardshot < 5)
            {
                GlacialShard shardS = shards[shardshot].gameObject.GetComponent<GlacialShard>();
                if (casted && shardS.ShardState == GlacialShard.ShardsState.standby)
                {
                    shardS.ShardState = GlacialShard.ShardsState.fire;
                    shards[shardshot].right = lightningTarget.position - shards[shardshot].position;
                    shardS.speed = 10;
                    shardS.deducted = true;
                    PC.HP -= shardS.shieldValue;
                    shardshot += 1;
                }
            }
        }
        if (firstCast == upText && secondCast == downText && thirdCast == rightText)
        {
            if (!casted)
            {
                glacialShard();
            }
            if (casted)
            {
                foreach (Transform shard in shards)
                {
                    if (!shard.gameObject.activeInHierarchy)
                    {
                        glacialShard();
                    }
                }
            }
        }
        if (firstCast == upText && secondCast == downText && thirdCast == downText && !WC.isBeaming)
        {
            StartCoroutine(Beam());
        }

        resetCast();
        return;
    }

    void glacialShard()
    {
        casted = true;
        shardshot = 0;
        foreach (Transform shard in shards)
        {
            shard.gameObject.SetActive(true);
            GlacialShard shardS = shard.gameObject.GetComponent<GlacialShard>();
            shard.position = gameObject.transform.position;
            shardS.ShardState = GlacialShard.ShardsState.spawned;
            PC.HP += shardS.shieldValue;
            shardS.deducted = false;
        }
    }

    void GetClosestEnemy()
    {
        targets.Clear();
        targets.AddRange(visibleTargets);
        if(targets.Count > 0)
        {
            var nearestDist = float.MaxValue;
            foreach (var target in targets)
            {
                if (Vector3.Distance(gameObject.transform.position, target.position) < nearestDist)
                {
                    nearestDist = Vector3.Distance(gameObject.transform.position, target.position);
                    lightningTarget = target;
                }
            }
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

                    if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask) && Physics2D.Raycast(transform.position, dirTarget, distanceTarget, targetMask))
                    {
                        if (visibleTargets.Count < Mathf.Infinity)
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

                    if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask) && Physics2D.Raycast(transform.position, dirTarget, distanceTarget, targetMask))
                    {
                        if (visibleTargets.Count < Mathf.Infinity)
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

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if (!global)
        {
            angleDeg += transform.eulerAngles.z + 90;
        }
        return new Vector2(Mathf.Sin(angleDeg * Mathf.Deg2Rad), Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, viewRadius);
        Vector3 viewangleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewangleB = DirFromAngle(viewAngle / 2, false);
        Handles.DrawLine(transform.position, transform.position + viewangleA * viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewangleB * viewRadius);
    }

    void resetCast()
    {
        firstCastCheck = false;
        firstCast = "";
        secondCastCheck = false;
        secondCast = "";
        thirdCastCheck = false;
        thirdCast = "";
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
        yield return new WaitForSeconds(3);
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
