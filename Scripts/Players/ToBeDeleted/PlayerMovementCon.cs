using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCon : MonoBehaviour
{
    public PlayerCon PC;
    public Rigidbody2D rb;
    public CapsuleCollider2D coll;
    public SpriteRenderer sprite;
    [Space(10)]
    [SerializeField] private LayerMask jumpableGround;
    [Space(10)]
    private float dirX = 0f;
    private float dirY = 0f;
    float moveSpeed;
    [SerializeField] private float jumpForce = 14f;
    bool axisInUse;
    bool canDoubleJump;
    public enum MovementState { idle, running, jumping, falling }
    public MovementState MoveState = MovementState.idle;
    [Space(10)]
    [Header("Dash Variables")]
    public bool canDash = true;
    public float dashDuration;
    public float dashSpeed;
    public float dashJumpIncrease;
    public float dashCD;
    [Space(10)]
    [Header("Weapon Sprite")]
    public GameObject WeaponHeld;
    WeaponController WC;
    float originalSpeed;
    float originalJump;

    private void Awake()
    {
        WC = GetComponentInChildren<WeaponController>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        moveSpeed = PC.PSC.PlayerSpeed;
        originalSpeed = moveSpeed;
        originalJump = jumpForce;
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw(PC.PMI.HorizontalMovementAxis);
        dirY = Input.GetAxisRaw(PC.PMI.VerticalMovementAxis);
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        jumpingInput();
        if (Input.GetButtonDown(PC.PMI.SpecialMovementButton) && canDash)
        {
            print("dash");
            Dash();
        }
        UpdateAnimationState();
    }

    void jumpingInput()
    {
        if (Input.GetAxisRaw(PC.PMI.JumpButton) == 1)
        {
            if (!axisInUse)
            {
                //do thing
                jumping();
                print("jumped");
            }
            axisInUse = true;
        }
        else
        {
            axisInUse = false;
        }


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

    private void UpdateAnimationState()
    {

        if (dirX > 0f)
        {
            MoveState = MovementState.running;
            if(WC.dirX + WC.dirY == 0)
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

    void Dash()
    {
        StartCoroutine(DashCo());
    }

    IEnumerator DashCo()
    {
        canDash = false;
        moveSpeed = dashSpeed;
        jumpForce = dashJumpIncrease;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = originalSpeed;
        jumpForce = originalJump;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
