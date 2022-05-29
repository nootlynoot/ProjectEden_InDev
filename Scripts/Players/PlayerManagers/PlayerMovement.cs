using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    [Header("For Rewired")]
    public int playerId;
    Player player;
    [Header("Essentials")]
    public bool isPolitical;
    public PlayerCon PC;
    public Rigidbody2D rb;
    public CapsuleCollider2D coll;
    public SpriteRenderer sprite;
    [Space(10)]
    [Header("Movement")]
    bool jump;
    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D friction;
    [SerializeField] private LayerMask jumpableGround;
    public float jumpForce = 14f;
    [HideInInspector]
    public float moveSpeed;
    bool canDoubleJump;
    [HideInInspector]
    public float dirX;
    [HideInInspector]
    public float dirY;
    [Space(10)]
    [Header("Climbing")]
    bool interact;
    public float distance;
    bool isClimbing;
    public LayerMask whatIsLadder;
    [Space(10)]
    [Header("SpecialMovement")]
    bool SPM;
    public enum MovementState { idle, running, jumping, falling }
    public MovementState MoveState = MovementState.idle;
    [Header("Weapon Sprite")]
    public GameObject WeaponHeld;
    public WeaponController WC;
    [HideInInspector]
    public float originalSpeed;
    [HideInInspector]
    public float originalJump;

    private void Awake()
    {
        if (!isPolitical)
        {
            WC = GetComponentInChildren<WeaponController>();
        }
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = PC.PSC.PlayerSpeed;
        originalSpeed = moveSpeed;
        originalJump = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        processInput();
        UpdateAnimationState();
        changeFriction();
    }

    void changeFriction()
    {
        if (IsGrounded())
        {
            rb.sharedMaterial = friction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

    void getInput()
    {
        dirX = player.GetAxisRaw("Move Horizontal");
        dirY = player.GetAxisRaw("Move Vertical");
        jump = player.GetButtonDown("Jump");
        SPM = player.GetButtonDown("Movement Passive");
        interact = player.GetButtonDown("Interact");
    }

    void processInput()
    {
        if(dirX != 0f)
        {
            if(PC.PSC.Playertype != PlayerStatsClass.PlayerType.Outrider)
            {
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            }
            else
            {
                OutriderPlayerCon OC = GetComponent<OutriderPlayerCon>();
                rb.velocity = new Vector2(dirX * (moveSpeed + (OC.heatedSpdIncrease)) , rb.velocity.y);
            }
        }
        Climbing();
        if (jump)
        {
            jumping();
        }
        if (SPM)
        {
            switch (PC.PSC.Playertype)
            {
                case PlayerStatsClass.PlayerType.Military:
                    MilitaryPlayerCon MC = GetComponent<MilitaryPlayerCon>();
                    MC.SPM();
                    break;
                case PlayerStatsClass.PlayerType.Political:
                    PoliticalPlayerCon PC = GetComponent<PoliticalPlayerCon>();
                    PC.SPM();
                    break;
                case PlayerStatsClass.PlayerType.Religious:
                    ReligiousPlayerCon RC = GetComponent<ReligiousPlayerCon>();
                    RC.SPM();
                    break;
                case PlayerStatsClass.PlayerType.Outrider:
                    OutriderPlayerCon OC = GetComponent<OutriderPlayerCon>();
                    OC.SPM();
                    break;
            }
        }
    }

    void Climbing()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (hitInfo.collider != null)
        {
            if (interact)
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, dirY * originalSpeed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 3;
        }
    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            MoveState = MovementState.running;
            if (WC.dirX + WC.dirY == 0)
            {
                flipRight();
                WC.RotateL();
            }
        }
        else if (dirX < 0f)
        {
            MoveState = MovementState.running;
            if (WC.dirX + WC.dirY == 0)
            {
                flipLeft();
                WC.RotateR();
            }
        }
        else
        {
            MoveState = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            MoveState = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            MoveState = MovementState.falling;
        }
    }
    public void flipRight()
    {
        sprite.flipX = false;
        WeaponSpriteCon WC = WeaponHeld.GetComponent<WeaponSpriteCon>();
        WeaponHeld.transform.position = WC.originalPos.transform.position;
    }

    public void flipLeft()
    {
        sprite.flipX = true;
        WeaponSpriteCon WC = WeaponHeld.GetComponent<WeaponSpriteCon>();
        WeaponHeld.transform.position = WC.flippedPos.transform.position;
    }

    void jumping()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
