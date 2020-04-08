using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movePower = 2f;
    public float jumpPower = 8f;

    private bool isJumping = false, isGrounded = false, isCharging = false, canShoot = false;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Coroutine readyShootCoroutine;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckState();
        if (isGrounded)
        {
            Shoot();
            if (isCharging) return;

            if (!isJumping) Jump();
            HorizontalMove();

        }
        else
        {
            HorizontalMove();
        }

    }

    void CheckState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.9f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    void HorizontalMove()
    {
        Vector3 moveVelocity = new Vector3(0f, 0f, 0f);

        if (Input.GetAxisRaw("Horizontal") < 0)
            moveVelocity = Vector3.left * movePower;
        if (Input.GetAxisRaw("Horizontal") > 0)
            moveVelocity = Vector3.right * movePower;

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        rb2d.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rb2d.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isJumping = true;
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("IN");
            readyShootCoroutine = StartCoroutine(ReadyShoot());
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopCoroutine(readyShootCoroutine);
            isCharging = false;
            if (canShoot)
            {
                Debug.Log("발사!");
                canShoot = false;
            }
            else
            {
                Debug.Log("노발사..");
            }

            Color color = spriteRenderer.color;
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            spriteRenderer.color = color;
        }
    }


    // 임시로, 기모으는 중은 색이 점점 붉어지며, 기 모으는게 끝나면(canShoot=true) 초록색으로,
    // 기 모으다 취소하면 원래 색으로 돌아가도록 표시한다.
    IEnumerator ReadyShoot()
    {
        isCharging = true;
        Color color = spriteRenderer.color;
        int count = 0;

        Debug.Log("기모으는중..." + count.ToString());
        while (count++ < 5)
        {
            Debug.Log("기모으는중..." + count.ToString());
            color.g -= 0.1f;
            color.b -= 0.1f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.5f);
        }

        color.r = 0f;
        color.g = 1f;
        color.b = 0f;
        spriteRenderer.color = color;

        isCharging = false;
        canShoot = true;
    }
}
