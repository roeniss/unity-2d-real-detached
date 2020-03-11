using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorderCheckerLeft : MonoBehaviour {
    public MainCameraScript mainCamera;

    void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<MainCameraScript>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("LeftBorder in");
            mainCamera.StopMoveLeft();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("LeftBorder out");
            mainCamera.StartMoveLeft();
        }
    }
}