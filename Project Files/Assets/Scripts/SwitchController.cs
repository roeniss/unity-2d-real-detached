using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject unplugged_switch;
    public GameObject plugged_green;
    public GameObject plugged_red;
    public GameObject handCheck;
    public float handCheckRadius;
    private bool plugged;
    private bool handAround;
    private bool activated;

    void Start()
    {
        activated = false;
        plugged_green.SetActive(false);
        plugged_red.SetActive(false);
    }

    void Update()
    {
        HandCheck();
        ActivateSwitch();
    }

    private void HandCheck()
    {
        handAround = Physics2D.OverlapCircle(handCheck.transform.position, handCheckRadius, LayerMask.GetMask("Hand"));
    }

    private void ActivateSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (plugged)
            {
                if (activated)
                {
                    activated = false;
                    plugged_green.SetActive(false);
                    plugged_red.SetActive(true);
                }
                else
                {
                    activated = true;
                    plugged_green.SetActive(true);
                    plugged_red.SetActive(false);
                }
            }
            else
            {
                if (handAround && !plugged)
                {
                    plugged = true;
                    plugged_red.SetActive(true);
                    unplugged_switch.SetActive(false);
                }
            }
        }
    }

    public bool getActivated() { return activated; }

    public bool getPlugged() { return plugged; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(handCheck.transform.position, handCheckRadius);
    }
}
