using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public Camera camera;
    public float retrieveSpeed;
    public float moveSpeed;
    public float retreiveRadius;
    private short dir;
    private short lastDir;

    private float gravityScale;
    private float mass;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private Vector2 playerPosition;
    private enum handState { idle, moving };
    private handState state;
    private bool controlling;
    private bool retrieveComplete;

    void Start()
    {
        gameObject.SetActive(false);
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        dir = 1;
        lastDir = 1;
        gravityScale = rigidbody.gravityScale;
        mass = rigidbody.mass;
        playerPosition = player.transform.position;
        state = handState.idle;
        controlling = false;
        retrieveComplete = true;
    }

    void Update()
    {
        if (!controlling)
        {
            Retrieve();
        }
        else
        {
            Move();
        }
        AnimationControl();
    }

    public void Fire(float power)
    {
        // Set every property to default
        state = handState.idle;
        rigidbody.gravityScale = gravityScale;
        rigidbody.mass = mass;
        retrieveComplete = false;
        Vector2 fireVector = Vector2.zero;

        playerPosition = player.transform.position;
        gameObject.SetActive(true);

        // Then fire
        switch (playerController.getDir())
        {
            case 1:
                playerPosition.x += 1;
                gameObject.transform.position = playerPosition;
                fireVector = new Vector2(5 + power, 15 + power);
                break;
            case -1:
                playerPosition.x -= 1;
                gameObject.transform.position = playerPosition;
                fireVector = new Vector2(-5 - power, 15 + power);
                break;
        }

        rigidbody.AddForce(fireVector, ForceMode2D.Impulse);
    }

    public void StartRetrieve()
    {
        // Trigger Retrieve() in the Update()
        state = handState.moving;
        boxCollider.isTrigger = true;
        rigidbody.gravityScale = 0f;
        rigidbody.mass = 0f;
    }

    void Retrieve()
    {
        playerPosition = player.transform.position;

        if (state == handState.moving)
        {
            Vector2 temp = new Vector2(transform.position.x, transform.position.y);
            Vector2 diff = playerPosition - temp;
            Vector2 direction = diff.normalized;
            Vector2 movement = direction * retrieveSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            if (diff.magnitude < retreiveRadius)
            {
                rigidbody.gravityScale = gravityScale;
                rigidbody.mass = mass;
                boxCollider.isTrigger = false;
                gameObject.SetActive(false);
                state = handState.idle;
                retrieveComplete = true;
            }
        }
    }

    public bool RetreiveComplete()
    {
        return retrieveComplete;
    }

    void Move()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.z -= 10;
        camera.transform.position = cameraPosition;

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, 0);

        if (movement.x > 0)
        {
            dir = 1;
            lastDir = 1;
        }
        else if (movement.x < 0)
        {
            dir = -1;
            lastDir = -1;
        }
        else if (movement.x == 0)
        {
            dir = 0;
        }

        rigidbody.transform.Translate(movement);
    }

    void AnimationControl()
    {
        switch(dir) {
            case 1:
                anim.Play("move_right");
                break;
            case -1:
                anim.Play("move_left");
                break;
            case 0:
                if (lastDir == 1) anim.Play("idle_right");
                if (lastDir == -1) anim.Play("idle_left");
                break;
        }
    }

    public bool getControlling()
    {
        return controlling;
    }

    public void setControlling(bool input)
    {
        controlling = input;
    }
}
