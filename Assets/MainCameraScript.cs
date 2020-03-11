using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {
    public GameObject target;

    public int smoothness = 2;
    // public float offsetY = 1;

    private bool canGoLeft = true, canGoRight = true, canGoUp = true, canGoDown = true;
    private Vector3 lastTargetPos;
    void Start() { }

    void FixedUpdate() {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        if (IsBlockedX() && IsBlockedY()) {
            targetPos.x = lastTargetPos.x;
            targetPos.y = lastTargetPos.y;
            lastTargetPos.z = targetPos.z;
        }
       else  if (IsBlockedX()) {
            targetPos.x = lastTargetPos.x;
            lastTargetPos.y = targetPos.y;
            lastTargetPos.z = targetPos.z;
        }else if (IsBlockedY()) {
            lastTargetPos.x = targetPos.x;
            targetPos.y = lastTargetPos.y;
            lastTargetPos.z = targetPos.z;
        }else{
            lastTargetPos = targetPos;
        }
        // transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothness);
        transform.position = targetPos;
    }


    private bool IsBlockedX() {
        return !canGoLeft || !canGoRight;
    }

    private bool IsBlockedY() {
        return !canGoDown || !canGoUp;
    }

    public void StopMoveLeft() {
        canGoLeft = false;
    }

    public void StartMoveLeft() {
        canGoLeft = true;
    }

    public void StopMoveRight() {
        canGoRight = false;
    }

    public void StartMoveRight() {
        canGoRight = true;
    }

    public void StopMoveUp() {
        canGoUp = false;
    }

    public void StartMoveUp() {
        canGoUp = true;
    }

    public void StopMoveDown() {
        canGoDown = false;
    }

    public void StartMoveDown() {
        canGoDown = true;
    }
}