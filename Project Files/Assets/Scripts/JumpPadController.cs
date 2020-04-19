using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public GameObject playerCheck;
    public SwitchController switchController;
    public float playerCheckRadius;
    public float jumpPower;
    private bool playerOnPad;
    private bool activated;

    void Start()
    {
        activated = false;
    }

    void Update()
    {
        PlayerCheck();
        ActivateJumpPad();
    }

    void PlayerCheck()
    {
        playerOnPad = Physics2D.OverlapCircle(playerCheck.transform.position, playerCheckRadius, LayerMask.GetMask("Player"));

        if (!playerOnPad && activated)
        {
            activated = false;
        }
    }

    void ActivateJumpPad()
    {
        if (switchController.getActivated() && playerOnPad && !activated)
        {
            activated = true;
            playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.transform.position, playerCheckRadius);
    }
}
