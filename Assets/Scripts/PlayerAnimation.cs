using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private  Animator anim;
    private PlayerMovement movement;
    private Rigidbody2D rb;

    private int groundID;
    private int hangingID;
    private int crouchID;
    private int speedID;
    private int fallID;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");
    }


    void Update()
    {
        //anim.SetBool("isOnGround", movement.isOnGround);
        anim.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        anim.SetBool(groundID, movement.isOnGround);
        anim.SetBool(hangingID, movement.isHanging );
        anim.SetBool(crouchID, movement.isCrouch );
        anim.SetFloat(fallID, rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }
}
