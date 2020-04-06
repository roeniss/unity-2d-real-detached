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

    private float gravityScale;
    private float mass;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private Vector2 playerPosition;
    private enum handState { idle, moving };
    private handState state;
    private handState lastState;
    private bool controlling;

    void Start()
    {
        gameObject.SetActive(false);
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScale = rigidbody.gravityScale;
        mass = rigidbody.mass;
        playerPosition = player.transform.position;
        state = handState.idle;
        lastState = handState.idle;
        controlling = false;
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
    }

    public void Fire(float power)
    {
        gameObject.SetActive(true);
        Vector2 fireVector = Vector2.zero;
        playerPosition = player.transform.position;

        switch (playerController.getDir())
        {
            case 1:
                gameObject.transform.position = playerPosition;
                fireVector = new Vector2(200 + power, 300 + power);
                break;
            case -1:
                gameObject.transform.position = playerPosition;
                fireVector = new Vector2(-200 - power, 300 + power);
                break;
        }

        rigidbody.AddForce(fireVector, ForceMode2D.Impulse);
    }

    public void StartRetrieve()
    {
        state = handState.moving;
        lastState = handState.moving;
        boxCollider.isTrigger = true;
        rigidbody.gravityScale = 0f;
        rigidbody.mass = 0f;
    }

    public void Retrieve()
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
            }
        }
    }

    public bool RetreiveComplete()
    {
        if (lastState == handState.moving)
        {
            if (state == handState.idle)
            {
                lastState = handState.idle;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public void Move()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.z -= 10;
        camera.transform.position = cameraPosition;

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, 0);
        rigidbody.transform.Translate(movement);
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
