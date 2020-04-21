using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject unplugged_switch, plugged_green, plugged_red;
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
        SpriteControl();
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
                // Activating switch
                if (!activated)
                {
                    activated = true;
                    Invoke("Deactivate", 0.5f);
                }
            }
            else
            {
                // Plugging into switch
                if (handAround && !plugged)
                {
                    plugged = true;
                }
            }
        }
    }

    private void Deactivate() {
        activated = false;
        plugged_green.SetActive(false);
        plugged_red.SetActive(true);
    }

    private void SpriteControl()
    {
        if (plugged)
        {
            if (activated)
            {
                plugged_red.SetActive(false);
                plugged_green.SetActive(true);
                unplugged_switch.SetActive(false);
            }
            else
            {
                plugged_red.SetActive(true);
                plugged_green.SetActive(false);
                unplugged_switch.SetActive(false);
            }
        }
        else
        {
            plugged_red.SetActive(false);
            plugged_green.SetActive(false);
            unplugged_switch.SetActive(true);
        }
    }

    public bool getActivated() { return activated; }

    public bool getPlugged() { return plugged; }

    public void setPlugged(bool plugged) { this.plugged = plugged; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(handCheck.transform.position, handCheckRadius);
    }
}
