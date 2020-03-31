using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public float speed;
    public float retreiveRadius;
    private float gravityScale;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private Vector2 playerPosition;
    private enum handState { idle, moving };
    private handState state;
    private handState lastState;

    void Start()
    {
        gameObject.SetActive(false);
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScale = rigidbody.gravityScale;
        playerPosition = player.transform.position;
        state = handState.idle;
        lastState = handState.idle;
    }

    void Update()
    {
        Move();
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
                fireVector = new Vector2(200 + power, 200 + power);
                break;
            case -1:
                gameObject.transform.position = playerPosition;
                fireVector = new Vector2(-200 - power, 200 + power);
                break;
        }

        rigidbody.AddForce(fireVector, ForceMode2D.Impulse);
    }

    public void Retreive()
    {
        state = handState.moving;
        lastState = handState.moving;
        boxCollider.isTrigger = true;
        rigidbody.gravityScale = 0f;
    }

    public void Move()
    {
        playerPosition = player.transform.position;

        if (state == handState.moving)
        {
            Vector2 temp = new Vector2(transform.position.x, transform.position.y);
            Vector2 diff = playerPosition - temp;
            Vector2 direction = diff.normalized;
            Vector2 movement = direction * speed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            if (diff.magnitude < retreiveRadius)
            {
                rigidbody.gravityScale = gravityScale;
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
            if (state == handState.idle) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }
}
