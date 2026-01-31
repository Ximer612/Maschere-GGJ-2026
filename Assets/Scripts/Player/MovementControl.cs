using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovementControl : MonoBehaviour
{
    [SerializeField] PlayerInput pInput;
    public float DefaultMovementSpeed { get; private set; }
    public float MovementSpeed;
    [SerializeField] float timeToAccelerate = 0.2f;
    [SerializeField] float timeToDecelerate = 0.1f;
    Vector2 currentVelocity;

    float acceleration => MovementSpeed / timeToAccelerate;
    float deceleration => MovementSpeed / timeToDecelerate;

    public float JumpForce;
    [SerializeField] float jumpBufferTime = 0.05f;
    float jumpBufferCounter = 0f;
    [SerializeField] float riseGravity = 1.0f;
    [SerializeField] float fallGravity = 1.5f;

    public Animator Localanimator;
    Rigidbody2D rb2d;
    public float LJoyHReadValue { get => lJoyHReadValue; }
    public float LJoyVReadValue { get => lJoyVReadValue; }
    [SerializeField] private float lJoyHReadValue, lJoyVReadValue;

    public bool IsGrounded => isGrounded;
    [SerializeField] bool isGrounded, canClimb, isClimbing, isDropping;
    //[SerializeField] int jumpCounter = 0, maxJumps = 1;
    [SerializeField] float coyoteCounter, coyoteTimer;
    [SerializeField] BoxGroundController boxGroundController;

    private float defaultPlayerGravity;

    public Action<Vector2> OnMoved;

    void Start()
    {
        DefaultMovementSpeed = MovementSpeed;

        rb2d = GetComponent<Rigidbody2D>();
        defaultPlayerGravity = rb2d.gravityScale;
        coyoteTimer = 0.2f;

        //stepOnTimer = new Timer(0.02f, true);
    }

    void Update()
    {
        isGrounded = boxGroundController.IsGrounded;

        if (isGrounded)
        {
            coyoteCounter = coyoteTimer;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        ProcessHMovement();
        //ProcessVMovement();

        if (pInput.enabled)
        {
            float TargetHVel = MovementSpeed * lJoyHReadValue;
            float accelRate = (Mathf.Abs(TargetHVel) > Mathf.Abs(currentVelocity.x)) ? acceleration : deceleration;

            float newXVel = Mathf.MoveTowards(currentVelocity.x, TargetHVel, accelRate * Time.deltaTime);
            currentVelocity = new Vector2(newXVel, GetYVelocity()); // no delta time? [ no I'm setting a velocity :) ]
            rb2d.linearVelocity = currentVelocity;
        }
        else
        {
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
        }

        //if(stepOnTimer.Tick(Time.deltaTime))
        //{
        //    StepOn();
        //}
    }

    private void FixedUpdate()
    {
        if (rb2d.linearVelocity.y < 0)
            rb2d.gravityScale = defaultPlayerGravity * fallGravity;
        else
            rb2d.gravityScale = defaultPlayerGravity * riseGravity;

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }

        Jump();

    }
    void ProcessHMovement()
    {
        if (Mathf.Abs(lJoyHReadValue) > 0.01)
        {
            if (Mathf.Abs(lJoyHReadValue) > 0.5)
            {

                lJoyHReadValue = 1 * Mathf.Sign(lJoyHReadValue);
            }

            Localanimator.SetFloat("HVelocity", 2);
            Localanimator.SetBool("FlipX", lJoyHReadValue < 0);
        }
        else
        {
            Localanimator.SetFloat("HVelocity", -1);
        }

        if (rb2d.linearVelocity.y < 1f)
        {
            Localanimator.SetBool("bIsLanded", isGrounded);
        }
    }

    public void HMovementPlayerInput(InputAction.CallbackContext ctx)
    {
        lJoyHReadValue = ctx.ReadValue<float>();
    }

    //void ProcessVMovement()
    //{
    //if (isClimbing)
    //{
    // Localanimator.SetFloat("VVelocity", LJoyVReadValue);
    //GameRPC.Singleton.SetAnimationFloatServerRPC(gameObject.GetComponent<NetworkObject>(), "VVelocity", LJoyVReadValue);
    //}

    //Localanimator.SetBool("bIsClimbing", isClimbing);
    //GameRPC.Singleton.SetAnimationBoolServerRPC(gameObject.GetComponent<NetworkObject>(), "bIsClimbing", isClimbing);
    //}

    float GetYVelocity()
    {
        float yVelocity = rb2d.linearVelocity.y;

        if (canClimb)
        {
            if (lJoyVReadValue != 0)
            {
                if (!isClimbing)
                {
                    isClimbing = true;
                    rb2d.gravityScale = 0f;
                }
                else
                {
                    yVelocity = MovementSpeed * lJoyVReadValue;
                }
            }
            else if (isClimbing)
            {
                yVelocity = 0f;
            }
        }

        return yVelocity;
    }

    public void VMovementPlayerInput(InputAction.CallbackContext ctx)
    {
        lJoyVReadValue = ctx.ReadValue<float>();
    }
    private void Jump()
    {
        if (!isClimbing && rb2d.linearVelocity.y > 0.01f)
            return;


        if ((coyoteCounter > 0 || isClimbing) && jumpBufferCounter > 0)
        {
            //if (isClimbing)
            //{
            //    NotClimbing();
            //}

            coyoteCounter = 0;

            Localanimator.SetBool("bIsLanded", false);
            Localanimator.SetTrigger("BJump");

            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0);
            rb2d.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            jumpBufferCounter = 0f;
        }
    }
    public void JumpAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (!netcodePlayer) return;

    //    if (collision.CompareTag("Ladder") && !(rb2d.linearVelocityY > 0))
    //    {
    //        canClimb = true;

    //    }

    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!netcodePlayer) return;

    //    if (collision.CompareTag("Ladder"))
    //    {
    //        NotClimbing();
    //    }
    //}

    //private void NotClimbing()
    //{
    //    canClimb = false;
    //    isClimbing = false;
    //    rb2d.gravityScale = defaultPlayerGravity;
    //}

    //private void StepOn()
    //{
    //    Vector2 dir = new Vector2(CharSprite.flipX ? -1 : 1, 0);
    //    Debug.DrawRay(stepOverRay.transform.position, dir * stepOnCheckDistance);

    //    if (!isGrounded || rb2d.linearVelocity == Vector2.zero || isDropping)
    //        return;

    //    RaycastHit2D[] results;
    //    results = Physics2D.RaycastAll(stepOverRay.transform.position, dir, stepOnCheckDistance, stepOnLayerMask);

    //    for (int i = 0; i < results.Length; i++)
    //    {
    //        if (!results[i].collider.isTrigger)
    //        {
    //            rb2d.position = results[i].point;
    //        }
    //    }

    //}
}