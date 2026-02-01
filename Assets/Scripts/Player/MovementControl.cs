using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControl : MonoBehaviour
{
    public static MovementControl Singleton;

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
    [SerializeField] float dashTime = 2f, dashCooldown = 2f;
    float jumpBufferCounter = 0f;
    [SerializeField] float dashForce = 5.0f;
    [SerializeField] float riseGravity = 1.0f;
    [SerializeField] float fallGravity = 1.5f;

    public Animator Localanimator;
    Rigidbody2D rb2d;
    public float LJoyHReadValue { get => lJoyHReadValue; }
    public float LJoyVReadValue { get => lJoyVReadValue; }
    [SerializeField] private float lJoyHReadValue, lJoyVReadValue;

    public bool IsGrounded => isGrounded;
    public bool isGrounded, canDash;
    private bool isDashing;
    public bool IsDashing { get => isDashing; }
    public int jumpCounter = 0, maxJumps = 1;
    [SerializeField] float coyoteCounter, coyoteTimer;
    [SerializeField] BoxGroundController boxGroundController;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] EdgeGrabbing edgeGrabbing;
    [SerializeField] CinemachinePositionComposer cameraPositionComposer;
    float defaultCameraYOffset;
    float defaultCameraXOffset;
    [SerializeField] float durationEdgeClimbing = 0.25f;
    Vector3 beforeEdgeClimbPosition;
    Timer edgeGrabTimer;

    private float defaultPlayerGravity;

    public Action<Vector2> OnMoved;

    [SerializeField] bool stillPressingJump = false;
    [SerializeField] float stillPressingGravityScaler = 0.5f;
    [SerializeField] bool shouldEdgeGrab = false, isEdgeClimbing = false;
    [SerializeField] float lastEdgeGrabTime = -1, edgeGrabAgainTime = 1f;
    [SerializeField] float walkSoundDistance;
    [SerializeField] Vector2 lastStepSound;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip dashClip, jumpClip, grabClip, landClip, walkClip1,walkClip2;
    bool walkOneOrTwo;

    void Start()
    {
        Singleton = this;

        DefaultMovementSpeed = MovementSpeed;

        rb2d = GetComponent<Rigidbody2D>();
        defaultPlayerGravity = rb2d.gravityScale;
        coyoteTimer = 0.2f;
        trailRenderer.time = dashTime;
        edgeGrabTimer = new Timer(durationEdgeClimbing, false,true);
        //stepOnTimer = new Timer(0.02f, true);

        defaultCameraYOffset = cameraPositionComposer.TargetOffset.y;
        defaultCameraXOffset = cameraPositionComposer.TargetOffset.x;

        boxGroundController.OnNewLand += LandSound;

        lastStepSound = transform.position;
    }

    void LandSound()
    {
        PlayAudioWithRandomPitch(landClip, volume: 0.25f);
    }

    void Update()
    {
        if (shouldEdgeGrab)
        {
            if(isEdgeClimbing)
            {
                if(edgeGrabTimer.Tick(Time.deltaTime))
                {
                    //rb2d.linearVelocity = Vector2.zero;
                    //rb2d.gravityScale = 0f;
                    //isEdgeClimbing = true;
                    //yield return new WaitForSeconds(0.25f);
                    transform.position = edgeGrabbing.standPoint;
                    isEdgeClimbing = false;
                    shouldEdgeGrab = false;
                    Localanimator.SetBool("bIsGrabbing", isEdgeClimbing);
                    rb2d.linearVelocity = Vector2.zero;
                    rb2d.gravityScale = 0f;
                    edgeGrabTimer.Reset();
                    return;
                }

                Vector2 toLerpPos;

                if (edgeGrabTimer.GetCounter() > edgeGrabTimer.GetTimer() * 0.5f)
                    toLerpPos = new Vector2(transform.position.x, edgeGrabbing.standPoint.y);
                else
                    toLerpPos = edgeGrabbing.standPoint;

                float lerpValue = edgeGrabTimer.GetCounter() / edgeGrabTimer.GetTimer();

                transform.position = Vector3.Lerp(toLerpPos, beforeEdgeClimbPosition, lerpValue);
            }   
            
            return;
        }

        isGrounded = boxGroundController.IsGrounded;

        if (isGrounded)
        {
            coyoteCounter = coyoteTimer;
            jumpCounter = 0;
        }
        else
        {
            if (edgeGrabbing.Hitting && !shouldEdgeGrab && Time.time - lastEdgeGrabTime > edgeGrabAgainTime)
            {
                lastEdgeGrabTime = Time.time;

                shouldEdgeGrab = true;
                rb2d.linearVelocity = Vector2.zero;
                rb2d.gravityScale = 0f;
                PlayAudioWithRandomPitch(grabClip,volume:0.25f);
                //StartCoroutine(TeleportToEdge());
                return;
            }

            coyoteCounter -= Time.deltaTime;
        }

        if (isDashing)
            return;

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

        if(IsGrounded && Vector2.Distance(lastStepSound,transform.position) > walkSoundDistance)
        {
            walkOneOrTwo = !walkOneOrTwo;
            PlayAudioWithRandomPitch(walkOneOrTwo ? walkClip1 : walkClip2);
            lastStepSound = transform.position;
        }
    }

    void PlayAudioWithRandomPitch(AudioClip clip, float minRange = 0.8f, float maxRange = 1.1f, float volume = 0.5f)
    {
        source.pitch = UnityEngine.Random.Range(minRange, maxRange);
        source.volume = volume;
        source.PlayOneShot(clip);
    }

    private void FixedUpdate()
    {
        if (shouldEdgeGrab)
            return;

        if (!isDashing)
        {
            if (rb2d.linearVelocity.y < 0)
            {
                rb2d.gravityScale = defaultPlayerGravity * fallGravity;

            }
            else
            {
                rb2d.gravityScale = defaultPlayerGravity * riseGravity;

                if (stillPressingJump)
                {
                    rb2d.gravityScale *= stillPressingGravityScaler;
                }
            }
        }

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

        if (shouldEdgeGrab && ctx.performed)
        {
            bool dir = Localanimator.GetBool("FlipX") ? false : true;

            if (dir && lJoyHReadValue > 0 || !dir && lJoyHReadValue < 0)
            {
                beforeEdgeClimbPosition = transform.position;
                isEdgeClimbing = true;
                Localanimator.SetBool("bIsGrabbing", isEdgeClimbing);
                //StartCoroutine(TeleportToEdge());
            }
            else if (!isEdgeClimbing)
            {
                shouldEdgeGrab = false;
            }

        }

        if (ctx.performed)
        {
            cameraPositionComposer.TargetOffset.x = lJoyHReadValue > 0 ? defaultCameraXOffset : -defaultCameraXOffset;
        }

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

        return yVelocity;
    }

    public void VMovementPlayerInput(InputAction.CallbackContext ctx)
    {
        lJoyVReadValue = ctx.ReadValue<float>();

        if (ctx.performed)
        {
            cameraPositionComposer.TargetOffset.y = lJoyVReadValue > 0 ? defaultCameraYOffset * 1.5f : -defaultCameraYOffset;
        }
        else if(ctx.canceled)
        {
            cameraPositionComposer.TargetOffset.y = defaultCameraYOffset;
        }

    }
    private void Jump()
    {
        //if (rb2d.linearVelocity.y > 0.01f)
        //    return;

        if (jumpBufferCounter > 0)
        {
            if (maxJumps == 1 && coyoteCounter < 0)
                return;

            jumpCounter++;

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

            PlayAudioWithRandomPitch(jumpClip,0.25f,0.5f,0.15f);

        }
    }
    public void JumpAction(InputAction.CallbackContext ctx)
    {
        stillPressingJump = !ctx.canceled;

        if (shouldEdgeGrab && ctx.performed && !isEdgeClimbing)
        {
            shouldEdgeGrab = false;
            jumpCounter = 0;
        }

        if (ctx.performed && jumpCounter < maxJumps)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    public void DashAction(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !canDash || isDashing)
            return;

        StartCoroutine(ContinueDashing());
        PlayAudioWithRandomPitch(dashClip, 0.6f, 0.9f, 0.5f);
    }

    private IEnumerator ContinueDashing()
    {
        canDash = false;
        isDashing = true;
        float oldGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0;
        rb2d.linearVelocity = Vector2.right * (Localanimator.GetBool("FlipX") ? -dashForce : dashForce);
        trailRenderer.emitting = true;
        Localanimator.SetBool("bIsDashing", true);
        yield return new WaitForSeconds(dashTime);
        Localanimator.SetBool("bIsDashing", false);
        trailRenderer.emitting = false;
        rb2d.gravityScale = oldGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator TeleportToEdge()
    {
        shouldEdgeGrab = true;
        //rb2d.linearVelocity = Vector2.zero;
        //rb2d.gravityScale = 0f;
        isEdgeClimbing = true;
        yield return new WaitForSeconds(0.25f);
        transform.position = edgeGrabbing.standPoint;
        isEdgeClimbing = false;
        shouldEdgeGrab = false;
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