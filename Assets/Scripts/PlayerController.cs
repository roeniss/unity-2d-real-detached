using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Attributes")]
    public float moveSpeed;
    public float jumpHeight;
    private bool isGrounded;
    private bool jumped;
    private Rigidbody2D rigidBody;
    private bool movable;

    [Header("Shoot Attributes")]
    private short arms;
    private short cnt;
    private bool retrieving;

    [Header("Ground Check Attributes")]
    public GameObject groundCheck;
    public float groundCheckRadius;
    private LayerMask whatIsGrounded;

    private Animator animator;
    private short dir;
    private short lastDir;
    private enum State { idle, walk, jump_ready, jump_air, charge, fire };
    private State state;
    private bool stateFixed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movable = true;
        jumped = false;

        arms = 2;
        cnt = 0;
        retrieving = false;

        whatIsGrounded = LayerMask.GetMask("Ground");

        dir = 0;
        lastDir = 1;
        state = State.idle;
        stateFixed = false;
    }
    
    void Update()
    {
        GroundCheck();
        Jump();
        Move();
        Shoot();
        AnimationControl();
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, whatIsGrounded);

        if (!isGrounded)
        {
            state = State.jump_air;
            jumped = false;
        }
        else
        {
            if (state == State.jump_ready || state == State.fire) stateFixed = true;
            else stateFixed = false;
        }
    }

    void Move()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, 0);

        if (movement.x < 0)
        {
            dir = -1;
            lastDir = -1;
            if (isGrounded)
            {
                if (!stateFixed) state = State.walk;
            }
        }
        if (movement.x > 0)
        {
            dir = 1;
            lastDir = 1;
            if (isGrounded)
            {
                if (!stateFixed) state = State.walk;
            }
        }
        if (movement.x == 0)
        {
            dir = 0;
            if (isGrounded)
            {
                if (!stateFixed) state = State.idle;
            }
        }
        
        if (movable) rigidBody.transform.Translate(movement);
    }

    void Jump()
    {
        if (isGrounded && movable)
        {
            if (Input.GetButtonDown("Jump") && !jumped)
            {
                // Jump start.
                state = State.jump_ready;
                // Wait until the jump_animation is finished.
                Invoke("MakeJump", 0.3f);
                // Player's state is fixed while jump_ready animation is playing.
                stateFixed = true;
                // Player has jumped.
                jumped = true;
            }
        }
    }

    void MakeJump()
    {
        rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    void Shoot()
    {
        if (isGrounded && !stateFixed)
        {
            // Charge
            if (Input.GetKey(KeyCode.L) && arms != 0)
            {
                if (!retrieving)
                {
                    // Charging start.
                    state = State.charge;
                    // Player can't move while charging.
                    movable = false;

                    if (cnt == 0) cnt++;
                }
            }
            // Fire
            if (Input.GetKeyUp(KeyCode.L) && arms != 0)
            {
                if (!retrieving)
                {
                    // Firing start.
                    state = State.fire;
                    // Wait for the fire animation to finish.
                    Invoke("MakeShoot", 0.3f);
                    //  player's state is fixed while the animation is playing
                    stateFixed = true;

                    if (cnt == 1) cnt++;
                }
            }
        }

        // Retreive
        if (Input.GetKeyDown(KeyCode.L) && arms == 0)
        {
            // Retreiving start.
            retrieving = true;
            // Restore arm number.
            arms = 2;

            cnt++;
        }
        // Retreiving finished. This has to be fixed when shooting mechanism is implemented.
        if (Input.GetKeyUp(KeyCode.L) && retrieving)
        {
            retrieving = false;
        }

        if (cnt == 3)
        {
            cnt = 0;
        }
    }

    void MakeShoot()
    {
        // Once firing is done, player is able to move, change state and an arm is reduced.
        movable = true;
        state = State.idle;
        arms--;
    }

    void AnimationControl()
    {
        switch (state)
        {
            case State.idle:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Idle_Right_1");
                    if (arms == 1) animator.Play("Idle_Right_2");
                    if (arms == 0) animator.Play("Idle_Right_3");

                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Idle_Left_1");
                    if (arms == 1) animator.Play("Idle_Left_2");
                    if (arms == 0) animator.Play("Idle_Left_3");
                }
                break;
            case State.walk:
                if (dir == 1)
                {
                    if (arms == 2) animator.Play("Walk_Right_1");
                    if (arms == 1) animator.Play("Walk_Right_2");
                    if (arms == 0) animator.Play("Walk_Right_3");
                }
                if (dir == -1)
                {
                    if (arms == 2) animator.Play("Walk_Left_1");
                    if (arms == 1) animator.Play("Walk_Left_2");
                    if (arms == 0) animator.Play("Walk_Left_3");
                }
                break;
            case State.jump_ready:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Jump_Right_Ready_1");
                    if (arms == 1) animator.Play("Jump_Right_Ready_2");
                    if (arms == 0) animator.Play("Jump_Right_Ready_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Jump_Left_Ready_1");
                    if (arms == 1) animator.Play("Jump_Left_Ready_2");
                    if (arms == 0) animator.Play("Jump_Left_Ready_3");
                }
                break;
            case State.jump_air:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Jump_Right_Air_1");
                    if (arms == 1) animator.Play("Jump_Right_Air_2");
                    if (arms == 0) animator.Play("Jump_Right_Air_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Jump_Left_Air_1");
                    if (arms == 1) animator.Play("Jump_Left_Air_2");
                    if (arms == 0) animator.Play("Jump_Left_Air_3");
                }
                break;
            case State.charge:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Shoot_Right_Charge_1");
                    if (arms == 1) animator.Play("Shoot_Right_Charge_2");
                    if (arms == 0) animator.Play("Shoot_Right_Charge_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Shoot_Left_Charge_1");
                    if (arms == 1) animator.Play("Shoot_Left_Charge_2");
                    if (arms == 0) animator.Play("Shoot_Left_Charge_3");
                }
                break;
            case State.fire:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Shoot_Right_Fire_1");
                    if (arms == 1) animator.Play("Shoot_Right_Fire_2");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Shoot_Left_Fire_1");
                    if (arms == 1) animator.Play("Shoot_Left_Fire_2");
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
    }
}
