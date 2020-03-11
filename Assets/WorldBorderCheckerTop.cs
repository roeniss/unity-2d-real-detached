﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorderCheckerTop : MonoBehaviour {
    public MainCameraScript mainCamera;

    void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<MainCameraScript>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("TopBorder in");
            mainCamera.StopMoveUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("TopBorder out");
            mainCamera.StartMoveUp();
        }
    }
}