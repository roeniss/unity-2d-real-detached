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

    private void Start()
    {
        activated = false;
    }

    private void Update()
    {
        PlayerCheck();
        ActivateJumpPad();
    }

    private void PlayerCheck()
    {
        playerOnPad = Physics2D.OverlapCircle(playerCheck.transform.position, playerCheckRadius, LayerMask.GetMask("Player"));
    }

    private void ActivateJumpPad()
    {
        if (switchController.getActivated() && playerOnPad && !activated)
        {
            playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            activated = true;
            Invoke("Deactivate", 0.5f);
        }
    }

    private void Deactivate() { activated = false; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.transform.position, playerCheckRadius);
    }
}
