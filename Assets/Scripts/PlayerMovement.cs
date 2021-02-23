using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动参数")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15f;

    private float jumpTime;

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("环境监测")]
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;           //判断角色与地面的距离，小于这个距离即为在地面上
    public LayerMask groundLayer;
    private float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;            //判断角色与墙壁的距离，在一定距离之内可以悬挂墙壁
    public float reachOffset = 0.7f;             //判断头顶上没有墙壁但是头下有墙壁的射线


    public float xVelocity;

    //按键设置
    private bool jumpPressed;
    private bool jumpHeld;
    private bool crouchHeld;
    private bool crouchPressed;

    //碰撞体尺寸
    Vector2 colliderStandSize;         //站立尺寸
    Vector2 colliderStandOffset;       //站立坐标
    Vector2 colliderCrouchSize;        //下蹲尺寸
    Vector2 colliderCrouchOffset;      //下蹲坐标 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;

        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }

    
    void Update()
    {
        if (GameManager.GameOver())
            return;
        if(Input.GetButtonDown("Jump"))
            jumpPressed = true;
        if(Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");


    }

    private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            xVelocity = 0;
            rb.velocity = new Vector2(0, 0);
            return;
        }
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    private void PhysicsCheck()
    {
        //判断双脚脚底碰撞
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f),Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f),Vector2.down, groundDistance, groundLayer);

        if (leftCheck|| rightCheck)
            isOnGround = true;
        else
            isOnGround = false;
        //判断头顶碰撞
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if (headCheck)
            isHeadBlocked = true;
        else
            isHeadBlocked = false;
        //判断悬挂射线左右方向
        float direction = transform.localScale.x;
        Vector2 graDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if(!isOnGround &&rb.velocity.y<0f && ledgeCheck && wallCheck &&!blockedCheck)
        {
            Vector3 pos= transform.position;
            pos.x +=( wallCheck.distance -0.05f)* direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
    // 地面移动代码
    void GroundMovement()
    {
        if (isHanging)
            return;
        //蹲下站立
        if (crouchHeld && !isCrouch && isOnGround)
            Crouch();
        else if (!crouchHeld && isCrouch&&!isHeadBlocked )
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        //左右移动
        xVelocity = Input.GetAxis("Horizontal");    //介于-1f~1f的浮点型
        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        FilpDirction();
    }

    //角色移动左右翻转
    void FilpDirction()
    {
        if(xVelocity > 0)
            transform .localScale =new Vector3 (1,1,1);
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1,1);
    }

    //跳跃
    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
                jumpPressed = false;
            }
            if(crouchPressed )
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                isHanging = false;
                crouchPressed = false;
            }
        }
        else if(jumpPressed &&isOnGround &&!isJump && !isHeadBlocked)
        {

            if (isCrouch )
            {
                StandUp();
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;
            jumpTime = Time.time + jumpHoldDuration;
            rb.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse );
            AudioManager.PlayJumpAudio();

        }
        else if(isJump)
        {
            if (jumpHeld)
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            if (jumpTime < Time.time)
            {
                isJump = false;
                jumpPressed = false;
            }
        }
    }

    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()
    {
        isCrouch = false;
        crouchPressed = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    RaycastHit2D Raycast (Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green; 
        Debug.DrawRay(pos + offset, rayDiraction* length,color );
        return hit;
    }
}
