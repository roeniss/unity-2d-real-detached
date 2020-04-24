using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject switch_on, switch_off;
    public GameObject handCheck;
    public PlayerController player;
    public HandController leftHand, rightHand;
    public float handCheckRadius;

    private Rigidbody2D leftRigid, rightRigid;

    private bool switchStatus;
    private bool leftTrigger, rightTrigger, Trigger;
    private bool leftPlugged, rightPlugged;
    public bool playerRetrieve;

    void Start()
    {
        switchStatus = false;
        leftPlugged = false;
        rightPlugged = false;
        switch_on.SetActive(false);
        switch_off.SetActive(true);

        leftRigid = leftHand.GetComponent<Rigidbody2D>();
        rightRigid = rightHand.GetComponent<Rigidbody2D>();

    }

    
    void Update()
    {
        playerRetrieve = player.getControlling() && Input.GetKeyDown(KeyCode.R); 
        HandCheck();
        ActivateSwitch();
        DeactiveSwitch();
    }

    public bool GetSwitchActive()
    {
        return switchStatus;
    }

    private void HandCheck()
    {
        leftTrigger= Physics2D.OverlapCircle(handCheck.transform.position, handCheckRadius, LayerMask.GetMask("Left Hand"));
        rightTrigger= Physics2D.OverlapCircle(handCheck.transform.position, handCheckRadius, LayerMask.GetMask("Right Hand"));
        Trigger = leftTrigger || rightTrigger;
    }

    private void ActivateSwitch()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Activate switch
            if (Trigger && !switchStatus)
            {
                if (leftTrigger)
                {
                    leftHand.GetComponent<SpriteRenderer>().enabled = false;
                    leftHand.setMovable(false);
                    leftPlugged = true;
                }
                else if (rightTrigger)
                {
                    rightHand.GetComponent<SpriteRenderer>().enabled = false;
                    rightHand.setMovable(false);
                    rightPlugged = true;
                }

                switch_on.SetActive(true);
                switch_off.SetActive(false);
                switchStatus = true;
            }
            //Deactivate switch
            else if (switchStatus)
            {
                Vector2 switchPos = transform.position;

                if (leftPlugged && leftHand.getControlling())
                {
                    leftHand.GetComponent<SpriteRenderer>().enabled = true;
                    leftHand.setMovable(true);

                    leftHand.transform.position = switchPos;
                    leftRigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
                    leftPlugged = false;

                    switch_on.SetActive(false);
                    switch_off.SetActive(true);
                    switchStatus = false;
                }
                else if (rightPlugged && rightHand.getControlling())
                {
                    rightHand.GetComponent<SpriteRenderer>().enabled = true;
                    rightHand.setMovable(true);

                    rightHand.transform.position = switchPos;
                    rightRigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
                    rightPlugged = false;

                    switch_on.SetActive(false);
                    switch_off.SetActive(true);
                    switchStatus = false;
                }
            }
        }        
    }

    private void DeactiveSwitch()
    {
        if (playerRetrieve)
        {
            if (switchStatus)
            {
                Vector2 switchPos = transform.position;

                if (leftPlugged)
                {
                    leftHand.GetComponent<SpriteRenderer>().enabled = true;
                    leftHand.setMovable(true);

                    leftHand.transform.position = switchPos;
                    leftRigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
                    leftPlugged = false;

                    switch_on.SetActive(false);
                    switch_off.SetActive(true);
                    switchStatus = false;
                }
                else if (rightPlugged)
                {
                    rightHand.GetComponent<SpriteRenderer>().enabled = true;
                    rightHand.setMovable(true);

                    rightHand.transform.position = switchPos;
                    rightRigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
                    rightPlugged = false;

                    switch_on.SetActive(false);
                    switch_off.SetActive(true);
                    switchStatus = false;
                }
            }

        }
    }

}
