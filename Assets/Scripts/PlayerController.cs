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

    [Header("Shoot Attributes")]
    private short arms;

    [Header("Ground Check Attributes")]
    public GameObject groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGrounded;

    private Animator animator;
    private short dir;
    private short lastDir;
    private enum State { idle, walk, jump_ready, jump_air, charge, fire };
    private State state;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumped = false;
        arms = 2;
        dir = 0;
        lastDir = 1;
        state = State.idle;
    }
    
    void FixedUpdate()
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
    }

    void Move()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, 0);

        if (isGrounded)
        {
            if (movement.x < 0)
            {
                dir = -1;
                lastDir = -1;
                state = State.walk;
            }
            if (movement.x > 0)
            {
                dir = 1;
                lastDir = 1;
                state = State.walk;
            }
            if (movement.x == 0)
            {
                dir = 0;
                state = State.idle;
            }
        }
        
        rigidBody.transform.Translate(movement);
    }

    void Jump()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !jumped)
            {
                state = State.jump_ready;
                jumped = true;
                Invoke("MakeJump", 0.15f);
            }
        }
    }

    void MakeJump()
    {
        state = State.jump_air;
        rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        jumped = false;
    }

    void Shoot()
    {
        if (isGrounded && Input.GetKey(KeyCode.L))
        {
            state = State.charge;
        }
        if (isGrounded && Input.GetKeyUp(KeyCode.L))
        {
            state = State.fire;
            if (arms != 0) arms--;
        }
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
                if (dir > 0)
                {
                    if (arms == 2) animator.Play("Walk_Right_1");
                    if (arms == 1) animator.Play("Walk_Right_2");
                    if (arms == 0) animator.Play("Walk_Right_3");
                }
                if (dir < 0)
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
                    if (arms == 0) animator.Play("Shoot_Right_Fire_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Shoot_Left_Fire_1");
                    if (arms == 1) animator.Play("Shoot_Left_Fire_2");
                    if (arms == 0) animator.Play("Shoot_Left_Fire_3");
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
    }
}
